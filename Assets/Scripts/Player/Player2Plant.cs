using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Plant : MonoBehaviour
{
    [SerializeField]
    private GameObject bombPrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) &&
            GetComponent<Player2Move>().MoveStatus == MoveCode.STATIONARY)
            Instantiate(bombPrefab, transform.position, Quaternion.identity);
    }
}
