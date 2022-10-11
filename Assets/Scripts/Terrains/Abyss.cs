using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abyss : MonoBehaviour
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
        } else {
            // Anything else dies
            StartCoroutine(SlowDeath(other.gameObject.transform));
        }
    }

    private IEnumerator SlowDeath(Transform faller) {
        yield return new WaitForSeconds(2.0f);
        var parent = faller.parent;
            Destroy(faller.gameObject);
            if (parent.childCount == 1) Destroy(parent.gameObject);
    }
}
