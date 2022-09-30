using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallWeak : MonoBehaviour, IDestructible
{
    [SerializeField]
    private int health;

    public void Hit(int damage) {
        health -= damage;
        if (health <= 0) Destroy(gameObject);
    }
}
