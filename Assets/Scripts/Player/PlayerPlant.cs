using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlant : MonoBehaviour
{
    [SerializeField]
    private GameObject bombPrefab;
    private PlayerInfo playerInfo;

    void Awake() {
        playerInfo = GetComponent<PlayerInfo>();
    }
    void Update()
    {
        if (Input.GetKeyDown(playerInfo.BombKey) && 
            GetComponent<PlayerMove>().MoveStatus == MoveCode.STATIONARY) {
            Instantiate(bombPrefab, transform.position, Quaternion.identity);
        }
    }
}
