using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.UIElements;

public class TimeDisplay : MonoBehaviour
{
    public float timeAmount;
    public TMP_Text timeT;



    private void Start()
    {
        timeAmount = 60;
    }
    void Update()
    {
        if (timeAmount > 0)
        {
            timeAmount -= Time.deltaTime;
        } else
        {
            timeAmount = 0;
            GameOverInfo.isGameOver = true;


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
        if( timeAmount <= 0)
        {
            GameOverInfo.isGameOver = true;//

            return true;
        }
        return false;
    }
    public float getTime()
    {
        return timeAmount;
    }

}
