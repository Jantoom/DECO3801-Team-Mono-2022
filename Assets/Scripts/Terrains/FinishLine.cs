using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used for the object at the end of the terrain generation. The first person to collide with the
// parent game object is the first person to cross the finish line. This is one way for a team to
// win the game.
public class FinishLine : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            GameOverInfo.isGameOver = true;
            if (other.gameObject.name == "Player1") {
                GameOverInfo.winner = "Winner is \n Team 1";
            } else if (other.gameObject.name == "Player2") {
                GameOverInfo.winner = "Winner is \n Team 2";
            }
        }
    }
}
