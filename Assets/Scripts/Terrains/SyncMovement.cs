using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to synchronise movement of an object with the speed of the terrain generation.
public class SyncMovement : MonoBehaviour
{
    private TerrainGenerator _terrainGenerator;
    
    void Start()
    {
        _terrainGenerator = GameObject.Find("GameInfo").GetComponent<TerrainGenerator>();
    }
    void Update()
    {
        if (_terrainGenerator.IsGenerating) {
            gameObject.transform.Translate(Vector3.forward * (1 / _terrainGenerator.GenerationSpeed) * Time.deltaTime, Space.World);
        }
    }
}
