using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    void Update()
    {
        if (GameObject.Find("GameInfo").GetComponent<TerrainGenerator>().IsGenerating) {
            gameObject.transform.Translate(Vector3.forward * (1 / TerrainGenerator.GENERATION_SPEED) * Time.deltaTime, Space.World);
        }
    }
}
