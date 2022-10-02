using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Powerup
{
    private int healAmount = 100;

    public override void StartPowerup() {
        playerInfo.Health += healAmount;
        EndPowerup();
    }
}
