using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerLives : MonoBehaviour
{
    private PlayerInfo playerInfo;

    public GameObject heart1, heart2, heart3;

    private void Awake()
    {
        playerInfo = GetComponent<PlayerInfo>();
    }

    // Update is called once per frame
    void Update()
    {
            
        if (!GameOverInfo.isGameOver)
        {

            switch (playerInfo.Health)
            {

                case 20:
                    heart1.SetActive(false);
                    heart2.SetActive(true);
                    heart3.SetActive(true);
                    break;
                case 10:
                    heart1.SetActive(false);
                    heart2.SetActive(false);
                    heart3.SetActive(true);
                    break;
                case 0: //Game Over
                    heart1.SetActive(false);
                    heart2.SetActive(false);
                    heart3.SetActive(false);
                    GameOverInfo.isGameOver = true;
                   // Destroy(gameObject);
                    break;
            }
        }


    }
}
