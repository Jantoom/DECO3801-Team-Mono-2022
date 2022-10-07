using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    // Player GameObject
    [SerializeField]
    private int playerNumber;
    private GameObject player = null;
    public GameObject Player { get => player; }
    // Player Stats
    private int health = 3, shield = 0, baseAttack = 1;
    private float attackMultiplier = 1f, cooldownMultiplier = 1f;
    public int Health { get => health; set => health = Mathf.Clamp(value, 0, 3); }
    public int Shield { get => shield; set => shield = Mathf.Clamp(value, 0, 100); }
    public int Attack { get => Mathf.RoundToInt(baseAttack * attackMultiplier); }
    public float AttackMultiplier { get => attackMultiplier; set => attackMultiplier = Mathf.Max(value, 0); }
    public float CooldownMultiplier { get => cooldownMultiplier; set => cooldownMultiplier = Mathf.Max(value, 0); }
    // Player Bomb
    private Object defaultBombPrefab = null, overrideBombPrefab = null;
    public Object BombPrefab { get => overrideBombPrefab != null ? overrideBombPrefab : defaultBombPrefab; set => overrideBombPrefab = value; }
    // Player Item (only useful if we decide to separate controls for items and bombs)
    private Object itemPrefab = null;
    public Object ItemPrefab { get => itemPrefab; set => itemPrefab = value; }

    private void Awake()
    {
        defaultBombPrefab = Resources.Load("Prefabs/Bombs/DefaultBomb");
        player = GameObject.Find("Player" + playerNumber.ToString());
    }
}
