using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private int damage = 1, range = 0;
    private float persistence = 0.2f, speed = 0.1f;
    private Vector3 direction = Vector3.zero;
    private bool initialised;
    private IEnumerator propagateCoroutine;

    void Start()
    {
        propagateCoroutine = Propagate();
        StartCoroutine(propagateCoroutine);
    }

    public void Init(int damage, int range, float persistence, float speed, Vector3 direction) {
        if (!initialised) {
            this.damage = damage;
            this.range = range;
            this.persistence = persistence;
            this.speed = speed;
            this.direction = direction;
            initialised = true;
        }
    }

    private IEnumerator Propagate() {
        yield return new WaitForSeconds(speed);
        if (range > 0) {
            var explosion = Instantiate(gameObject, transform.position + direction, Quaternion.identity);
            explosion.GetComponent<Explosion>().Init(damage, range - 1, persistence, speed, direction);
        }
        yield return new WaitForSeconds(persistence);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision) {
        collision.gameObject.GetComponent<IDestructible>()?.Hit(damage);
        if (collision.gameObject.GetComponent<WallStrong>() != null) {
            Destroy(gameObject);
        }
    }
}
