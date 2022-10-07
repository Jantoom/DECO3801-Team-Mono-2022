using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exhaust : Powerup
{
    private float duration = 2f;
    public override float Duration { get => duration; }

    public override void StartPowerup() {
        playerInfo.Exhaust = true;
        base.StartPowerup();
    }
    public override void EndPowerup()
    {
        playerInfo.Exhaust = false;
        base.EndPowerup();
    }
}
