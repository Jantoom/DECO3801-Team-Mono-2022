using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDestruction : MonoBehaviour, IDestructible
{
    private PlayerInfo playerInfo;

    void Awake() {
        playerInfo = gameObject.GetComponent<PlayerInfo>();
    }

    public void Hit(int damage) {
        var damageOverflow = Mathf.Max(damage - playerInfo.Shield, 0);
        playerInfo.Shield -= damage;
        playerInfo.Health -= damageOverflow;
        if (playerInfo.Health == 0) {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.GetComponent<Bomb>() == null) {
            collision.gameObject.GetComponent<IDestructible>()?.Hit(playerInfo.Attack);
        }
    }
}
