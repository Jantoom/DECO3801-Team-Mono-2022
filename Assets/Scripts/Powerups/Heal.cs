using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Powerup
{
    public override void StartPowerup() {
        playerInfo.Health += GameInfo.BASE_HEALTH * 10;
        EndPowerup();
    }
}
