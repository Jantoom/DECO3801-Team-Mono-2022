using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Describes the behaviour of a sword that is spawned by the player. 
public class Sword : MonoBehaviour
{
    [SerializeField] private GameObject _swingPrefab;

    void Start()
    {
        Instantiate(_swingPrefab, transform.position + Vector3.forward, transform.rotation);
        Destroy(gameObject);
    }
}
