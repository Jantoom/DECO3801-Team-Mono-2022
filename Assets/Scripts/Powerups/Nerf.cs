using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nerf : Powerup
{
    public string NerfStat;

    void OnCollisionEnter(Collision collision) {
        if (PlayerInfo == null && collision.gameObject.TryGetComponent<PlayerInfo>(out var info) && info.LoadedPowerup == null) {
            PlayerInfo = info;
            // Collision is a first encounter with a player not currently holding an unactivated powerup
            var powerup = PlayerInfo.Player.AddComponent<Nerf>();
            powerup.NerfStat = NerfStat;
            powerup.Duration = Duration;
            PlayerInfo.LoadedPowerup = powerup;
            // No further functionality required from collectable game object
            Destroy(gameObject);
        }
    }

    public override void KillDuplicatePowerups() {
        foreach (var powerup in PlayerInfo.GetComponents<Nerf>()) {
            // Kill any activated powerups of the same type (effectively refreshes the powerup)
            if (powerup.NerfStat == NerfStat && powerup != this) {
                powerup.EndPowerup();
            }
        }
    }

    public override void StartPowerup() {
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
            PlayerInfo.GetType().GetProperty(NerfStat).SetValue(PlayerInfo, true);
            base.StartPowerup();
        }
    }

    public override void EndPowerup()
    {
        if (Activated) {
            // No nerf on owner
        } else {
            // Undo nerf on opponent
            PlayerInfo.GetType().GetProperty(NerfStat).SetValue(PlayerInfo, false);
        }
        base.EndPowerup();
    }
}
