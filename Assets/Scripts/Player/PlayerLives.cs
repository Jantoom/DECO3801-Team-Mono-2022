using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerLives : MonoBehaviour
{
    private PlayerInfo playerInfo;
    [SerializeField]
    private KeyCode liveKey;

    [SerializeField]
    private Image heart1, heart2, heart3;

    bool gameOverCon;//
    public KeyCode LiveKey { get => liveKey; }  
    public Image Heart1 { get => heart1; }
    public Image Heart2 { get => heart2; }
    public Image Heart3 { get => heart3; }

    private void Awake()
    {
        playerInfo = GetComponent<PlayerInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(liveKey))
        {
            playerInfo.Health --;

            switch (playerInfo.Health)
            {
                case 2:
                    heart1.gameObject.SetActive(false);
                    break;
                case 1:
                    heart2.gameObject.SetActive(false);
                    break;
                case 0: //Game Over
                    heart3.gameObject.SetActive(false);
                    GameOverInfo.isGameOver = true;//


                    break;


            }
        }
    }
}
