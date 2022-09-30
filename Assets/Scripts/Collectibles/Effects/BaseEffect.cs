using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEffect : MonoBehaviour
{
    protected PlayerInfo playerInfo;
    protected float startTime = 0f, tick = 0.25f;
    public virtual float Duration { get => 0f; }
    public float StartTime { get => startTime; }
    public float EndTime { get => StartTime + Duration; }

    void Awake() {
        playerInfo = GetComponent<PlayerInfo>();
    }
    public virtual void StartEffect() {
        startTime = Time.time;
        StartCoroutine(RunEffect());
    }
    public virtual void ExtendEffect() {
        startTime = Time.time;
    }
    public IEnumerator RunEffect() {
        while (true) {
            if (Time.time >= EndTime) {
                EndEffect();
            }
            TickEffect();
            yield return new WaitForSeconds(tick);
        }
    }
    public virtual void TickEffect() {}
    public virtual void EndEffect() {
        Destroy(this);
    }
}
