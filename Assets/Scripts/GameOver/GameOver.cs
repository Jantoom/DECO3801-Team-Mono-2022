using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class GameOver : MonoBehaviour
{
    public TMP_Text gameOverText;
    public TMP_Text winnerText;
    public TMP_Text winnerNameText;
    public GameObject gameOverScreen;
    public GameObject gameOverButton;
    TimeDisplay timeValue;

    private void Awake()
    {
        timeValue = GameObject.Find("TimeDisplayManager").GetComponent<TimeDisplay>();
    }
    void Update()
    {
        //Debug.Log(TimeDisplay.timeAmount);
        DisplayGameOverScreen();
    }
    void DisplayGameOverScreen()
    {
        if(timeValue.IsTimeOver())
        {
            Debug.Log(timeValue.IsTimeOver());


            gameOverScreen.SetActive(true);
            gameOverText.text = "Game Over";
            winnerText.text = "Winner is";
            winnerNameText.text = "Player 1";//?
            gameOverButton.SetActive(true);
        }
    }

}
