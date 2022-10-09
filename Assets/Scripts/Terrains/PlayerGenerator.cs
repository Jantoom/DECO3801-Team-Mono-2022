using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerGenerator : MonoBehaviour
{
    private static readonly int SPAWN_ROW_BUFFER = 3;
    private Transform mapTran;

    void Start()
    {
        mapTran = GameObject.Find("Map").transform;
    }

    public void Spawn(GameObject player) {
        var spawnCellTran = player.name == "Player1" ? FindSpawnPlayerOne() : FindSpawnPlayerTwo();
        if (spawnCellTran != null) {
            player.transform.SetPositionAndRotation(spawnCellTran.position + Vector3.up, spawnCellTran.rotation);
        } else {
            Debug.LogError("Couldn't spawn player");
        }
    }

    private Transform FindSpawnPlayerOne() {
        for (int terrainCount = 1; terrainCount < mapTran.childCount; terrainCount++) {
            var terrainTran = mapTran.GetChild(terrainCount);
            for (int rowCount = SPAWN_ROW_BUFFER; rowCount < terrainTran.childCount; rowCount++) {
                var rowTran = terrainTran.GetChild(rowCount);
                for (int cellCount = 0; cellCount < Mathf.FloorToInt(rowTran.childCount / 2); cellCount++) {
                    var cellTran = rowTran.GetChild(cellCount);
                    if (cellTran.childCount < 2) {
                        return cellTran;
                    }
                }
            }
        }
        return null;
    }

    private Transform FindSpawnPlayerTwo() {
        for (int terrainCount = 1; terrainCount < mapTran.childCount; terrainCount++) {
            var terrainTran = mapTran.GetChild(terrainCount);
            for (int rowCount = SPAWN_ROW_BUFFER; rowCount < terrainTran.childCount; rowCount++) {
                var rowTran = terrainTran.GetChild(rowCount);
                for (int cellCount = rowTran.childCount - 1; cellCount > Mathf.FloorToInt(rowTran.childCount / 2); cellCount--) {
                    var cellTran = rowTran.GetChild(cellCount);
                    if (cellTran.childCount < 2) {
                        return cellTran;
                    }
                }
            }
        }
        return null;
    }
}
