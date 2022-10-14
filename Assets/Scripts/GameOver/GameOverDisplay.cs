using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class GameOverDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text gameOverText, winnerNameText, menuText;

    

    [SerializeField]
    GameObject gameOverScreen;
    bool displayGameOver;
    [SerializeField] private KeyCode transitionKey;
    [SerializeField] private string scene;
    [SerializeField] private PlayerInfo player1Prefab, player2Prefab;

    private bool gotPlayerComponent;


    private void Awake()
    {
        //
        if (PlayerControls.UseSerialControls && !PlayerControls.SerialInputOpen)
        {
            PlayerControls.SerialInput.Open();
            PlayerControls.SerialInput.ReadTimeout = 1;
            PlayerControls.SerialInputOpen = true;
        }
        //

        displayGameOver = false;
        gotPlayerComponent = false;
    }
    void Update()
    {
        if (gotPlayerComponent == false)
        {
            player1Prefab = GameObject.Find("Player1").GetComponent<PlayerInfo>();
            player2Prefab = GameObject.Find("Player2").GetComponent<PlayerInfo>();
            gotPlayerComponent = true;
        }

        if (GameOverInfo.isGameOver)
        {
            if (!displayGameOver)
            {
                DisplayGameOverScreen();
            }
        }

        if (GameOverInfo.isGameOver)
        {
            if (PlayerControls.UseSerialControls)
            {
                if (PlayerControls.SerialInput.BytesToRead > 0)
                {
                   // PlayerControls.SerialInput.ReadByte();
                   PlayerControls.SerialInput.ReadExisting();
                    SceneManager.LoadScene(scene);

                }
            }
            else if (Input.GetKeyDown(transitionKey))
            {
                 SceneManager.LoadScene(scene);

            }

        }


    }
    public void DisplayGameOverScreen()
    {
        displayGameOver = true;
        gameOverScreen.SetActive(true);
        gameOverText.text = "Game Over";
        Findwinner();
        winnerNameText.text = GameOverInfo.winner;
        menuText.text = "Sit on any chair to go back to the Main Menu (Press 1)";
        FindObjectOfType<AudioManager>().stop("GameMusic");
        FindObjectOfType<AudioManager>().play("WinSound");
    }

    public void Findwinner()
    {
        /* If the winner is not found yet (If one of the players is reached the finish line, the winner
          is already found and set. */
        if(GameOverInfo.winner == "")
        {   
            if (player1Prefab.Health == 0)
            {
                GameOverInfo.winner = "Winner is \n Team 2";
            }
            else if (player1Prefab.Health == 0)
            {
                GameOverInfo.winner = "Winner is \n Team 1";

            }
            else if (player1Prefab.Health > player2Prefab.Health)
            {
                GameOverInfo.winner = "Winner is \n Team 1";
            }
            else if (player1Prefab.Health < player2Prefab.Health)
            {
                GameOverInfo.winner = "Winner is \n Team 2";
            }
            else
            {
                GameOverInfo.winner = "Game has no winner";
            }
        }

    }
   

}
