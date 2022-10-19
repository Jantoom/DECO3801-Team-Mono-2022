using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Makes wall prefabs unbreakable by player collision and most weapons, but can be broken from 
// bombs due to their massive explosion damage.
public class WallUnbreakable : MonoBehaviour, IDestructible
{
    [field: SerializeField] public int Health { get; private set; } = GameInfo.BASE_HEALTH * 20;

    public void TakeDamage(int damage)
    {
        if (Health <= damage) Destroy(gameObject);
    }
}
