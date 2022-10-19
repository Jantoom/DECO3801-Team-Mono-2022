using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Responsible for spawn terrain within the game. It does this at a consistent rate until all
// wanted terrains are generated, and then the final terrain with the finish line is generated.
public class TerrainGenerator : MonoBehaviour
{
    private GameInfo _gameInfo;
    // Generation constants
    public static readonly int MAX_TERRAINS_PER_MAP = 3, MAX_ACTIVE_ROWS = 12;
    public static readonly float GENERATION_DELAY = 2.0f, GENERATION_SPEED = 2.0f, DEGENERATION_SPEED = 0.4f;
    // For synchronous object movement and other generators
    [field: SerializeField] public bool IsGenerating { get; private set; } = false;
    [field: SerializeField] public int RowsGenerated { get; private set; } = 0;
    private float _generationMultiplier = 1.0f;
    public float GenerationSpeed { get => GENERATION_SPEED * _generationMultiplier; }
    // Generation prefabs and components
    [field: SerializeField] public RuntimeAnimatorController SpawnAnimator { get; private set; }
    [SerializeField] private GameObject startTerrain, finishTerrain;
    [SerializeField] private GameObject[] randomTerrains;
    // Rows that have generated and not fallen
    public Queue<Transform> ActiveRows = new();

    void Awake()
    {
        _gameInfo = GetComponent<GameInfo>();
        StartCoroutine(GenerateMap());
    }
    //
    // Summary:
    //     Generates the entire map from start to finish.
    //
    // Returns:
    //     The enumerator for this coroutine.
    private IEnumerator GenerateMap()
    {
        // Initialise parent map object
        var map = new GameObject("Map").transform;
        map.position = new Vector3(0.0f, -0.5f, 0.0f);
        // Generate starting terrain immediately
        yield return GenerateTerrain(map, startTerrain.transform, 0, true);
        yield return new WaitForSeconds(GENERATION_DELAY);
        // Generate rest of terrains one row at a time
        IsGenerating = true;
        for (var count = 1; count < MAX_TERRAINS_PER_MAP; count++) {
            yield return GenerateTerrain(map, PickNextRandomValidTerrain().transform, count, false);
        }
        // Generate final terrain one row at a time
        yield return GenerateTerrain(map, finishTerrain.transform, MAX_TERRAINS_PER_MAP, false);
        IsGenerating = false;
    }
    //
    // Summary:
    //     Generates an entire terrain prefab.
    //
    // Parameters:
    //   map:
    //     Parent transform for the terrain
    //   prefab:
    //     Container for rows to generate
    //   id:
    //     Identification of the terrain
    //   instant:
    //     Will generate rows instantly if true, otherwise generates at generation speed.
    //
    // Returns:
    //     The enumerator for this coroutine.
    private IEnumerator GenerateTerrain(Transform map, Transform prefab, int id, bool instant)
    {
        var terrain = new GameObject($"{id} ({prefab.name})").transform;
        terrain.SetParent(map, false);
        terrain.localPosition = Vector3.forward * RowsGenerated;

        for (var count = 0; count < prefab.childCount; count++) {
            GenerateRow(terrain, prefab.GetChild(count));
            if (!instant) {
                _generationMultiplier = DistanceFrontPlayerToFrontRow() / MAX_ACTIVE_ROWS;
                yield return new WaitForSeconds(GenerationSpeed);
            }
        }
    }
    //
    // Summary:
    //     Generates an entire row prefab.
    //
    // Parameters:
    //   terrain:
    //     Parent transform for the row
    //   prefab:
    //     Container for cells to generate
    //
    // Returns:
    //     The enumerator for this coroutine.
    private void GenerateRow(Transform terrain, Transform prefab)
    {
        var row = new GameObject(prefab.name).transform;
        row.SetParent(terrain, false);
        row.localPosition = prefab.localPosition;

        for (var count = 0; count < prefab.childCount; count++) {
            GenerateCell(row, prefab.GetChild(count));
        }
        RowsGenerated++;

        ActiveRows.Enqueue(row);
        if (ActiveRows.Count > MAX_ACTIVE_ROWS) {
            StartCoroutine(DegenerateRow(ActiveRows.Dequeue()));
        }
    }
    //
    // Summary:
    //     Generates an entire cell prefab. Applies spawning animation controller.
    //
    // Parameters:
    //   row:
    //     Parent transform for the cell
    //   prefab:
    //     Container for cell objects to generate
    //
    // Returns:
    //     The enumerator for this coroutine.
    private void GenerateCell(Transform row, Transform prefab)
    {
        var cell = Instantiate(prefab.gameObject, row, false).transform;
        cell.name = prefab.name;
        cell.localPosition = prefab.localPosition;
        // Add extra Animator Controller for load in animation
        var animator = cell.gameObject.AddComponent<Animator>();
        animator.runtimeAnimatorController = SpawnAnimator;
        animator.applyRootMotion = true;
    }
    //
    // Summary:
    //     Detaches a row from the terrain, letting it fall into the void.
    //
    // Parameters:
    //   row:
    //     Row to detach from the terrain
    //
    // Returns:
    //     The enumerator for this coroutine.
    private IEnumerator DegenerateRow(Transform row)
    {
        var terrain = row.parent;
        // Play despawn animation for all cells
        foreach (Transform cell in row) {
            cell.gameObject.GetComponent<Animator>().Play("Despawn");
        }
        yield return new WaitForSeconds(DEGENERATION_SPEED);
        // Enable physics on all objects of the cells. Add explosive force for aesthetic effect.
        foreach (Transform cell in row) {
            foreach (Transform obj in cell) {
                // Stop any animations on the object
                Destroy(obj.GetComponent<Animator>());
                // Apply explosive physics for cool effect
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
    //
    // Summary:
    //     Picks a random terrain that is valid for generation considering the previous terrain.
    //
    // Returns:
    //     The next random valid terrain.
    private GameObject PickNextRandomValidTerrain()
    {
        return randomTerrains[Random.Range(0, randomTerrains.Length)];
    }
    //
    // Summary:
    //     Calculates the distance from the front player to the front row. This is useful for 
    //     tweaking the generation speed so that the player cannot be punished for travelling
    //     faster than the terrain can generate.
    //
    // Returns:
    //     How far the front-most player is from the front row.
    private float DistanceFrontPlayerToFrontRow() {
        return RowsGenerated - Mathf.Max(_gameInfo.PlayerOne.transform.position.z, 
                                         _gameInfo.PlayerTwo.transform.position.z);
    }
}