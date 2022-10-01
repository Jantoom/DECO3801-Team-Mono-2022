using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Juggernaut : BaseEffect
{
    private float duration = 3f;
    public override float Duration { get => duration; }

    public override void StartEffect() {
        playerInfo.AttackMultiplier *= 100;
        base.StartEffect();
    }
    public override void EndEffect()
    {
        playerInfo.AttackMultiplier /= 100;
        Debug.Log("Ending juggernaut");
        base.EndEffect();
    }
}
