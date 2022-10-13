using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Powerup
{
    public GameObject WeaponPrefab;

    void OnCollisionEnter(Collision collision)
    {
        if (PlayerInfo == null && collision.gameObject.TryGetComponent<PlayerInfo>(out PlayerInfo)) {
            // First encounter with a player
            Destroy(PlayerInfo.LoadedPowerup);
            var powerup = PlayerInfo.Player.AddComponent<Weapon>();
            powerup.WeaponPrefab = WeaponPrefab;
            powerup.Duration = Duration;
            PlayerInfo.LoadedPowerup = powerup;
            // No further functionality required from collectable game object
            Destroy(gameObject);
        }
    }
    //
    // Summary:
    //     Updates player's weapon prefab. Instantiates one instance of that weapon.
    protected override void StartPowerup()
    {
        PlayerInfo.WeaponPrefab = WeaponPrefab;
        base.StartPowerup();
    }
    //
    // Summary:
    //     Updates player's weapon prefab to null.
    protected override void EndPowerup()
    {
        PlayerInfo.WeaponPrefab = null;
        base.EndPowerup();
    }
    protected override void KillDuplicatePowerups()
    {
        foreach (var powerup in PlayerInfo.GetComponents<Weapon>()) {
            // Kill any activated powerups of the same type (effectively refreshes the powerup)
            if (powerup.WeaponPrefab == WeaponPrefab && powerup != this) {
                powerup.EndPowerup();
            }
        }
    }
}
