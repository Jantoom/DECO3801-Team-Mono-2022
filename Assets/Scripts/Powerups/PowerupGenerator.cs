using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Responsible for spawning powerups in the scene.
public class PowerupGenerator : MonoBehaviour
{
    // List of spawnable prefabs
    [field: SerializeField] public List<GameObject> PowerupsTierOne { get; private set; }
    [field: SerializeField] public List<GameObject> PowerupsTierTwo { get; private set; }
    // Powerup generation
    private TerrainGenerator _terrainGenerator;
    private static readonly float SPAWN_SPEED = 2.0f;
    // Reference to players for logic
    private PlayerInfo playerOne, playerTwo;

    void Start()
    {
        playerOne = GetComponent<GameInfo>().PlayerOne.GetComponent<PlayerInfo>();
        playerTwo = GetComponent<GameInfo>().PlayerTwo.GetComponent<PlayerInfo>();
        InvokeRepeating("SpawnPowerups", SPAWN_SPEED, SPAWN_SPEED);
    }
    //
    // Summary:
    //    Checks the state of the game and spawns powerups accordingly.
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
    //
    // Summary:
    //     Finds the appropriate cell to spawn a powerup given the imbalance between the players.
    //
    // Returns:
    //     The location of powerup spawn.
    private Transform FindEmptyCell() {
        List<Transform> emptyCells;
        if (playerOne.Health == playerTwo.Health) {
            emptyCells = GetAllEmptyCells();
        } else if (playerOne.Health < playerTwo.Health) {
            emptyCells = GetClosestEmptyCellsToPlayer(playerOne.transform, playerTwo.transform);
        } else {
            emptyCells = GetClosestEmptyCellsToPlayer(playerTwo.transform, playerOne.transform);
        }
        return emptyCells[Random.Range(0, emptyCells.Count)];
    }
    //
    // Summary:
    //     Finds all empty cells within the active terrain.
    //
    // Returns:
    //     A list of all empty cells.
    private List<Transform> GetAllEmptyCells() {
        var activeRows = _terrainGenerator.ActiveRows.ToList();
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
    //
    // Summary:
    //     Filters through all empty cells in the terrain to pick ones that favour the player
    //     behind in the game. This is defined by how many lives they have.
    //
    // Parameters:
    //   behind:
    //     Location of the player behind in the game
    //   ahead:
    //     Location of the player ahead in the game
    //
    // Returns:
    //     A list of favourable empty cells.
    private List<Transform> GetClosestEmptyCellsToPlayer(Transform behind, Transform ahead) {
        var activeRows = _terrainGenerator.ActiveRows.ToList();
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
    //
    // Summary:
    //     Picks a random powerup to spawn.
    //
    // Parameters:
    //   powerups:
    //     List of powerups to pick from. This is typically between tier 1 and 2 powerups.
    //
    // Returns:
    //     A randomly selected powerup.
    private GameObject GetRandomPowerup(List<GameObject> powerups) {
        return powerups[Random.Range(0, powerups.Count)];
    }
}
