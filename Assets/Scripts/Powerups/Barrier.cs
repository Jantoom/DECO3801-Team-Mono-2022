using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : BaseEffect
{
    private float duration = 3f;
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
