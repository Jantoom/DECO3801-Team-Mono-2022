using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    // Player GameObject
    public GameObject Player { get => gameObject; }
    // Player Stats
    private int health = GameInfo.BASE_HEALTH * 30, attack = GameInfo.BASE_HEALTH * 5;
    public int Health { get => health; set => health = Invincible && value < 0 ? health : Mathf.Clamp(value, 0, GameInfo.BASE_HEALTH * 30); }
    public int Attack { get => Mathf.RoundToInt(attack * (Juggernaut ? 2.0f : (Exhaust ? 0.2f : 1.0f))); }
    public bool Juggernaut = false, Exhaust = false, Invincible = false;
    // Player Bomb
    private Object defaultBombPrefab = null, overrideBombPrefab = null;
    public Object BombPrefab { get => overrideBombPrefab != null ? overrideBombPrefab : defaultBombPrefab; set => overrideBombPrefab = value; }
    // Player Item (only useful if we decide to separate controls for items and bombs)
    public Powerup LoadedPowerup = null;
    private Object itemPrefab = null;
    public Object ItemPrefab { get => itemPrefab; set => itemPrefab = value; }

    private void Awake()
    {
        defaultBombPrefab = Resources.Load("Prefabs/Bombs/DefaultBomb");
    }
}
