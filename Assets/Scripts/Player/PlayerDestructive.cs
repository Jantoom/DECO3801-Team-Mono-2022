using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMortality : MonoBehaviour
{
    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.GetComponent<Bomb>() == null) {
            collision.gameObject.GetComponent<IDestructible>()?.Hit(1);
        }
    }
}
