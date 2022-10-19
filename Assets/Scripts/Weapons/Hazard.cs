using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// General behaviour for an entity that deals damage to anything it touches. Think of this as the
// explosion from a bomb, or the beam from a laser gun.
public class Hazard : MonoBehaviour
{
    [SerializeField] private int _damage = GameInfo.BASE_HEALTH * 20;
    [SerializeField] private float _persistence = 0.2f;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(_persistence);
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<IDestructible>()?.TakeDamage(_damage);
    }
}
