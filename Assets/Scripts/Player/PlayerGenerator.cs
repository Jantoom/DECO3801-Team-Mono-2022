using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerGenerator : MonoBehaviour
{
    public static readonly int SPAWN_ROW_BUFFER = 2;
    private static TerrainGenerator _terrainGenerator;
    [SerializeField] private GameObject _playerOnePrefab, _playerTwoPrefab;

    void Awake()
    {
        var gameInfo = GameObject.Find("GameInfo");

        _terrainGenerator = gameInfo.GetComponent<TerrainGenerator>();

        var playerOne = Instantiate(_playerOnePrefab);
        var playerTwo = Instantiate(_playerTwoPrefab);

        playerOne.name = "Player1";
        playerTwo.name = "Player2";

        gameInfo.GetComponent<GameInfo>().PlayerOne = playerOne;
        gameInfo.GetComponent<GameInfo>().PlayerTwo = playerTwo;

        playerOne.GetComponent<PlayerInfo>().Opponent = playerTwo;
        playerTwo.GetComponent<PlayerInfo>().Opponent = playerOne;

        playerOne.GetComponent<PlayerLives>().Heart1 = GameObject.Find("Heart3");
        playerOne.GetComponent<PlayerLives>().Heart2 = GameObject.Find("Heart2");
        playerOne.GetComponent<PlayerLives>().Heart3 = GameObject.Find("Heart1");
        playerTwo.GetComponent<PlayerLives>().Heart1 = GameObject.Find("Heart6");
        playerTwo.GetComponent<PlayerLives>().Heart2 = GameObject.Find("Heart5");
        playerTwo.GetComponent<PlayerLives>().Heart3 = GameObject.Find("Heart4");
    }

    void Start() {
        var gameInfo = GameObject.Find("GameInfo").GetComponent<GameInfo>();
        Spawn(gameInfo.PlayerOne, false);
        Spawn(gameInfo.PlayerTwo, false);
    }
    public static void Spawn(GameObject player) => Spawn(player, true);
    // Summary:
    //     Spawns the given player in a suitable position based on which side the player has
    //     preference for.
    //
    // Parameters:
    //   player:
    //     The player to spawn
    //   useBuffer:
    //     Whether the generator should use include a buffer zone between the first row and the 
    //     player
    public static void Spawn(GameObject player, bool useBuffer)
    {
        var rows = _terrainGenerator.ActiveRows.ToList().Skip(useBuffer ? SPAWN_ROW_BUFFER : 0);
        foreach (Transform row in rows) {
            var cells = Enumerable.Range(0, row.childCount).Select(x => row.GetChild(x));
            cells = player.name == "Player1" ?
                // Can only spawn in left half of cells, preferring leftmost
                cells.Take(Mathf.FloorToInt(row.childCount / 2)) :
                // Can only spawn in right half of cells, preferring rightmost
                cells.Skip(Mathf.CeilToInt(row.childCount / 2)).Reverse();
            foreach (var cell in cells) {
                if (cell.childCount < 2) {
                    player.transform.SetPositionAndRotation(cell.position, cell.rotation);
                    return;
                }
            }
        }
        Debug.LogError("Couldn't spawn player!");
    }
}
