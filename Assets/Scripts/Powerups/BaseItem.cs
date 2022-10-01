using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : MonoBehaviour
{
    protected PlayerInfo playerInfo;

    void Awake() {
        playerInfo = GetComponent<PlayerInfo>();
    }
    public abstract void UseItem();
}
