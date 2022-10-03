using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
    // Powerup Owner
    protected PlayerInfo playerInfo = null;
    public PlayerInfo PlayerInfo { get => playerInfo; set => playerInfo = playerInfo ?? value; }
    // Powerup Duration
    protected float startTime = 0f, tick = 0.25f;
    public virtual float Duration { get => 0f; }
    public float StartTime { get => startTime; }
    public float EndTime { get => StartTime + Duration; }
    private IEnumerator effectCoroutine;
    // Powerup Type
    public virtual bool IsItem { get => false; }

    void OnCollisionEnter(Collision collision) {
        if (playerInfo == null && collision.gameObject.TryGetComponent<PlayerInfo>(out playerInfo)) {
            if (!playerInfo.Player.TryGetComponent(this.GetType(), out var powerup)) {
                powerup = playerInfo.Player.AddComponent(this.GetType());
            }
            ((Powerup) powerup).PlayerInfo = playerInfo;
            ((Powerup) powerup).Activate();
            
            Destroy(gameObject);
        }
    }

    public virtual void Activate() {
        if (effectCoroutine == null) {
            foreach (var powerup in playerInfo.Player.GetComponents<Powerup>()) {
                if (powerup.IsItem && powerup != this) powerup.EndPowerup();
            }
            StartPowerup();
        } else {
            ExtendPowerup();
        }
    }

    public virtual void StartPowerup() {
        startTime = Time.time;
        effectCoroutine = RunPowerup();
        StartCoroutine(effectCoroutine);

    }

    public virtual void ExtendPowerup() {
        startTime = Time.time;
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
