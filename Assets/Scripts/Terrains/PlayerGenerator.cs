using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerGenerator : MonoBehaviour
{
    public static readonly int SPAWN_ROW_BUFFER = 1;
    [SerializeField] private GameObject playerOnePrefab, playerTwoPrefab;
    private static TerrainGenerator terrainGenerator;

    void Start() {
        terrainGenerator = GameObject.Find("GameInfo").GetComponent<TerrainGenerator>();

        var playerOne = Instantiate(playerOnePrefab);
        var playerTwo = Instantiate(playerTwoPrefab);

        playerOne.GetComponent<PlayerLives>().heart1 = GameObject.Find("Heart1");
        playerOne.GetComponent<PlayerLives>().heart2 = GameObject.Find("Heart2");
        playerOne.GetComponent<PlayerLives>().heart3 = GameObject.Find("Heart3");
        playerTwo.GetComponent<PlayerLives>().heart1 = GameObject.Find("Heart4");
        playerTwo.GetComponent<PlayerLives>().heart2 = GameObject.Find("Heart5");
        playerTwo.GetComponent<PlayerLives>().heart3 = GameObject.Find("Heart6");

        playerOne.name = "Player1";
        playerTwo.name = "Player2";

        playerOne.GetComponent<PlayerInfo>().Opponent = playerTwo;
        playerTwo.GetComponent<PlayerInfo>().Opponent = playerOne;

        Spawn(playerOne, false);
        Spawn(playerTwo, false);
    }

    public static void Spawn(GameObject player, bool useBuffer) {
        var rows = terrainGenerator.ActiveRows.ToList().Skip(useBuffer ? SPAWN_ROW_BUFFER : 0);
        foreach (Transform row in rows) {
            var cells = Enumerable.Range(0, row.childCount).Select(x => row.GetChild(x));
            cells = player.name == "Player1" ?
                // Can only spawn in left half of cells, preferring leftmost
                cells.Take(Mathf.FloorToInt(row.childCount / 2)) :
                // Can only spawn in right half of cells, preferring rightmost
                cells.Skip(Mathf.CeilToInt(row.childCount / 2)).Reverse();
            foreach (var cell in cells) {
                if (cell.childCount < 2) {
                    player.transform.SetPositionAndRotation(cell.position + Vector3.up, cell.rotation);
                    return;
                }
            }
        }
        Debug.LogError("Couldn't spawn player!");
    }
}
