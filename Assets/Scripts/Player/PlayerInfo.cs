using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField]
    private int playerNumber;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.Find("Player" + playerNumber.ToString());
        player.GetComponent<PlayerPlant>().SetPlayerNumber(playerNumber);
        player.GetComponent<PlayerMove>().SetPlayerNumber(playerNumber);
    }
}
