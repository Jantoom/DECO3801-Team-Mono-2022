using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Describes the behaviour of a laser that is spawned by the player. It will typically shoot a long
// laser beam in the direction of the player until it hits an unbreakable wall.
public class Laser : MonoBehaviour
{
    [SerializeField] private GameObject _beamPrefab;
    [SerializeField] private int _range = 3;

    void Start()
    {
        for (var length = 1; length < _range; length++) {
            var blocked = false;
            var spawnPos = transform.position + transform.forward * length;
            foreach (var collider in Physics.OverlapSphere(spawnPos, 0.3f)) {
                // Lasers are not able to pass through strong walls
                if (collider.gameObject.TryGetComponent<WallUnbreakable>(out var temp)) {
                    blocked = true;
                    break;
                }
            }
            if (blocked) break;

            Instantiate(_beamPrefab, spawnPos, transform.rotation);
        }

        Destroy(gameObject);
    }
}
