using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : Powerup
{
    //
    // Summary:
    //     Swaps the locations of both players.
    protected override void StartPowerup()
    {
        var playerPos = transform.position;
        var opponentPos = PlayerInfo.Opponent.transform.position;
        transform.position = opponentPos;
        PlayerInfo.Opponent.transform.position = playerPos;
        EndPowerup();
    }
}
