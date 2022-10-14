using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.UIElements;

public class TimeDisplay : MonoBehaviour
{
    
    private float timeAmount;
    [SerializeField] private TMP_Text timeLimit;



    private void Start()
    {
        timeAmount = 10;
    }
    void Update()
    {
        if (!GameOverInfo.isGameOver)
        {
            if (timeAmount > 0)
            {
                timeAmount -= Time.deltaTime;
            }
            else
            {
                timeAmount = 0;
                GameOverInfo.isGameOver = true;

            }
            ShowTime(timeAmount);
        }

        
    }
    void ShowTime(float timeToShow)
    {
        if (timeToShow < 0)
        {
            timeToShow = 0;
        }
        float min = Mathf.FloorToInt(timeToShow / 60);
        float sec = Mathf.FloorToInt(timeToShow % 60);
        timeLimit.text = string.Format("{0:00}:{1:00}", min, sec); ;
    }

}
