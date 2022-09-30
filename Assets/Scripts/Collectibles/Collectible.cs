using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField]
    private Component collectible;

    public void Collect(PlayerInfo owner) {
        var existingCollectible = owner.Player.GetComponent(collectible.GetType());
        if (existingCollectible == null) {
            collectible = owner.Player.AddComponent(collectible.GetType());
            if (collectible.GetType() == typeof(BaseEffect)) {
                ((BaseEffect) collectible).StartEffect();
            } else if (collectible.GetType() == typeof(PlayerItem)) {
                // ??
            }
        } else {
            if (existingCollectible.GetType() == typeof(BaseEffect)) {
                ((BaseEffect) existingCollectible).ExtendEffect();
            } else if (existingCollectible.GetType() == typeof(PlayerItem)) {
                // ??
            }
        }
        
        Destroy(gameObject);
    }
}
