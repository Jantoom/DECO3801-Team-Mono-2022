using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialBomb : BaseBomb
{
    // Too hard :(((
    protected override IEnumerator Explode() {
        isExploding = true;
        gameObject.GetComponent<Renderer>().enabled = false;
        explosionPrefab.GetComponent<Explosion>().Owner = owner;
        
        var directions = new List<Vector3> { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
        var radius = 0;
        do {
            foreach (var direction in directions) {
                var blocked = false;
                var spawnPos = transform.position + direction * radius;
                foreach (var collider in Physics.OverlapSphere(spawnPos, 1f)) {
                    if (collider.GetComponent<WallStrong>()) {
                        blocked = true;
                        break;
                    }
                }
                if (blocked) {
                    directions.Remove(direction);
                } else {
                    var explosion = Instantiate(explosionPrefab, spawnPos, Quaternion.identity);
                }
            }
            radius++;
            yield return new WaitForSeconds(speed);
        } while (radius < range);

        Destroy(gameObject);
    }
}
