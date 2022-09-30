using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : BaseEffect
{
    [SerializeField]
    private int healAmount;

    public override void StartEffect() {
        playerInfo.Health += healAmount;
        EndEffect();
    }
}
