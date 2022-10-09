using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Juggernaut : Powerup
{
    private float duration = 3f;
    public override float Duration { get => duration; }

    public override void StartPowerup() {
        playerInfo.Juggernaut = true;
        base.StartPowerup();
    }
    public override void EndPowerup()
    {
        playerInfo.Juggernaut = false;
        base.EndPowerup();
    }
}
