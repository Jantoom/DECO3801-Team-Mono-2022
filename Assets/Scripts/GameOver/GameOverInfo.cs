using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverInfo : MonoBehaviour
{
    [HideInInspector]
    public static bool isGameOver;
    [HideInInspector]
    public static string winner;//

    private void Awake()
    {
        isGameOver = false;
        winner = "";//

    }

}
