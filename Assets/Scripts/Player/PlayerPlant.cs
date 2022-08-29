using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlant : MonoBehaviour
{
    [SerializeField]
    private GameObject bombPrefab;
    private int playerNumber;

    internal void SetPlayerNumber(int playerNum)
    {
        playerNumber = playerNum;
    }
    void Update()
    {
        if (playerNumber.Equals(1))
        {
            if (Input.GetKeyDown(KeyCode.Space) &&
                GetComponent<PlayerMove>().MoveStatus == MoveCode.STATIONARY)
                Instantiate(bombPrefab, transform.position, Quaternion.identity);
        } else if (playerNumber.Equals(2))
        {
            if (Input.GetKeyDown(KeyCode.Return) &&
                GetComponent<PlayerMove>().MoveStatus == MoveCode.STATIONARY)
                Instantiate(bombPrefab, transform.position, Quaternion.identity);
        }
    }


}
