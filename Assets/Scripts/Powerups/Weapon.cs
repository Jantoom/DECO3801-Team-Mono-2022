using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Powerup
{
    public GameObject WeaponPrefab;

    void OnCollisionEnter(Collision collision) {
        if (PlayerInfo == null && collision.gameObject.TryGetComponent<PlayerInfo>(out PlayerInfo) && PlayerInfo.LoadedPowerup == null) {
            // Collision is a first encounter with a player not currently holding an unactivated powerup
            var powerup = PlayerInfo.Player.AddComponent<Weapon>();
            powerup.WeaponPrefab = WeaponPrefab;
            powerup.Duration = Duration;
            PlayerInfo.LoadedPowerup = powerup;
            // No further functionality required from collectable game object
            Destroy(gameObject);
        }
    }

    public override void KillDuplicatePowerups() {
        foreach (var powerup in PlayerInfo.GetComponents<Weapon>()) {
            // Kill any activated powerups of the same type (effectively refreshes the powerup)
            if (powerup.WeaponPrefab == WeaponPrefab && powerup != this) {
                powerup.EndPowerup();
            }
        }
    }

    public override void Activate()
    {
        base.Activate();
        Instantiate(WeaponPrefab, transform.position, transform.rotation);
    }

    public override void StartPowerup()
    {
        PlayerInfo.BombPrefab = WeaponPrefab;
        base.StartPowerup();
    }

    public override void EndPowerup()
    {
        PlayerInfo.BombPrefab = null;
        base.EndPowerup();
    }
}
