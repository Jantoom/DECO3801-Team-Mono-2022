using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Powerup
{
    private float duration = 10f;
    public override float Duration { get => duration; }
    private Object laserBombPrefab;

    void Awake() {
        laserBombPrefab = Resources.Load("Prefabs/Bombs/LaserBomb");
    }

    public override void StartPowerup()
    {
        playerInfo.BombPrefab = laserBombPrefab;
        base.StartPowerup();
    }

    public override void EndPowerup()
    {
        playerInfo.BombPrefab = null;
        base.EndPowerup();
    }
}
