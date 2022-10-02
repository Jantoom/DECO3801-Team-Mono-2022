using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private int damage = GameInfo.BASE_HEALTH;
    [SerializeField]
    private float persistence = 0.2f;
    private PlayerInfo owner = null;
    public PlayerInfo Owner { get => owner; set => owner = owner ?? value; }

    void Start()
    {
        StartCoroutine(Fizzle());
    }

    private IEnumerator Fizzle()
    {
        yield return new WaitForSeconds(persistence);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<IDestructible>()?.TakeDamage(damage);
    }
}
