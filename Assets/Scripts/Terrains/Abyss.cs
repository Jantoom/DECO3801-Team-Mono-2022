using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abyss : MonoBehaviour
{
    private GameObject gameInfo;
    // Start is called before the first frame update
    void Start()
    {
        gameInfo = GameObject.Find("GameInfo");
    }

    // Update is called once per frame
    void Update()
    {
        if (gameInfo.GetComponent<TerrainGenerator>().IsGenerating) {
            gameObject.transform.Translate(Vector3.forward * TerrainGenerator.GENERATION_COOLDOWN * Time.deltaTime, Space.World);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            gameInfo.GetComponent<PlayerGenerator>().Spawn(other.gameObject);
            other.gameObject.GetComponent<IDestructible>().TakeDamage(GameInfo.BASE_HEALTH * 10);
        }
    }
}
