using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : BaseEffect
{
    [SerializeField]
    private float duration;
    public override float Duration { get => duration; }

    public override void StartEffect() {
        playerInfo.Shield += 9999;
        base.StartEffect();
    }
    public override void EndEffect()
    {
        playerInfo.Shield -= 9999;
        base.EndEffect();
    }
}
