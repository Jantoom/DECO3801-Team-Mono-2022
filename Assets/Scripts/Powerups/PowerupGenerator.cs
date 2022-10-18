using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PowerupGenerator : MonoBehaviour
{
    public TerrainGenerator terrainGenerator;
    private Queue<Transform> activeRows;
    private float SPAWN_DELAY = 2.0f;
    [field: SerializeField] public List<GameObject> PowerupsTierOne { get; private set; }
    [field: SerializeField] public List<GameObject> PowerupsTierTwo { get; private set; }
    private PlayerInfo playerOne, playerTwo;

    // Start is called before the first frame update
    void Start()
    {
        terrainGenerator = GetComponent<TerrainGenerator>();
        playerOne = GameObject.Find("Player1").GetComponent<PlayerInfo>();
        playerTwo = GameObject.Find("Player2").GetComponent<PlayerInfo>();
        InvokeRepeating("SpawnPowerups", SPAWN_DELAY, SPAWN_DELAY);
    }

    private void SpawnPowerups() {
        if (!GameOverInfo.isGameOver) {
            Transform emptyCell = FindEmptyCell();
            GameObject randomPowerup = null;
            int rand = Random.Range(1, 101);
            if (playerOne.Health != playerTwo.Health) {
                // This runs if a player is behind on lives
                // 60% chance no powerup spawns
                // 20% chance tier one powerup spawns
                // 20% chance tier two powerup spawns
                if (rand > 60 && rand < 80) {
                    randomPowerup = GetRandomPowerup(PowerupsTierOne);
                } else if (rand > 80) {
                    randomPowerup = GetRandomPowerup(PowerupsTierTwo);
                }
            } else {
                // This runs if players have the same amount of lives
                // 80% chance no powerup spawns
                // 20% chance tier one powerup spawns
                if (rand >= 80) {
                    randomPowerup = GetRandomPowerup(PowerupsTierOne);
                }
            }
            if (randomPowerup != null) {
                Instantiate(randomPowerup, emptyCell.transform.position + new Vector3(0.0f, 0.5f, 0.0f), Quaternion.identity, emptyCell.transform);
            }
        }
    }

    private Transform FindEmptyCell() {
        List<Transform> emptyCells;
        if (playerOne.Health == playerTwo.Health) {
            emptyCells = GetAllEmptyCells();
        }
        else if (playerOne.Health < playerTwo.Health) {
            emptyCells = GetClosestEmptyCellsToPlayer(playerOne.transform, playerTwo.transform);
        } else {
            emptyCells = GetClosestEmptyCellsToPlayer(playerTwo.transform, playerOne.transform);
        }
        return emptyCells[Random.Range(0, emptyCells.Count)];
    }

    private List<Transform> GetAllEmptyCells() {
        activeRows = terrainGenerator.getActiveRows();
        List<Transform> emptyCells = new List<Transform>();
        int rowNumber = 0;
        foreach (Transform row in activeRows) {
            // Do not want to spawn in the first 5 rows
            if (rowNumber++ > 5) {
                foreach (Transform cell in row) {
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
        foreach (Transform row in activeRows) {
            // Do not want to spawn in the first 5 rows
            if (rowNumber++ > 5) {
                foreach (Transform cell in row) {
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
        return powerups[Random.Range(0, powerups.Count)];
    }
}
