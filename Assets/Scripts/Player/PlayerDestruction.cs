using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDestruction : MonoBehaviour, IDestructible
{
    private PlayerInfo playerInfo;

    void Awake() {
        playerInfo = gameObject.GetComponent<PlayerInfo>();
    }

    public void TakeDamage(int damage) {
        var health = playerInfo.Health;
        playerInfo.Health -= damage;
        if (health > playerInfo.Health) {
            // Took actual damage/lost a life
            if (playerInfo.Health == 0) {
                // Has no more lives
                Destroy(gameObject);
            }
        }
    }
}
