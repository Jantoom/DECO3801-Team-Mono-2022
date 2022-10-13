using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
