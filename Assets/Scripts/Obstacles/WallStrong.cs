using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallStrong : MonoBehaviour, IDestructible
{
    private int health = GameInfo.BASE_HEALTH * 20;
    public int Health { get => health; }

    public void TakeDamage(int damage) {
        if (health <= damage) Destroy(gameObject);
    }
}
