using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDestructive : MonoBehaviour
{
    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.GetComponent<Bomb>() == null) {
            collision.gameObject.GetComponent<IDestructible>()?.ApplyDamage(1);
        }
    }
}
