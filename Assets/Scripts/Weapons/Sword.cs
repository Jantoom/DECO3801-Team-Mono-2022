using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField]
    protected GameObject swingPrefab;
    [SerializeField]
    protected int range = 3;
    protected PlayerInfo owner = null;
    public PlayerInfo Owner { get => owner; set => owner = owner ?? value; }

    void Start()
    {
        swingPrefab.GetComponent<Explosion>().Owner = owner;
        var explosion = Instantiate(swingPrefab, transform.position + Vector3.forward, transform.rotation);
        Destroy(gameObject);
    }
}
