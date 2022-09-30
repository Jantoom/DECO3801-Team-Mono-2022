using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exhaust : BaseEffect
{
    [SerializeField]
    private float duration;
    public override float Duration { get => duration; }

    public override void StartEffect() {
        playerInfo.AttackMultiplier /= 2;
        playerInfo.CooldownMultiplier *= 2;
        base.StartEffect();
    }
    public override void EndEffect()
    {
        playerInfo.AttackMultiplier *= 2;
        playerInfo.CooldownMultiplier /= 2;
        base.EndEffect();
    }
}
