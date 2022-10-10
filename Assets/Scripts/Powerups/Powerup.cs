using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
    // Powerup Owner
    public PlayerInfo PlayerInfo;
    // Powerup State
    public bool Activated = false;
    // Powerup Duration
    protected float startTime = 0f, tick = 0.10f;
    public float Duration;
    public float StartTime { get => startTime; }
    public float EndTime { get => StartTime + Duration; }
    
    void Start() {
        PlayerInfo = gameObject.GetComponent<PlayerInfo>();
    }

    void OnCollisionEnter(Collision collision) {
        if (PlayerInfo == null && collision.gameObject.TryGetComponent<PlayerInfo>(out PlayerInfo) && PlayerInfo.LoadedPowerup == null) {
            // Collision is a first encounter with a player not currently holding an unactivated powerup
            var powerup = (Powerup) PlayerInfo.Player.AddComponent(this.GetType());
            PlayerInfo.LoadedPowerup = powerup;
            // No further functionality required from collectable game object
            Destroy(gameObject);
        }
    }

    public virtual void KillDuplicatePowerups() {
        foreach (var powerup in PlayerInfo.GetComponents(this.GetType())) {
            // Kill any activated powerups of the same type (effectively refreshes the powerup)
            if (powerup != this) {
                ((Powerup) powerup).EndPowerup();
            }
        }
    }

    public virtual void Activate() {
        if (!Activated) {
            KillDuplicatePowerups();
            PlayerInfo.LoadedPowerup = null;
            Activated = true;
            StartPowerup();
        }
    }

    public virtual void StartPowerup() {
        startTime = Time.time;
        StartCoroutine(RunPowerup());
    }

    public IEnumerator RunPowerup() {
        while (Time.time < EndTime) {
            TickPowerup();
            yield return new WaitForSeconds(tick);
        }
        EndPowerup();
    }

    public virtual void TickPowerup() {
    }

    public virtual void EndPowerup() {
        Destroy(GetComponent(this.GetType()));
    }
}
