using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private string powerupName;

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.TryGetComponent<PlayerInfo>(out var owner)) {
            var powerupType = System.Type.GetType(powerupName);

            var existingPowerup = owner.Player.gameObject.GetComponent(powerupType);
            if (existingPowerup == null) {
                var addedPowerup = owner.Player.AddComponent(powerupType);
                if (typeof(BaseEffect).IsAssignableFrom(addedPowerup.GetType())) {
                    Debug.Log("Starting juggernaut effect");
                    ((BaseEffect) addedPowerup).StartEffect();
                } else if (typeof(BaseItem).IsAssignableFrom(addedPowerup.GetType())) {
                    // ??
                }
            } else {
                if (typeof(BaseEffect).IsAssignableFrom(existingPowerup.GetType())) {
                    ((BaseEffect) existingPowerup).ExtendEffect();
                } else if (typeof(BaseItem).IsAssignableFrom(existingPowerup.GetType())) {
                    // ??
                }
            }

            Destroy(gameObject);
        }
    }
}
