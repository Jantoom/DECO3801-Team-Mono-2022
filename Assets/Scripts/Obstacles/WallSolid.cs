using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSolid : MonoBehaviour, IDestructible
{
    [field: SerializeField] public int Health { get; private set; } = GameInfo.BASE_HEALTH * 20;

    public void TakeDamage(int damage)
    {
        if (Health <= damage) Destroy(gameObject);
    }
}
