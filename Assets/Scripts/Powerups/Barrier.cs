using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : Powerup
{
    private float duration = 3f;
    public override float Duration { get => duration; }

    public override void StartPowerup() {
        playerInfo.Invincible = true;
        base.StartPowerup();
    }
    public override void EndPowerup()
    {
        playerInfo.Invincible = false;
        base.EndPowerup();
    }
}
