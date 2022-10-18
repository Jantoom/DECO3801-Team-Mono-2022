using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class GameOverDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _gameOverText, _winnerNameText, _menuText;
    [SerializeField] private GameObject _gameOverScreen;
    bool displayGameOver;
    [SerializeField] private KeyCode transitionKey;
    [SerializeField] private string scene;

    void Awake()
    {
        if (PlayerControls.UseSerialControls && !PlayerControls.SerialInputOpen) {
            PlayerControls.SerialInput.Open();
            PlayerControls.SerialInput.ReadTimeout = 1;
            PlayerControls.SerialInputOpen = true;
        }
        displayGameOver = false;
    }
    void Update()
    {
        if (GameOverInfo.isGameOver) {
            if (!displayGameOver) {
                DisplayGameOverScreen();
            }
        }

        if (GameOverInfo.isGameOver) {
            if (PlayerControls.UseSerialControls) {
                if (PlayerControls.SerialInput.BytesToRead > 0) {
                   PlayerControls.SerialInput.ReadExisting();
                    SceneManager.LoadScene(scene);
                }
            } else if (Input.GetKeyDown(transitionKey)) {
                 SceneManager.LoadScene(scene);
            }
        }
    }
    private void DisplayGameOverScreen()
    {
        displayGameOver = true;
        _gameOverScreen.SetActive(true);
        _gameOverText.text = "Game Over";
        FindWinner();
        _winnerNameText.text = GameOverInfo.winner;
        _menuText.text = "Sit on any chair to go back to the Main Menu (Press 1)";
        FindObjectOfType<AudioManager>().Stop("GameMusic");
        FindObjectOfType<AudioManager>().Play("WinSound");
    }
    private void FindWinner()
    {
        var gameInfo = GameObject.Find("GameInfo").GetComponent<GameInfo>();
        var playerOneInfo = gameInfo.PlayerOne.GetComponent<PlayerInfo>();
        var playerTwoInfo = gameInfo.PlayerTwo.GetComponent<PlayerInfo>();

        if(GameOverInfo.winner == "") {   
            if (playerOneInfo.Health == 0) {
                GameOverInfo.winner = "Winner is \n Team 2";
            } else if (playerOneInfo.Health == 0) {
                GameOverInfo.winner = "Winner is \n Team 1";
            } else if (playerOneInfo.Health > playerTwoInfo.Health) {
                GameOverInfo.winner = "Winner is \n Team 1";
            } else if (playerOneInfo.Health < playerTwoInfo.Health) {
                GameOverInfo.winner = "Winner is \n Team 2";
            } else {
                GameOverInfo.winner = "Game has no winner";
            }
        }
    }
}
