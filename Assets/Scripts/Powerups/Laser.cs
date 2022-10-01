using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : BaseItem
{
    [SerializeField]
    private GameObject beamPrefab;
    protected int range = 3;
    protected float damage = GameInfo.BASE_HEALTH, delay = 2.0f, speed = 0.1f;
    protected PlayerInfo owner = null;
    protected bool isExploding = false;
    public PlayerInfo Owner { get => owner; set => owner = owner ?? value; }
    public override void UseItem()
    {
        isExploding = true;
        beamPrefab.GetComponent<Explosion>().Owner = owner;
        var length = 0;
        do {
            var blocked = false;
            var spawnPos = transform.position + transform.rotation * (Vector3.forward * length);
            foreach (var collider in Physics.OverlapSphere(spawnPos, 0.3f)) {
                if (collider.gameObject.TryGetComponent<WallStrong>(out var temp)) {
                    Debug.Log("blocked");
                    blocked = true;
                    break;
                }
            }
            if (blocked) {
                break;
            } else {
                var explosion = Instantiate(beamPrefab, spawnPos, transform.rotation);
            }
            length++;
        } while (length < range);

        Destroy(this);
    }
}
