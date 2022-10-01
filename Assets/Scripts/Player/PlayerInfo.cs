using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField]
    private int playerNumber, health, baseAttack;
    [SerializeField]
    private KeyCode forwardKey, backKey, leftKey, rightKey, bombKey, itemKey;
    private int shield;
    private float attackMultiplier = 1f, cooldownMultiplier = 1f;
    private GameObject player;
    // Player GameObject
    public GameObject Player { get => player; }
    // Player Stats
    public int Health { get => health; set => health = Mathf.Clamp(value, 0, 3); }
    public int Shield { get => shield; set => shield = Mathf.Clamp(value, 0, 100); }
    public int Attack { get => Mathf.RoundToInt(baseAttack * attackMultiplier); }
    public float AttackMultiplier { get => attackMultiplier; set => attackMultiplier = Mathf.Max(value, 0); }
    public float CooldownMultiplier { get => cooldownMultiplier; set => cooldownMultiplier = Mathf.Max(value, 0); }
    // Player Controls
    public KeyCode ForwardKey { get => forwardKey; }
    public KeyCode BackKey { get => backKey; }
    public KeyCode LeftKey { get => leftKey; }
    public KeyCode RightKey { get => rightKey; }
    public KeyCode BombKey { get => bombKey; }
    public KeyCode ItemKey { get => itemKey; }

    private void Awake()
    {
        player = GameObject.Find("Player" + playerNumber.ToString());
    }
}
