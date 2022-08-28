using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlant : MonoBehaviour
{
    [SerializeField]
    private GameObject bombPrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) &&
            GetComponent<PlayerMove>().MoveStatus == MoveCode.STATIONARY)
            Instantiate(bombPrefab, transform.position, Quaternion.identity);
    }
}
