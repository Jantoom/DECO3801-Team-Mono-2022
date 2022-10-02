using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallWeak : MonoBehaviour, IDestructible
{
    private int health = 2;
    public int Health { get => health; }

    public void TakeDamage(int damage) {
        health -= damage;
        if (health <= 0) Destroy(gameObject);
    }
}
