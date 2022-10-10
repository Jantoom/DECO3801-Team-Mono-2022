using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerLives : MonoBehaviour
{
    private PlayerInfo playerInfo;
    //private bool isWinnerFound;

    [SerializeField]
    private Image heart1, heart2, heart3;
    public Image Heart1 { get => heart1; }
    public Image Heart2 { get => heart2; }
    public Image Heart3 { get => heart3; }

    private void Awake()
    {
        playerInfo = GetComponent<PlayerInfo>();
    }

    // Update is called once per frame
    void Update()
    {
            
        if (!GameOverInfo.isGameOver)
        {
            switch (playerInfo.Health)
            {

                case 20:
                    Heart1.gameObject.SetActive(false);
                    Heart2.gameObject.SetActive(true);
                    Heart3.gameObject.SetActive(true);
                    break;
                case 10:
                    Heart1.gameObject.SetActive(false);
                    Heart2.gameObject.SetActive(false);
                    Heart3.gameObject.SetActive(true);
                    break;
                case 0: //Game Over
                    Heart1.gameObject.SetActive(false);
                    Heart2.gameObject.SetActive(false);
                    Heart3.gameObject.SetActive(false);

                    FindWinner();
                    GameOverInfo.isGameOver = true;
                    Destroy(gameObject);
                    break;
            }
        }
        /*if (!GameOverInfo.isGameOver)
    {
        Debug.Log(gameObject.name + " damage is " + damage);
        Debug.Log(gameObject.name + " health is " + playerInfo.Health);
    }*/


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
