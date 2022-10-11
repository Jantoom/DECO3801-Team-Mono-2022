using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PowerupSpawner : MonoBehaviour
{
    public TerrainGenerator terrainGenerator;
    private Queue<GameObject> activeRows;
    private float SPAWN_DELAY = 5.0f;
    private List<GameObject> powerupsTierOne;
    private List<GameObject> powerupsTierTwo;
    private PlayerInfo playerOne, playerTwo;

    // Start is called before the first frame update
    void Start()
    {
        terrainGenerator = GetComponent<TerrainGenerator>();
        playerOne = GameObject.Find("Player1").GetComponent<PlayerInfo>();
        playerTwo = GameObject.Find("Player1").GetComponent<PlayerInfo>();
        powerupsTierOne = Resources.LoadAll<GameObject>("Prefabs/Powerups/Tier1").ToList();
        powerupsTierTwo = Resources.LoadAll<GameObject>("Prefabs/Powerups/Tier2").ToList();
        InvokeRepeating("SpawnPowerups", SPAWN_DELAY, SPAWN_DELAY);
    }

    private void SpawnPowerups() {
        Transform emptyCell = FindEmptyCell();
        GameObject randomPowerup = GetRandomPowerup(powerupsTierTwo);
        Debug.Log(randomPowerup.name);
        Instantiate(randomPowerup, emptyCell.transform.position + new Vector3(0.0f, 0.5f, 0.0f), Quaternion.identity, emptyCell.transform);
    }

    private Transform FindEmptyCell() {
        List<Transform> emptyCells;
        if (playerOne.Health == playerTwo.Health) {
            emptyCells = GetAllEmptyCells();
        } else if (playerOne.Health < playerTwo.Health) {
            emptyCells = GetClosestEmptyCellsToPlayer(playerOne.transform, playerTwo.transform);
        } else {
            emptyCells = GetClosestEmptyCellsToPlayer(playerTwo.transform, playerOne.transform);
        }
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
                    if (cell.transform.childCount == 1) {
                        emptyCells.Add(cell);
                    }
                }
            }
        }
        return emptyCells;
    }

    private List<Transform> GetClosestEmptyCellsToPlayer(Transform behind, Transform ahead) {
        activeRows = terrainGenerator.getActiveRows();
        List<Transform> emptyCells = new List<Transform>();
        int rowNumber = 0;
        foreach (GameObject row in activeRows) {
            // Do not want to spawn in the first 3 rows
            if (rowNumber++ > 2) {
                foreach (Transform cell in row.transform) {
                    if (cell.transform.childCount == 1) {
                        // Only adds cell to list if it is closer to the player behind than the one in front
                        float distanceToAhead = Vector3.Distance(cell.position, ahead.position);
                        float distanceToBehind = Vector3.Distance(cell.position, behind.position);
                        if (distanceToBehind < distanceToAhead) {
                            emptyCells.Add(cell);
                        }
                    }
                }
            }
        }
        return emptyCells;
    }

    private GameObject GetRandomPowerup(List<GameObject> powerups) {
        return powerups[Random.Range(0, powerups.Count - 1)];
    }
}
