using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : Powerup
{
    public string BuffStat;

    void OnCollisionEnter(Collision collision) {
        if (PlayerInfo == null && collision.gameObject.TryGetComponent<PlayerInfo>(out var info) && info.LoadedPowerup == null) {
            PlayerInfo = info;
            // Collision is a first encounter with a player not currently holding an unactivated powerup
            var powerup = PlayerInfo.Player.AddComponent<Buff>();
            powerup.BuffStat = BuffStat;
            powerup.Duration = Duration;
            PlayerInfo.LoadedPowerup = powerup;
            // No further functionality required from collectable game object
            Destroy(gameObject);
        }
    }

    public override void KillDuplicatePowerups() {
        foreach (var powerup in PlayerInfo.GetComponents<Buff>()) {
            // Kill any activated powerups of the same type (effectively refreshes the powerup)
            if (powerup.BuffStat == BuffStat && powerup != this) {
                powerup.EndPowerup();
            }
        }
    }

    public override void StartPowerup() {
        PlayerInfo.GetType().GetProperty(BuffStat).SetValue(PlayerInfo, true);
        base.StartPowerup();
    }

    public override void EndPowerup()
    {
        PlayerInfo.GetType().GetProperty(BuffStat).SetValue(PlayerInfo, false);
        base.EndPowerup();
    }
}
