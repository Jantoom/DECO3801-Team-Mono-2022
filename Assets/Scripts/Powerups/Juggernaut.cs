using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Juggernaut : Powerup
{
    private float duration = 3f;
    public override float Duration { get => duration; }

    public override void StartPowerup() {
        playerInfo.AttackMultiplier *= 100;
        base.StartPowerup();
    }
    public override void EndPowerup()
    {
        playerInfo.AttackMultiplier /= 100;
        base.EndPowerup();
    }
}
