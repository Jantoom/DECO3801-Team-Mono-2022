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
    public float StartTime { get; private set; } = 0.0f;
    public float Duration;
    public float EndTime { get => StartTime + Duration; }
    protected float _tick = 0.10f;
    // Powerup UI
    [SerializeField] private Sprite _loadedImage, _effectImage;
    [SerializeField] private ParticleSystem _effectParticles;
    
    void Start()
    {
        PlayerInfo = gameObject.GetComponent<PlayerInfo>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (PlayerInfo == null && collision.gameObject.TryGetComponent<PlayerInfo>(out var info)) {
            // First encounter with a player
            Destroy(info.LoadedPowerup);
            var powerup = (Powerup) info.Player.AddComponent(this.GetType());
            powerup.Duration = Duration;
            info.LoadedPowerup = powerup;
            FindObjectOfType<AudioManager>().play("PowerupSound");
            // No further functionality required from collectable game object
            Destroy(gameObject);
        }
        
    }
    void OnDestroy()
    {
        if (PlayerInfo != null) EndPowerup();
    }
    //
    // Summary:
    //     Activates this powerup if is not yet already activated. Activation includes killing
    //     duplicate powerups.
    public virtual void Activate()
    {
        if (!Activated) {
            KillDuplicatePowerups();
            PlayerInfo.LoadedPowerup = null;
            Activated = true;
            StartPowerup();
        }
    }
    //
    // Summary:
    //     Starts the effect given by this powerup.
    protected virtual void StartPowerup()
    {
        StartTime = Time.time;
        StartCoroutine(RunPowerup());
    }
    //
    // Summary:
    //     Main loop that is run while the powerup has not completed its duration.
    //
    // Returns:
    //     The enumerator for this coroutine.
    protected IEnumerator RunPowerup()
    {
        while (Time.time < EndTime) {
            TickPowerup();
            yield return new WaitForSeconds(_tick);
        }
        EndPowerup();
    }
    //
    // Summary:
    //     Ran every tick for the powerup. Useful for tick-based logic.
    protected virtual void TickPowerup()
    {
    }
    //
    // Summary:
    //     Ends the effect given by this powerup. It is typically destroyed after this.
    protected virtual void EndPowerup()
    {
        Destroy(this);
    }
    //
    // Summary:
    //     Searches for similar powerups that the player owns and ends them.
    protected virtual void KillDuplicatePowerups()
    {
        foreach (var powerup in PlayerInfo.GetComponents(this.GetType())) {
            // Kill any activated powerups of the same type (effectively refreshes the powerup)
            if (powerup != this) {
                ((Powerup) powerup).EndPowerup();
            }
        }
    }
}
