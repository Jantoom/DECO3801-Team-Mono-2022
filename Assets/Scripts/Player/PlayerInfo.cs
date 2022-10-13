using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public GameObject Player { get => gameObject; }
    public GameObject Opponent;
    // Player Stats
    private int _health = GameInfo.BASE_HEALTH * 3, _attack = GameInfo.BASE_HEALTH * 5;
    public int Health { get => _health; set => _health = Invincible && value < _health ? _health : Mathf.Clamp(value, 0, GameInfo.BASE_HEALTH * 3); }
    public int Attack { get => Mathf.RoundToInt(_attack * (Juggernaut ? 2.0f : (Exhausted ? 0.2f : 1.0f))); }
    // Player Effects
    public bool Invincible = false, Juggernaut = false, Ghosted = false, Exhausted = false, Frozen = false;
    // Player Items
    public Powerup LoadedPowerup = null;
    public GameObject WeaponPrefab = null;
}
