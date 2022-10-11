using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            GameOverInfo.isGameOver = true;
            if (other.gameObject.name == "Player1") {
                GameOverInfo.winner = "Team 2";
            } else if (other.gameObject.name == "Player2") {
                GameOverInfo.winner = "Team 1";
            }
        }
    }
}
