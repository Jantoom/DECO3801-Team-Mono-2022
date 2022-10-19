using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Responsible for spawning the players in the map. This includes initialisation and when a 
// player falls off the map.
public class PlayerGenerator : MonoBehaviour
{
    public static readonly int SPAWN_ROW_BUFFER = 2;
    [SerializeField] private GameObject _playerOnePrefab, _playerTwoPrefab;

    void Awake()
    {
        var gameInfo = GameObject.Find("GameInfo").GetComponent<GameInfo>();
        // Spawn players in the world
        var playerOne = Instantiate(_playerOnePrefab);
        var playerTwo = Instantiate(_playerTwoPrefab);
        // Give aliases to the players
        playerOne.name = "Player1";
        playerTwo.name = "Player2";
        // Reference the players in the game info
        gameInfo.PlayerOne = playerOne;
        gameInfo.PlayerTwo = playerTwo;

        var playerOneInfo = playerOne.GetComponent<PlayerInfo>();
        var playerTwoInfo = playerTwo.GetComponent<PlayerInfo>();
        // Give players acknowledgement of each other
        playerOneInfo.Opponent = playerTwo;
        playerTwoInfo.Opponent = playerOne;
        // Give reference to UI element
        playerOneInfo.LoadedPowerupImage = gameInfo.LoadedPowerupPlayerOne;
        playerTwoInfo.LoadedPowerupImage = gameInfo.LoadedPowerupPlayerTwo;
        // Initialise images to be hidden due to no starting powerups
        playerOneInfo.LoadedPowerupImage.enabled = false;
        playerTwoInfo.LoadedPowerupImage.enabled = false;

        var playerOneLives = playerOne.GetComponent<PlayerLives>();
        var playerTwoLives = playerTwo.GetComponent<PlayerLives>();
        // Give reference to UI elements
        playerOneLives.Heart1 = GameObject.Find("Heart3");
        playerOneLives.Heart2 = GameObject.Find("Heart2");
        playerOneLives.Heart3 = GameObject.Find("Heart1");
        playerTwoLives.Heart1 = GameObject.Find("Heart6");
        playerTwoLives.Heart2 = GameObject.Find("Heart5");
        playerTwoLives.Heart3 = GameObject.Find("Heart4");
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
        var terrainGenerator = GameObject.Find("GameInfo").GetComponent<TerrainGenerator>();
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
                    player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    player.transform.SetPositionAndRotation(cell.position + Vector3.up * 0.5f, cell.rotation);
                    return;
                }
            }
        }
        Debug.LogError("Couldn't spawn player!");
    }
}
