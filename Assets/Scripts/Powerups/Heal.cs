using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : BaseEffect
{
    private int healAmount = 100;

    public override void StartEffect() {
        playerInfo.Health += healAmount;
        EndEffect();
    }
}
