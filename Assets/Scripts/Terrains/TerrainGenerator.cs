using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    private Vector3 rollingRowSpawnPoint = new Vector3(0f, -0.5f, 0f);
    [SerializeField]
    private GameObject[] terrainPrefabs;
    [SerializeField]
    private RuntimeAnimatorController animController;
    private GameObject parentObject;

    void Start()
    {
        parentObject = GameObject.Find("Map");
        StartCoroutine(SpawnTerrain());
    }

    private IEnumerator SpawnTerrain() {
        var terrainCount = 0;
        while (true) {
            var terrainPrefab = PickNextRandomValidTerrain();
            for (int i = 0; i < terrainPrefab.transform.childCount; i++) {
                var rowPrefab = terrainPrefab.transform.GetChild(i).gameObject;
                var spawnedRow = Instantiate(new GameObject(), parentObject.transform.position + new Vector3(0, -0.5f, i + 10f * terrainCount), parentObject.transform.rotation);
                spawnedRow.transform.SetParent(parentObject.transform);
                for (int j = 0; j < rowPrefab.transform.childCount; j++) {
                    var cellPrefab = rowPrefab.transform.GetChild(j).gameObject;
                    var spawnedCell = Instantiate(cellPrefab, parentObject.transform.position + new Vector3(j, -0.5f, i + 10f * terrainCount), parentObject.transform.rotation);
                    spawnedCell.AddComponent<Animator>().runtimeAnimatorController = animController;
                    spawnedCell.transform.SetParent(spawnedRow.transform);
                }               
                yield return new WaitForSeconds(1.0f);
            }
            terrainCount++;
        }
    }

    private GameObject PickNextRandomValidTerrain() {
        return terrainPrefabs[0];
    }
}