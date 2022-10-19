using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Custom powerup that doesn't fit any of the general types of powerups. Teleports the player to the
// other's location, and vice versa.
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
