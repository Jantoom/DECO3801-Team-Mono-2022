using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeDisplay : MonoBehaviour
{
    public float timeAmount = 120;
    //public Text timeText;
    public TMP_Text timeT;
    void Update()
    {
        if (timeAmount > 0)
        {
            timeAmount -= Time.deltaTime;
        } else
        {
            timeAmount = 0;
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
        //timeText.text = string.Format("{0:00}:{1:00}", min, sec);
        timeT.text = string.Format("{0:00}:{1:00}", min, sec); ;
    }
}
