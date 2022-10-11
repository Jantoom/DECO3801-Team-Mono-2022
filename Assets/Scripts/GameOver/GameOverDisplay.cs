using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOverDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text gameOverText, winnerText, winnerNameText, menuText;

    [SerializeField]
    GameObject gameOverScreen;
    //TimeDisplay timeValue;
    bool displayGameOver;
    [SerializeField] private KeyCode transitionKey;
    [SerializeField] private string scene;
   // PlayerInfo p1;
    //PlayerInfo p2;

    private void Awake()
    {

        //timeValue = GameObject.Find("TimeDisplayManager").GetComponent<TimeDisplay>();

        displayGameOver = false;
 


    }
    private void Start()
    {
        
       // p2 = gameObject.GetComponent<PlayerInfo>();
       // p1 = gameObject.GetComponent<PlayerInfo>();
    }
    void Update()
    {
        if (GameOverInfo.isGameOver)
        {
            if (!displayGameOver)
            {
                DisplayGameOverScreen();
            }
            if (Input.GetKeyDown(transitionKey))
            {
                SceneManager.LoadScene(scene);
            }
        }


    }
    public void DisplayGameOverScreen()
    {
       // winners();
        //Debug.Log(timeValue.IsTimeOver());
        displayGameOver = true;
        gameOverScreen.SetActive(true);
        gameOverText.text = "Game Over";
        winnerText.text = "Winner is";
        winnerNameText.text = GameOverInfo.winner;
        menuText.text = "Sit on any chair to go back to the Main Menu (Press 1)";
        FindObjectOfType<AudioManager>().stop("GameMusic");
        FindObjectOfType<AudioManager>().play("WinSound");
    }

   /* public void winners()
    {
        if (p1.Health > p2.Health)
        {
            GameOverInfo.winner = "Team 1";
        } else if (p1.Health < p2.Health)
        {
            GameOverInfo.winner = "Team 2";
        } else
        {
            GameOverInfo.winner = "Game has no winner";
        }
    }
   */

}
