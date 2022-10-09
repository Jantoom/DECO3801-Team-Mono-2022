using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerLives : MonoBehaviour
{
    private PlayerInfo playerInfo;
    //private string winner;
    //private bool isWinnerFound;

    [SerializeField]
    private Image heart1, heart2, heart3;
    public Image Heart1 { get => heart1; }
    public Image Heart2 { get => heart2; }
    public Image Heart3 { get => heart3; }
    //public string Winner { get => winner; }
    //public void SetWinner (string name) { winner = name; }

    private void Awake()
    {
        playerInfo = GetComponent<PlayerInfo>();
        //isWinnerFound = false;
        //winner = "";
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Health is " + playerInfo.Health);
        //Debug.Log("GameOver is " + GameOverInfo.isGameOver);
        //Debug.Log(GameObject.Find("Player1"));
        //Debug.Log(gameObject.ToString());
        if (!GameOverInfo.isGameOver)
        {
            switch (playerInfo.Health)
            {

                case 20:
                    Heart1.gameObject.SetActive(false);

                    break;
                case 10:
                    Heart2.gameObject.SetActive(false);
                    break;
                case 0: //Game Over
                    Heart3.gameObject.SetActive(false);
                    //if (!isWinnerFound) { SetWinner(gameObject.ToString()); isWinnerFound = true; Debug.Log(Winner); }
                    FindWinner();
                    GameOverInfo.isGameOver = true;
                    //Destroy(gameObject);
                    break;
            }
        }


   }

    public void FindWinner()
    {
        if (gameObject.name == "Player1")
        {
            GameOverInfo.winner = "Team 2";
        } else if (gameObject.name == "Player2")
        {
            GameOverInfo.winner = "Team 1";
        }
    }
}
