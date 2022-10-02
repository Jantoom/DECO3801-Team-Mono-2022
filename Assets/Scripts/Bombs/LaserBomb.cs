using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBomb : BaseBomb
{
    protected override IEnumerator Explode() {
        isExploding = true;
        explosionPrefab.GetComponent<Explosion>().Owner = owner;
        var length = 1;
        do {
            var blocked = false;
            var spawnPos = transform.position + transform.forward * length;
            foreach (var collider in Physics.OverlapSphere(spawnPos, 0.3f)) {
                if (collider.gameObject.TryGetComponent<WallStrong>(out var temp)) {
                    blocked = true;
                    break;
                }
            }
            if (blocked) {
                break;
            } else {
                var explosion = Instantiate(explosionPrefab, spawnPos, transform.rotation);
            }
            length++;
        } while (length < range);

        Destroy(gameObject);
        yield return null;
    }
}
