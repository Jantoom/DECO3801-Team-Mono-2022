using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour {
    public enum Location {
        LEFT,
        MIDDLE,
        RIGHT
    }

    public List<Location> entries;
    public List<Location> exits;
    public List<List<int>> powerupLocations;


}
