using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    // Scene GameObjects
    public GameObject Player { get => gameObject; }
    public GameObject Opponent;
    // Player Stats
    private int health = GameInfo.BASE_HEALTH * 30, attack = GameInfo.BASE_HEALTH * 5;
    public int Health { get => health; set => health = Invincible && value < health ? health : Mathf.Clamp(value, 0, GameInfo.BASE_HEALTH * 30); }
    public int Attack { get => Mathf.RoundToInt(attack * (Juggernaut ? 2.0f : (Exhausted ? 0.2f : 1.0f))); }
    private bool invincible = false, juggernaut = false, ghosted = false, exhausted = false, frozen = false;
    public bool Invincible { get => invincible; set => invincible = value; }
    public bool Juggernaut { get => juggernaut; set => juggernaut = value; }
    public bool Ghosted { get => ghosted; 
        set { 
            if (ghosted == true && value == false && GetComponent<Rigidbody>().SweepTest(Vector3.forward, out var hit, 0.5f)) {
                PlayerGenerator.Spawn(Player);
                transform.rotation = Quaternion.identity;
            }
            ghosted = value; 
        }
    }
    public bool Exhausted { get => exhausted; set => exhausted = value; }
    public bool Frozen { get => frozen; set => frozen = value; }
    // Player Bomb
    [SerializeField]
    private Object defaultBombPrefab = null, overrideBombPrefab = null;
    public Object BombPrefab { get => overrideBombPrefab != null ? overrideBombPrefab : defaultBombPrefab; set => overrideBombPrefab = value; }
    // Player Item (only useful if we decide to separate controls for items and bombs)
    public Powerup LoadedPowerup = null;
    private Object itemPrefab = null;
    public Object ItemPrefab { get => itemPrefab; set => itemPrefab = value; }

    void Awake() {
        Opponent = name == "Player1" ? GameObject.Find("Player2") : GameObject.Find("Player1");
    }
}
