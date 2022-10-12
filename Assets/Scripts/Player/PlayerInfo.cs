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
    public bool Ghosted { get => ghosted; set => ghosted = value; }
    public bool Exhausted { get => exhausted; set => exhausted = value; }
    public bool Frozen { get => frozen; set => frozen = value; }
    // Player Item
    public Powerup LoadedPowerup = null;
    [field: SerializeField] public GameObject WeaponPrefab { get; set; } = null;
}
