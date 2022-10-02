using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.UIElements;

public class TimeDisplay : MonoBehaviour
{
    float timeAmount;//static float timeAmount;
    public TMP_Text timeT;
    // Use a singleton class or combine gameover class with this one or use static variable
    /*public TMP_Text gameOverText;
    public TMP_Text winnerText;
    public TMP_Text winnerNameText;
    public GameObject gameOverScreen;*/
    //

    private void Start()
    {
        timeAmount = 600;
    }
    void Update()
    {
        if (timeAmount > 0)
        {
            timeAmount -= Time.deltaTime;
        } else
        {
            timeAmount = 0;
           // DisplayGameOverScreen();


        }
        ShowTime(timeAmount);
    }
    void ShowTime(float timeToShow)
    {
        if (timeToShow < 0)
        {
            timeToShow = 0;
        }
        float min = Mathf.FloorToInt(timeToShow / 60);
        float sec = Mathf.FloorToInt(timeToShow % 60);
        timeT.text = string.Format("{0:00}:{1:00}", min, sec); ;
    }
    public Boolean IsTimeOver()
    {
        return timeAmount <= 0;
    }
    public float getTime()
    {
        return timeAmount;
    }

    /*void DisplayGameOverScreen() { 
    
         gameOverScreen.SetActive(true);
         gameOverText.text = "Game Over";
         winnerText.text = "Winner is";
         winnerNameText.text = "Player 1";//?
    }*/
}
