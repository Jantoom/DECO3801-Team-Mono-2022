using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("GameInfo").GetComponent<TerrainGenerator>().IsGenerating) {
            gameObject.transform.Translate(Vector3.forward * TerrainGenerator.GENERATION_SPEED * Time.deltaTime, Space.World);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            PlayerGenerator.Spawn(other.gameObject, true);
            other.gameObject.GetComponent<IDestructible>().TakeDamage(GameInfo.BASE_HEALTH * 10);
        }
    }
}
