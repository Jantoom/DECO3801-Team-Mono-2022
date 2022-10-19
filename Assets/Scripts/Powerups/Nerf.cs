using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents a commonly found pattern between most powerups. The nerf is in action once a single
// field belonging to an opponent is altered. Use this if the effect of the nerf is state-like,
// otherwise create a custom powerup inheriting from Powerup.
public class Nerf : Powerup
{
    public string NerfStat;

    void OnCollisionEnter(Collision collision)
    {
        if (PlayerInfo == null && collision.gameObject.TryGetComponent<PlayerInfo>(out PlayerInfo)) {
            // First encounter with a player
            var powerup = AttachPowerupToPlayer(PlayerInfo);
            ((Nerf) powerup).NerfStat = NerfStat;
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
            powerup.PlayerInfo = PlayerInfo.Opponent.GetComponent<PlayerInfo>();
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
