using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDestruction : MonoBehaviour, IDestructible
{
    private PlayerInfo playerInfo;
    //private PlayerLives playerLives;

    void Awake() {
        playerInfo = gameObject.GetComponent<PlayerInfo>();
        //playerLives = gameObject.GetComponent<PlayerLives>();
    }

    public void TakeDamage(int damage) {
        if (damage > 0) {
            // Took actual damage/lost a life
            playerInfo.Health -= GameInfo.BASE_HEALTH * 10; // Health regulation for under/overkill damage
            if (playerInfo.Health == 0) {
                // Has no more lives
                Destroy(gameObject);
            }
        }
    }
}
