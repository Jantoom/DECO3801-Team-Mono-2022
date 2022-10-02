using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exhaust : Powerup
{
    private float duration = 2f;
    public override float Duration { get => duration; }

    public override void StartPowerup() {
        playerInfo.AttackMultiplier /= 2;
        playerInfo.CooldownMultiplier *= 2;
        base.StartPowerup();
    }
    public override void EndPowerup()
    {
        playerInfo.AttackMultiplier *= 2;
        playerInfo.CooldownMultiplier /= 2;
        base.EndPowerup();
    }
}
