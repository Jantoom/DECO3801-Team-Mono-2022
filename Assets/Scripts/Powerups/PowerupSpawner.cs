using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PowerupSpawner : MonoBehaviour
{

    public TerrainGenerator terrainGenerator;
    private Queue<GameObject> activeRows;
    private float SPAWN_DELAY = 5.0f;
    private List<GameObject> powerups;

    // Start is called before the first frame update
    void Start()
    {
        terrainGenerator = GetComponent<TerrainGenerator>();
        powerups = Resources.LoadAll<GameObject>("Prefabs/Powerups").ToList();
        InvokeRepeating("SpawnPowerups", SPAWN_DELAY, SPAWN_DELAY);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void SpawnPowerups() {
        Debug.Log("Spawn Powerups");
        GameObject emptyCell = FindEmptyCell();
        GameObject randomPowerup = GetRandomPowerup();
        Instantiate(randomPowerup, emptyCell.transform.position, Quaternion.identity, emptyCell.transform);
    }

    private GameObject FindEmptyCell() {
        activeRows = terrainGenerator.getActiveRows();
        Transform firstRow = activeRows.Peek().transform;
        foreach (Transform cell in firstRow) {
            if (cell.childCount == 1) {
                return cell.gameObject;
            }
        }
        return null;
    }

    private GameObject GetRandomPowerup() {
        return powerups[Random.Range(0, powerups.Count - 1)];
    }
}
