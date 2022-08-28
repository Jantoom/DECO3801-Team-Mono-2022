using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDestructible : MonoBehaviour, IDestructible
{
    [SerializeField]
    private int health;

    public void ApplyDamage(int damage) {
        health -= damage;
        if (health <= 0) Destroy(gameObject);
    }
}
