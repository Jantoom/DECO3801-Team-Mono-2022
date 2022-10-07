using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBomb : MonoBehaviour, IDestructible
{
    [SerializeField]
    protected GameObject explosionPrefab;
    [SerializeField]
    protected int range = 3;
    [SerializeField]
    protected float damage = GameInfo.BASE_HEALTH, delay = 2.0f, speed = 0.1f;
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
        gameObject.GetComponent<Renderer>().enabled = false;
        explosionPrefab.GetComponent<Explosion>().Owner = owner;
        
        var directions = new List<Vector3> { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
        var radius = 0;
        do {
            var removeList = new List<Vector3> {};
            foreach (var direction in directions) {
                var blocked = false;
                var spawnPos = transform.position + direction * radius;
                foreach (var collider in Physics.OverlapSphere(spawnPos, 0.3f)) {
                    if (collider.gameObject.TryGetComponent<WallStrong>(out var temp)) {
                        blocked = true;
                        break;
                    }
                }
                if (blocked) {
                    removeList.Add(direction);
                } else {
                    var explosion = Instantiate(explosionPrefab, spawnPos, Quaternion.identity);
                }
            }
            foreach (var direction in removeList) {
                directions.Remove(direction);
            }
            radius++;
            yield return new WaitForSeconds(speed);
        } while (radius < range);

        Destroy(gameObject);
        FindObjectOfType<AudioManager>().play("BombSound");
    }
}
