using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PowerupSpawner : MonoBehaviour
{

    public TerrainGenerator terrainGenerator;
    private Queue<GameObject> activeRows;
    private float SPAWN_DELAY = 5.0f;
    private List<GameObject> powerups;

    // Start is called before the first frame update
    void Start()
    {
        terrainGenerator = GetComponent<TerrainGenerator>();
        powerups = Resources.LoadAll<GameObject>("Prefabs/Powerups").ToList();
        InvokeRepeating("SpawnPowerups", SPAWN_DELAY, SPAWN_DELAY);
    }

    private void SpawnPowerups() {
        Debug.Log("Spawn Powerups");
        Transform emptyCell = FindEmptyCell();
        Debug.Log(emptyCell.name);
        GameObject randomPowerup = GetRandomPowerup();
        Debug.Log(randomPowerup.name);
        Instantiate(randomPowerup, emptyCell.transform.position, Quaternion.identity, emptyCell.transform);
    }

    private Transform FindEmptyCell() {
        List<Transform> emptyCells = GetAllEmptyCells();
        return emptyCells[Random.Range(0, emptyCells.Count - 1)];
    }

    private List<Transform> GetAllEmptyCells() {
        activeRows = terrainGenerator.getActiveRows();
        List<Transform> emptyCells = new List<Transform>();
        int rowNumber = 0;
        foreach (GameObject row in activeRows) {
            // Do not want to spawn in the first 3 rows
            if (rowNumber++ > 2) {
                foreach (Transform cell in row.transform) {
                    Debug.Log("Checking cell: " + cell.name);
                    if (cell.transform.childCount == 1) {
                        Debug.Log("Empty cell found: " + cell.name);
                        emptyCells.Add(cell);
                    }
                }
            }
        }
        return emptyCells;
    }

    private GameObject GetRandomPowerup() {
        return powerups[Random.Range(0, powerups.Count - 1)];
    }
}
