using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used for the large game object resting at the bottom of the game map. Used to detect when a
// player has fallen from the active terrain, and will signal to respawn them. If a fallen object
// is not a player, they are aesthetically removed from the environment.
public class Abyss : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            PlayerGenerator.Spawn(other.gameObject);
            other.gameObject.GetComponent<IDestructible>().TakeDamage(GameInfo.BASE_HEALTH * 10);
        } else {
            // Anything else dies
            StartCoroutine(SlowDeath(other.gameObject.transform));
        }
    }
    //
    // Summary:
    //     Waits a while before destroying the fallen object. This is done for aesthetic purposes.
    private IEnumerator SlowDeath(Transform faller)
    {
        yield return new WaitForSeconds(2.0f);
        var parent = faller.parent;
        Destroy(faller.gameObject);
        if (parent.childCount == 1) Destroy(parent.gameObject);
    }
}
