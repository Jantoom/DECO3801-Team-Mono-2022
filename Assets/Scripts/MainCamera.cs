using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("GameInfo").GetComponent<TerrainGenerator>().IsGenerating) {
            transform.Translate(Vector3.forward * TerrainGenerator.GENERATION_SPEED * Time.deltaTime, Space.World);
        }
    }
}
