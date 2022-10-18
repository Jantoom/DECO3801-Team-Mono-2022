using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInfo : MonoBehaviour
{
    public static readonly int BASE_HEALTH = 1;
    [field: SerializeField] public RuntimeAnimatorController SpawnAnimator { get; private set; }
    public GameObject PlayerOne, PlayerTwo;
    public Image LoadedPowerupPlayerOne, LoadedPowerupPlayerTwo;

    void Start() {
        PlayerOne = GameObject.Find("Player1");
        PlayerTwo = GameObject.Find("Player2");
    }
}
