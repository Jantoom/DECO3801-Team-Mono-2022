using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDestructive : MonoBehaviour, IDestructible
{
    [SerializeField]
    private int health;

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.GetComponent<Bomb>() == null) {
            collision.gameObject.GetComponent<IDestructible>()?.ApplyDamage(1);
        }
    }

    public void ApplyDamage(int damage) {
        health -= damage;
        if (health <= 0) Destroy(gameObject);
    }
}
