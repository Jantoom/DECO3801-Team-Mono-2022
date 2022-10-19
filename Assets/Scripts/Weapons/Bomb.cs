using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Describes the behaviour of a bomb that is spawned by a player. It will typically explode after
// a short delay.
public class Bomb : MonoBehaviour, IDestructible
{
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private int _range = 3;
    [SerializeField] private float _delay = 2.0f, _speed = 0.1f;
    private bool _isExploding = false;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(_delay);
        if (!_isExploding) StartCoroutine(Explode());
    }
    public void TakeDamage(int damage)
    {
        if (!_isExploding) StartCoroutine(Explode());
    }
    //
    // Summary:
    //     Explodes the bomb, spawning explosion objects in every direction.
    private IEnumerator Explode()
    {
        _isExploding = true;
        FindObjectOfType<AudioManager>().Play("BombSound");
        gameObject.GetComponent<Renderer>().enabled = false;

        var directions = new Vector3[] { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        for (int radius = 1; radius < _range; radius++) {
            foreach (var direction in directions) {
                var spawnPos = transform.position + direction * radius;
                Instantiate(_explosionPrefab, spawnPos, Quaternion.identity);
            }
            yield return new WaitForSeconds(_speed);
        }

        Destroy(gameObject);
    }
}
