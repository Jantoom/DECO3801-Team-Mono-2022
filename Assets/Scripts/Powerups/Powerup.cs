using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
    // Powerup Owner
    protected PlayerInfo playerInfo;
    // Powerup Duration
    protected float startTime = 0f, tick = 0.10f;
    public virtual float Duration { get => 0f; }
    public float StartTime { get => startTime; }
    public float EndTime { get => StartTime + Duration; }
    // Powerup State
    public bool Activated = false;
    public virtual bool IsItem { get => false; }

    void Start() {
        playerInfo = gameObject.GetComponent<PlayerInfo>();
    }

    void OnCollisionEnter(Collision collision) {
        if (playerInfo == null && collision.gameObject.TryGetComponent<PlayerInfo>(out playerInfo) && playerInfo.LoadedPowerup == null) {
            // Collision is a first encounter with a player not currently holding an unactivated powerup
            var powerup = (Powerup) playerInfo.Player.AddComponent(this.GetType());
            playerInfo.LoadedPowerup = powerup;
            // No further functionality required from collectable game object
            Destroy(gameObject);
        }
    }

    public virtual void Activate() {
        if (!Activated) {
            foreach (var powerup in playerInfo.GetComponents(this.GetType())) {
                // Kill any activated powerups of the same type (effectively refreshes the powerup)
                if (powerup != this) {
                    ((Powerup) powerup).EndPowerup();
                }
            }
            playerInfo.LoadedPowerup = null;
            Activated = true;
            StartPowerup();
        }
    }

    public virtual void StartPowerup() {
        startTime = Time.time;
        StartCoroutine(RunPowerup());
    }

    public IEnumerator RunPowerup() {
        while (true) {
            TickPowerup();
            if (Time.time >= EndTime) {
                EndPowerup();
            }
            yield return new WaitForSeconds(tick);
        }
    }

    public virtual void TickPowerup() {
    }

    public virtual void EndPowerup() {
        Destroy(GetComponent(this.GetType()));
    }
}
