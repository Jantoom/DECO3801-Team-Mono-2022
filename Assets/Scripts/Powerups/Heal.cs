using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
