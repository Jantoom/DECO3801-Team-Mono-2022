using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents a commonly found pattern between most powerups. The weapon is in action once the
// weapon prefab belonging to a player is altered. Use this if the effect of the weapon is 
// state-like, otherwise create a custom powerup inheriting from Powerup.
public class Weapon : Powerup
{
    public GameObject WeaponPrefab;

    void OnCollisionEnter(Collision collision)
    {
        if (PlayerInfo == null && collision.gameObject.TryGetComponent<PlayerInfo>(out PlayerInfo)) {
            // First encounter with a player
            var powerup = AttachPowerupToPlayer(PlayerInfo);
            ((Weapon) powerup).WeaponPrefab = WeaponPrefab;
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
