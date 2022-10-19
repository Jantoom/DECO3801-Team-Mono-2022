using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Holds generic game-related information like references to both players.
public class GameInfo : MonoBehaviour
{
    public static readonly int BASE_HEALTH = 1;
    public GameObject PlayerOne, PlayerTwo;
    public Image LoadedPowerupPlayerOne, LoadedPowerupPlayerTwo;
}
