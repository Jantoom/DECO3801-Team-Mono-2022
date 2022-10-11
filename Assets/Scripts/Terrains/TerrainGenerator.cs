using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public static readonly int MAX_TERRAINS_PER_MAP = 1, MAX_ACTIVE_ROWS = 12;
    public static readonly float GENERATION_DELAY = 2.0f, GENERATION_SPEED = 1.0f, DEGENERATION_SPEED = 0.4f;
    [field: SerializeField] public bool IsGenerating { get; private set; } = false;
    [SerializeField] private GameObject[] randomTerrains;
    [field: SerializeField] public int RowsGenerated { get; private set; } = 0;
    [SerializeField] private RuntimeAnimatorController spawnAnimator;
    [SerializeField] private GameObject startTerrain, finishTerrain;
    public Queue<Transform> ActiveRows = new();

    void Awake()
    {
        StartCoroutine(GenerateMap());
    }

    private IEnumerator GenerateMap() {
        var map = new GameObject("Map").transform;
        map.position = new Vector3(0.0f, -0.5f, 0.0f);

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
        var terrain = new GameObject($"{id} ({prefab.name})").transform;
        terrain.SetParent(map, false);
        terrain.localPosition = Vector3.forward * RowsGenerated;

        for (var count = 0; count < prefab.childCount; count++) {
            GenerateRow(terrain, prefab.GetChild(count), count);
            if (!instant) yield return new WaitForSeconds(GENERATION_SPEED);
        }
    }

    private void GenerateRow(Transform terrain, Transform prefab, int id) {
        var row = new GameObject(prefab.name).transform;
        row.SetParent(terrain, false);
        row.localPosition = prefab.localPosition;

        for (var count = 0; count < prefab.childCount; count++) {
            GenerateCell(row, prefab.GetChild(count), count);
        }
        RowsGenerated++;

        ActiveRows.Enqueue(row);
        if (ActiveRows.Count > MAX_ACTIVE_ROWS) {
            StartCoroutine(DegenerateRow(ActiveRows.Dequeue()));
        }
    }

    private void GenerateCell(Transform row, Transform prefab, int id) {
        var cell = Instantiate(prefab.gameObject, row, false).transform;
        cell.name = prefab.name;
        cell.localPosition = prefab.localPosition;
        // Add extra Animator Controller for load in animation
        var animator = cell.gameObject.AddComponent<Animator>();
        animator.runtimeAnimatorController = spawnAnimator;
        animator.applyRootMotion = true;
    }

    private IEnumerator DegenerateRow(Transform row) {
        var terrain = row.parent;

        foreach (Transform cell in row) {
            cell.gameObject.GetComponent<Animator>().Play("Despawn");
        }
        yield return new WaitForSeconds(DEGENERATION_SPEED);
        foreach (Transform cell in row) {
            foreach (Transform obj in cell) {
                var rigidbody = obj.gameObject.GetComponent<Rigidbody>();
                rigidbody.useGravity = true;
                rigidbody.constraints = RigidbodyConstraints.None;
                if (rigidbody.CompareTag("Tile")) {
                    rigidbody.AddExplosionForce(5.0f, obj.position + new Vector3(Random.Range(-0.5f, 0.5f), 1.0f, Random.Range(-0.25f, 0.5f)), 0.5f, 0.0f, ForceMode.Impulse);
                } else {
                    rigidbody.AddExplosionForce(5.0f, obj.position + new Vector3(Random.Range(-0.5f, 0.5f), 0.0f, Random.Range(-0.25f, 0.5f)), 0.5f, 0.0f, ForceMode.Impulse);
                }
                
            }
            
        }
    }

    private GameObject PickNextRandomValidTerrain() {
        return randomTerrains[Random.Range(1, randomTerrains.Length)];
    }

    public Queue<Transform> getActiveRows() {
        // Returning copy of queue so PowerupSpawner queue traversal
        // doesn't affect the TerrainGenerator
        return new Queue<Transform>(ActiveRows);
    }
}