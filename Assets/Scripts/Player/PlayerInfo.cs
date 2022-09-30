using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField]
    private int playerNumber, health, baseAttack;
    private int shield;
    private float attackMultiplier = 1f, cooldownMultiplier = 1f;
    private GameObject player;
    public GameObject Player { get => player; }
    public int Health { get => health; set => health = Mathf.Clamp(value, 0, 3); }
    public int Shield { get => shield; set => shield = Mathf.Clamp(value, 0, 100); }
    public int Attack { get => Mathf.RoundToInt(baseAttack * attackMultiplier); }
    public float AttackMultiplier { get => attackMultiplier; set => attackMultiplier = Mathf.Max(value, 0); }
    public float CooldownMultiplier { get => cooldownMultiplier; set => cooldownMultiplier = Mathf.Max(value, 0); }

    private void Awake()
    {
        player = GameObject.Find("Player" + playerNumber.ToString());
        player.GetComponent<PlayerPlant>().SetPlayerNumber(playerNumber);
        player.GetComponent<PlayerMove>().SetPlayerNumber(playerNumber);
    }
}
