using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public static readonly int MAX_TERRAINS_PER_MAP = 1, MAX_ACTIVE_ROWS = 12;
    public static readonly float GENERATION_DELAY = 2.0f, GENERATION_SPEED = 1.0f, DEGENERATION_SPEED = 2.0f;
    [field: SerializeField] public bool IsGenerating { get; private set; } = false;
    [field: SerializeField] public int RowsGenerated { get; private set; } = 0;
    [SerializeField] private GameObject startTerrain, finishTerrain;
    [SerializeField] private GameObject[] randomTerrains;
    [SerializeField] private RuntimeAnimatorController spawnAnimator;
    private Queue<Transform> activeRows = new();

    void Awake()
    {
        StartCoroutine(GenerateMap());
    }

    private IEnumerator GenerateMap() {
        var map = GameObject.Find("Map").transform;

        yield return GenerateTerrain(map, startTerrain.transform, 0, true);
        yield return new WaitForSeconds(GENERATION_DELAY);

        IsGenerating = true;
        for (var count = 1; count < MAX_TERRAINS_PER_MAP; count++) {
            yield return GenerateTerrain(map, PickNextRandomValidTerrain().transform, count, false);
        }

        yield return GenerateTerrain(map, finishTerrain.transform, MAX_TERRAINS_PER_MAP, false);
        IsGenerating = false;
    }

    private IEnumerator GenerateTerrain(Transform map, Transform prefab, int id, bool instant) {
        var terrain = new GameObject($"Terrain{id} ({prefab.name})").transform;
        terrain.SetParent(map, false);
        terrain.localPosition = Vector3.forward * RowsGenerated;

        for (var count = 0; count < prefab.childCount; count++) {
            GenerateRow(terrain, prefab.GetChild(count), count);
            if (!instant) yield return new WaitForSeconds(GENERATION_SPEED);
        }
    }

    private void GenerateRow(Transform terrain, Transform prefab, int id) {
        var row = new GameObject($"Row{id}").transform;
        row.SetParent(terrain, false);
        row.localPosition = prefab.localPosition;

        for (var count = 0; count < prefab.childCount; count++) {
            GenerateCell(row, prefab.GetChild(count), count);
        }
        RowsGenerated++;

        activeRows.Enqueue(row);
        if (activeRows.Count > MAX_ACTIVE_ROWS) {
            StartCoroutine(DegenerateRow(activeRows.Dequeue()));
        }
    }

    private void GenerateCell(Transform row, Transform prefab, int id) {
        var cell = Instantiate(prefab.gameObject, row, false).transform;
        cell.name = $"Cell{id}";
        cell.localPosition = prefab.localPosition;
        // Add extra Animator Controller for load in animation
        var animator = cell.gameObject.AddComponent<Animator>();
        animator.runtimeAnimatorController = spawnAnimator;
        animator.applyRootMotion = true;
    }

    private IEnumerator DegenerateRow(Transform row) {
        var terrain = row.parent;

        for (var count = 0; count < row.childCount; count++) {
            row.GetChild(count).gameObject.GetComponent<Animator>().Play("Despawn");
        }
        yield return new WaitForSeconds(DEGENERATION_SPEED);

        Destroy(row.gameObject);
        if (terrain.childCount == 1) Destroy(terrain.gameObject);
    }

    private GameObject PickNextRandomValidTerrain() {
        return randomTerrains[Random.Range(1, randomTerrains.Length)];
    }
}