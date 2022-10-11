using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerGenerator : MonoBehaviour
{
    public static readonly int SPAWN_ROW_BUFFER = 3;

    public static void Spawn(GameObject player) {
        var terrains = GameObject.Find("Map").transform;
        foreach (Transform terrain in terrains) {
            var rows = Enumerable.Range(0, terrain.childCount).Select(x => terrain.GetChild(x)).Skip(SPAWN_ROW_BUFFER);
            foreach (Transform row in rows) {
                var cells = Enumerable.Range(0, row.childCount).Select(x => row.GetChild(x));
                cells = player.name == "Player1" ?
                    // Can only spawn in left half of cells, preferring leftmost
                    cells.Take(Mathf.FloorToInt(row.childCount / 2)) :
                    // Can only spawn in right half of cells, preferring rightmost
                    cells.Skip(Mathf.CeilToInt(row.childCount / 2)).Reverse();
                foreach (var cell in cells) {
                    if (cell.childCount < 2) {
                        player.transform.SetPositionAndRotation(cell.position + Vector3.up, cell.rotation);
                        return;
                    }
                }
            }
        }
        Debug.LogError("Couldn't spawn player!");
    }
}
