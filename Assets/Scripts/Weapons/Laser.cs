using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    protected GameObject beamPrefab;
    [SerializeField]
    protected int range = 3;
    protected PlayerInfo owner = null;
    public PlayerInfo Owner { get => owner; set => owner = owner ?? value; }

    void Start()
    {
        beamPrefab.GetComponent<Explosion>().Owner = owner;
        
        for (var length = 1; length < range; length++) {
            var blocked = false;
            var spawnPos = transform.position + transform.forward * length;
            foreach (var collider in Physics.OverlapSphere(spawnPos, 0.3f)) {
                if (collider.gameObject.TryGetComponent<WallStrong>(out var temp)) {
                    blocked = true;
                    break;
                }
            }
            if (blocked) break;

            Instantiate(beamPrefab, spawnPos, transform.rotation);
        }

        Destroy(gameObject);
    }
}
