using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, IDestructible
{
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private int damage, range;
    [SerializeField]
    private float delay, persistence, speed;
    private bool isExploding = false;

    void Start()
    {
        StartCoroutine(StartExplosion());
    }

    private IEnumerator StartExplosion() {
        yield return new WaitForSeconds(delay);
        if (!isExploding) Explode();
    }

    public void ApplyDamage(int damage) {
        if (!isExploding) {
            Explode();
        }
    }

    private void Explode() {
        isExploding = true;
        gameObject.GetComponent<MeshRenderer>().forceRenderingOff = true;

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        var directions = new Vector3[] { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
        foreach (var direction in directions) {
            var explosion = Instantiate(explosionPrefab, transform.position + direction, Quaternion.identity);
            explosion.GetComponent<Explosion>().Init(damage, range - 1, persistence, speed, direction);
        }

        Destroy(gameObject);
    }
}
