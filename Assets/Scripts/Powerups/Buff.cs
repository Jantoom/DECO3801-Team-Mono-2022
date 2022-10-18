using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : Powerup
{
    public string BuffStat;

    void OnCollisionEnter(Collision collision)
    {
        if (PlayerInfo == null && collision.gameObject.TryGetComponent<PlayerInfo>(out PlayerInfo)) {
            // First encounter with a player
            var powerup = AttachPowerupToPlayer(PlayerInfo);
            ((Buff) powerup).BuffStat = BuffStat;
            // No further functionality required from collectable game object
            Destroy(gameObject);
        }
    }
    //
    // Summary:
    //     Updates player's buff stat to true.
    protected override void StartPowerup()
    {
        PlayerInfo.GetType().GetField(BuffStat).SetValue(PlayerInfo, true);
        base.StartPowerup();
    }
    //
    // Summary:
    //     Updates player's buff stat to false.
    protected override void EndPowerup()
    {
        PlayerInfo.GetType().GetField(BuffStat).SetValue(PlayerInfo, false);
        base.EndPowerup();
    }
    protected override void KillDuplicatePowerups()
    {
        foreach (var powerup in PlayerInfo.GetComponents<Buff>()) {
            // Kill any activated powerups of the same type (effectively refreshes the powerup)
            if (powerup.BuffStat == BuffStat && powerup != this) {
                powerup.EndPowerup();
            }
        }
    }
}
