using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : Powerup
{
    public override void StartPowerup() {
        var playerPos = transform.position;
        var opponentPos = PlayerInfo.Opponent.transform.position;
        transform.position = opponentPos;
        PlayerInfo.Opponent.transform.position = playerPos;
        EndPowerup();
    }
}
