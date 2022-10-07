using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class GameOverDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text gameOverText, winnerText, winnerNameText, menuText;
    [SerializeField]
    GameObject gameOverScreen;
    TimeDisplay timeValue;
    bool displayGameOver;
    [SerializeField] private KeyCode transitionKey;
    [SerializeField] private string scene;

    private void Awake()
    {

        timeValue = GameObject.Find("TimeDisplayManager").GetComponent<TimeDisplay>();
        displayGameOver = false;


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
        Debug.Log(timeValue.IsTimeOver());
        displayGameOver = true;
        gameOverScreen.SetActive(true);
        gameOverText.text = "Game Over";
        winnerText.text = "Winner is";
        winnerNameText.text = "Player 1";//?
        menuText.text = "Sit on any chair to go back to the Main Menu (Press 1)";

        //gameOverButton.SetActive(true);
        FindObjectOfType<AudioManager>().stop("GameMusic");
        FindObjectOfType<AudioManager>().play("WinSound");
    }


}
