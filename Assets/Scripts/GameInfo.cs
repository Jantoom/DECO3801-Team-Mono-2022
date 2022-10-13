using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo : MonoBehaviour
{
    public static readonly int BASE_HEALTH = 1;
    [field: SerializeField] public RuntimeAnimatorController SpawnAnimator { get; private set; }
}
