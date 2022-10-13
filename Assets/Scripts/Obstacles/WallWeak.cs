using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallWeak : MonoBehaviour, IDestructible
{
    [field: SerializeField] public int Health { get; private set; } = GameInfo.BASE_HEALTH * 10;

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0) Destroy(gameObject);
    }
}
