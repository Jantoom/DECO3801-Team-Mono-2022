using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Custom powerup that doesn't fit any of the general types of powerups. Heals the player for one
// life.
public class Heal : Powerup
{
    //
    // Summary:
    //     Heals player for one life.
    protected override void StartPowerup()
    {
        PlayerInfo.Health += GameInfo.BASE_HEALTH;
        EndPowerup();
    }
}
