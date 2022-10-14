using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncMovement : MonoBehaviour
{
    private TerrainGenerator _terrainGenerator;
    void Start() {
        _terrainGenerator = GameObject.Find("GameInfo").GetComponent<TerrainGenerator>();
    }
    void Update()
    {
        if (_terrainGenerator.IsGenerating) {
            gameObject.transform.Translate(Vector3.forward * (1 / _terrainGenerator.GenerationSpeed) * Time.deltaTime, Space.World);
        }
    }
}
