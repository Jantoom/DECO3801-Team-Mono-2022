using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPlant : MonoBehaviour
{
    public GameObject bombPrefab;
    private Rigidbody playerRb;
    private Vector3 spawnPo;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space key was pressed.");
            PlantBomb();
        }

    }
    void PlantBomb()
    {
        spawnPo = playerRb.transform.position;
        Instantiate(bombPrefab, spawnPo, bombPrefab.transform.rotation);
                                                                        
    }
}
