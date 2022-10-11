using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, IDestructible
{
    [SerializeField]
    protected GameObject explosionPrefab;
    [SerializeField]
    protected int range = 3;
    [SerializeField]
    protected float delay = 2.0f, speed = 0.1f;
    protected PlayerInfo owner = null;
    protected bool isExploding = false;
    public PlayerInfo Owner { get => owner; set => owner = owner ?? value; }

    void Start()
    {
        StartCoroutine(StartExplosion());
    }

    protected virtual IEnumerator StartExplosion() {
        yield return new WaitForSeconds(delay);
        if (!isExploding) StartCoroutine(Explode());
    }

    public void TakeDamage(int damage) {
        if (!isExploding) StartCoroutine(Explode());
    }

    protected virtual IEnumerator Explode() {
        isExploding = true;
        FindObjectOfType<AudioManager>().play("BombSound");
        gameObject.GetComponent<Renderer>().enabled = false;
        explosionPrefab.GetComponent<Explosion>().Owner = owner;

        var directions = new Vector3[] { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        for (int radius = 1; radius < range; radius++) {
            foreach (var direction in directions) {
                var spawnPos = transform.position + direction * radius;
                Instantiate(explosionPrefab, spawnPos, Quaternion.identity);
            }
            yield return new WaitForSeconds(speed);
        }

        Destroy(gameObject);
    }
}
