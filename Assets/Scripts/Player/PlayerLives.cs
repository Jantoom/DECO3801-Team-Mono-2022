using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Converts a player's health to a displayable array of lives. Also responsible for calculating
// damage taken from objects.
public class PlayerLives : MonoBehaviour, IDestructible
{
    private PlayerInfo _playerInfo;
    // Hearts on UI
    public GameObject Heart1, Heart2, Heart3;

    void Awake()
    {
        _playerInfo = GetComponent<PlayerInfo>();
    }
    public void TakeDamage(int damage)
    {
        if (damage > 0) {
            // Took actual damage/lost a life
            _playerInfo.Health -= GameInfo.BASE_HEALTH;
            UpdateHeartDisplay();
            FindObjectOfType<AudioManager>().Play("LoseLife");
        }
    }
    //
    // Summary:
    //     Updates the number of visible hearts on the UI for this player, based on the number of
    //     lives the player has.
    private void UpdateHeartDisplay()
    {
        switch (_playerInfo.Health) {
        case 3:
            Heart1.SetActive(true);
            Heart2.SetActive(true);
            Heart3.SetActive(true);
            break;
        case 2:
            Heart1.SetActive(true);
            Heart2.SetActive(true);
            Heart3.SetActive(false);
            break;
        case 1:
            Heart1.SetActive(true);
            Heart2.SetActive(false);
            Heart3.SetActive(false);
            break;
        case 0:
            Heart1.SetActive(false);
            Heart2.SetActive(false);
            Heart3.SetActive(false);
            GameOverInfo.isGameOver = true;
            break;
        }
    }
}
