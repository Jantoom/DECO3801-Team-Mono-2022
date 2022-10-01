using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUse : MonoBehaviour
{
    private PlayerInfo playerInfo;

    void Awake() {
        playerInfo = GetComponent<PlayerInfo>();
    }
    void Update()
    {
        if (Input.GetKeyDown(playerInfo.ItemKey) && 
            GetComponent<PlayerMove>().MoveStatus == MoveCode.STATIONARY) {
            GetComponent<BaseItem>()?.UseItem();
        }
    }
}
