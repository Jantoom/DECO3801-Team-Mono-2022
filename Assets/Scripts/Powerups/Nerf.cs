using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nerf : Powerup
{
    public string NerfStat;

    void OnCollisionEnter(Collision collision)
    {
        if (PlayerInfo == null && collision.gameObject.TryGetComponent<PlayerInfo>(out PlayerInfo)) {
            // First encounter with a player
            Destroy(PlayerInfo.LoadedPowerup);
            var powerup = PlayerInfo.Player.AddComponent<Nerf>();
            powerup.NerfStat = NerfStat;
            powerup.Duration = Duration;
            PlayerInfo.LoadedPowerup = powerup;
            // No further functionality required from collectable game object
            Destroy(gameObject);
        }
    }
    //
    // Summary:
    //     If this powerup is activated, then it will spawn itself on the opponent to start the 
    //     effect. If not activated, then the player's nerf stat is set to true.
    protected override void StartPowerup()
    {
        if (Activated) {
            // Owner of nerf powerup
            var powerup = PlayerInfo.Opponent.AddComponent<Nerf>();
            powerup.NerfStat = NerfStat;
            powerup.Duration = Duration;
            powerup.KillDuplicatePowerups();
            powerup.StartPowerup();
            EndPowerup();
        } else {
            // Opponent (receiver of nerf)
            PlayerInfo.GetType().GetField(NerfStat).SetValue(PlayerInfo, true);
            base.StartPowerup();
        }
    }
    //
    // Summary:
    //     If this powerup is activated, then it will do nothing. If not activated, then the 
    //     player's nerf stat is set to false.
    protected override void EndPowerup()
    {
        if (Activated) {
            // No nerf on owner
        } else {
            // Undo nerf on opponent
            PlayerInfo.GetType().GetField(NerfStat).SetValue(PlayerInfo, false);
        }
        base.EndPowerup();
    }
    protected override void KillDuplicatePowerups() 
    {
        foreach (var powerup in PlayerInfo.GetComponents<Nerf>()) {
            // Kill any activated powerups of the same type (effectively refreshes the powerup)
            if (powerup.NerfStat == NerfStat && powerup != this) {
                powerup.EndPowerup();
            }
        }
    }
}
