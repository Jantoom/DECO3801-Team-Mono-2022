using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRange : Powerup
{
    public override bool IsItem { get => true; }
    private float duration = 10f;
    public override float Duration { get => duration; }
    private Object longRangeBombPrefab;

    void Awake() {
        longRangeBombPrefab = Resources.Load("Prefabs/Bombs/LongRangeBomb");
    }
    
    public override void StartPowerup()
    {
        playerInfo.BombPrefab = longRangeBombPrefab;
        base.StartPowerup();
    }

    public override void EndPowerup()
    {
        playerInfo.BombPrefab = null;
        base.EndPowerup();
    }
}
