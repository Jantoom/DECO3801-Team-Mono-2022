using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public static readonly float GENERATION_DELAY = 2.0f, GENERATION_COOLDOWN = 1.0f, GENERATION_SPEED = 2.0f;
    public static readonly int NUM_ROWS_PER_TERRAIN = 10, MAX_TERRAINS_PER_MAP = 5, MAX_ACTIVE_ROWS = 12;
    [SerializeField] private GameObject[] terrainPrefabs;
    [SerializeField] private RuntimeAnimatorController animController;
    private GameObject mapObj, cameraObj;
    private bool isGenerating = false;
    public bool IsGenerating { get => isGenerating; }
    private Queue<GameObject> activeRows = new();

    void Start()
    {
        mapObj = GameObject.Find("Map");
        cameraObj = GameObject.Find("Main Camera");
        StartCoroutine(GenerateMap());
    }

    void Update() {
        if (isGenerating) {
            cameraObj.transform.Translate(Vector3.forward * GENERATION_COOLDOWN * Time.deltaTime, Space.World);
        }
    }

    private IEnumerator GenerateMap() {
        yield return GenerateInitialTerrain();
        isGenerating = true;
        yield return GenerateTerrain();
    }

    private IEnumerator GenerateInitialTerrain() {
        // Only valid starting prefab so far
        var terrainPref = terrainPrefabs[0]; // PickNextRandomValidTerrain();
        var terrainObj = new GameObject($"Terrain{0} ({terrainPref.name})");
        terrainObj.transform.SetParent(mapObj.transform, false);
        terrainObj.transform.localPosition = Vector3.forward * NUM_ROWS_PER_TERRAIN * 0;

        for (var rowCount = 0; rowCount < NUM_ROWS_PER_TERRAIN; rowCount++) {
            GenerateRow(terrainPref, terrainObj, rowCount);
        }
        yield return new WaitForSeconds(GENERATION_DELAY);
    }

    private IEnumerator GenerateTerrain() {
        for (var terrainCount = 1; terrainCount < MAX_TERRAINS_PER_MAP; terrainCount++) {
            var terrainPref = PickNextRandomValidTerrain();
            var terrainObj = new GameObject($"Terrain{terrainCount} ({terrainPref.name})");
            terrainObj.transform.SetParent(mapObj.transform, false);
            terrainObj.transform.localPosition = Vector3.forward * NUM_ROWS_PER_TERRAIN * terrainCount;

            for (var rowCount = 0; rowCount < NUM_ROWS_PER_TERRAIN; rowCount++) {
                GenerateRow(terrainPref, terrainObj, rowCount);
                yield return new WaitForSeconds(GENERATION_COOLDOWN);
            }
        }
        isGenerating = false;
    }

    private void GenerateRow(GameObject terrainPref, GameObject terrainObj, int rowCount) {
        var rowPref = terrainPref.transform.GetChild(rowCount).gameObject;
        var rowObj = new GameObject($"Row{rowCount}");
        rowObj.transform.SetParent(terrainObj.transform, false);
        rowObj.transform.localPosition = Vector3.forward * rowCount;

        for (var cellCount = 0; cellCount < rowPref.transform.childCount; cellCount++) {
            var cellPref = rowPref.transform.GetChild(cellCount).gameObject;
            var cellObj = Instantiate(cellPref, rowObj.transform, false);
            cellObj.name = $"Cell{cellCount}";
            cellObj.transform.localPosition = Vector3.right * cellCount;
            // Add extra Animator Controller for load in animation
            var animator = cellObj.AddComponent<Animator>();
            animator.runtimeAnimatorController = animController;
            animator.applyRootMotion = true;
        }
        activeRows.Enqueue(rowObj);
        if (activeRows.Count > MAX_ACTIVE_ROWS) {
            StartCoroutine(DegenerateRow(activeRows.Dequeue()));
        }
    }

    private IEnumerator DegenerateRow(GameObject rowObj) {
        var terrainObj = rowObj.transform.parent.gameObject;

        for (var cellCount = 0; cellCount < rowObj.transform.childCount; cellCount++) {
            var cellObj = rowObj.transform.GetChild(cellCount).gameObject;
            cellObj.GetComponent<Animator>().Play("Despawn");
        }
        yield return new WaitForSeconds(GENERATION_SPEED);

        Destroy(rowObj);
        if (terrainObj.transform.childCount == 1) {
            Destroy(terrainObj);
        }
    }

    private GameObject PickNextRandomValidTerrain() {
        return terrainPrefabs[Random.Range(1, terrainPrefabs.Length)];
    }

    public Queue<GameObject> getActiveRows() {
        // Returning copy of queue so PowerupSpawner queue traversal
        // doesn't affect the TerrainGenerator
        return new Queue<GameObject>(activeRows);
    }
}