using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO.Ports;

public class PlayerControls : MonoBehaviour
{
    private PlayerInfo _playerInfo;
    // Control Type
    public static SerialPort SerialInput = new SerialPort("COM4", 9600);
    public static bool UseSerialControls = false, SerialInputOpen = false;
        
    // Controls from Arduino
    public bool UseSerialControls { get; private set; } = true;
    // Serial Controls from Arduino
    public static SerialPort serialInput = new SerialPort("COM4", 9600);
    public static bool serialInputOpen = false;
    [field: SerializeField] public int ForwardSignal { get; private set; }
    [field: SerializeField] public int BackSignal { get; private set; }
    public bool UseSerialControls { get; private set; } = false;
    // Serial Controls from Arduino
    public static SerialPort serialInput = new SerialPort("COM4", 9600);
    public static bool serialInputOpen = false;
    [field: SerializeField] public int ForwardSignal { get; private set; }
    [field: SerializeField] public int BackSignal { get; private set; }
    [field: SerializeField] public int LeftSignal { get; private set; }
    [field: SerializeField] public int ForwardSignal { get; private set; }
    [field: SerializeField] public int RightSignal { get; private set; }
    // Controls from Keyboard
    [field: SerializeField] public KeyCode LeftKey { get; private set; }
    [field: SerializeField] public KeyCode ForwardKey { get; private set; }
    [field: SerializeField] public KeyCode RightKey { get; private set; }
    // Control Times
    private static readonly float ACTIVATION_WAIT_TIME = 0.15f, MOVE_TIME = 0.20f, REBOUND_TIME = 0.40f;
    public float TimeToMove { get => MOVE_TIME * (_playerInfo.Exhausted ? 10 : 1); }
    public float TimeToRebound { get => REBOUND_TIME * (_playerInfo.Exhausted ? 10 : 1); }
    private float _timeAtLastInput = 0.0f;
    // Control Movement
    public MoveCode MoveStatus { get; private set; } = MoveCode.STATIONARY;
    private Vector3[] _lastDirections = new Vector3[2] { Vector3.forward, Vector3.forward };
    private Vector3 _lastPosition;
    private IEnumerator _moveCoroutine = null;

    void Awake()
    {
        _playerInfo = GetComponent<PlayerInfo>();
        if (UseSerialControls && !SerialInputOpen) {
            SerialInput.Open();
            SerialInput.ReadTimeout = 1;
            SerialInputOpen = true;
        }
    }
    void Update()
    {
        // Player cannot move while frozen
        if (_playerInfo.Frozen) return;

        if (UseSerialControls) {
            ProcessDirection(SignalToDirection(SerialInput.BytesToRead > 0 ? SerialInput.ReadByte() : 0));
        } else {
            ProcessDirection(KeyCodeToDirection(KeyDownToKeyCode()));
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        // Player is free to traverse while ghosted
        if (_playerInfo.Ghosted) return;

        if (collision.gameObject.CompareTag("Wall") && MoveStatus.Equals(MoveCode.MOVING)) {
            // Player is moving into a wall from their own input
            if (!collision.gameObject.TryGetComponent<WallWeak>(out var wall) || 
                wall.Health - _playerInfo.Attack > 0) {
                // Player won't break the wall on collision
                MovePlayer(_lastPosition, TimeToRebound, MoveCode.REBOUNDING);
            }
            // If the colliding object is destructible, do collision damage to it
            collision.gameObject.GetComponent<IDestructible>()?.TakeDamage(_playerInfo.Attack);
        }
    }
    void OnCollisionStay(Collision collision)
    {
        // Player is free to traverse while ghosted or collider is freefalling from terrain
        if (_playerInfo.Ghosted || collision.collider.attachedRigidbody.useGravity) return;

        if (collision.gameObject.CompareTag("Wall") && MoveStatus.Equals(MoveCode.STATIONARY)) {
            // Player isn't moving themselves out of a wall
            var directions = new Vector3[] { Vector3.forward, Vector3.left, Vector3.right };
            for (var range = 1; range < 5; range++) {
                foreach (var direction in directions) {
                    var overlaps = Physics.OverlapSphere(transform.position + direction * range, 0.4f);
                    if (!overlaps.Any(x => x.CompareTag("Wall")) && 
                        overlaps.Any(x => x.CompareTag("Tile")) &&
                        overlaps.All(x => x.attachedRigidbody.useGravity == false)) {
                        // This cell has a tile to stand on and isn't blocked by a wall
                        MovePlayer(transform.position + direction * range, TimeToMove, MoveCode.FORCED);
                        return;
                    }
                }
            }
            // Player has no immediate cells to move into, so just find one using respawn algorithm
            PlayerGenerator.Spawn(gameObject);
        }
    }
    //
    // Summary:
    //     If the current and previous directions are left/right and received within the activation 
    //     time, the player's loaded powerup will activate. Otherwise, the direction is treated as 
    //     movement.
    private void ProcessDirection(Vector3 direction)
    {
        if (direction != Vector3.zero) {
            if ((direction + _lastDirections[0]).Equals(Vector3.zero) &&
                Time.time - _timeAtLastInput < ACTIVATION_WAIT_TIME) {
                ActivateItem();
                _lastDirections[0] = _lastDirections[1];  // This direction was used for powerups
            } else if (MoveStatus == MoveCode.STATIONARY && direction != Vector3.zero) {
                MovePlayer(transform.position + direction, TimeToMove, MoveCode.MOVING);
                _lastDirections[1] = _lastDirections[0];
                _lastDirections[0] = direction;
            }
            _timeAtLastInput = Time.time;
        }
    }
    //
    // Summary:
    //     Activates the player's loaded powerup. If the powerup is a weapon, then it is used.
    private void ActivateItem()
    {
        // Stop current movement
        MovePlayer(_lastPosition, 0.0f, MoveCode.FORCED);
        // Ensure proper rotation
        transform.LookAt(transform.position + _lastDirections[1]);
        // Powerup activation
        var powerup = _playerInfo.LoadedPowerup;
        if (powerup != null) powerup.Activate();
        // If player has a weapon, use it
        var weapon = _playerInfo.WeaponPrefab;
        if (weapon != null) Instantiate(weapon, transform.position, transform.rotation);
    }
    //
    // Summary:
    //     Moves player to the finish position within the given time duration.
    //
    // Parameters:
    //   finish:
    //     the finish position
    //   duration:
    //     the duration of the movement
    //   status:
    //     status of the movement, used for private and public movement logic
    private void MovePlayer(Vector3 finish, float duration, MoveCode status)
    {
        MoveStatus = status;
        if (MoveStatus != MoveCode.REBOUNDING) transform.LookAt(finish);
        if (MoveStatus == MoveCode.MOVING) GetComponent<Rigidbody>().AddForce(Vector3.up * 2.5f, ForceMode.Impulse);

        if (_moveCoroutine != null) {
            StopCoroutine(_moveCoroutine);
        } else {
            _lastPosition = transform.position;
        }
        _moveCoroutine = Move(transform.position, finish, duration);
        StartCoroutine(_moveCoroutine);
    }
    //
    // Summary:
    //     Moves the player from the start to the finish positions, using linear interpolation.
    //
    // Parameters:
    //   start:
    //     the start position
    //   finish:
    //     the finish position
    //   duration:
    //     the duration of the movement
    //
    // Returns:
    //     The enumerator for this coroutine.
    private IEnumerator Move(Vector3 start, Vector3 finish, float duration)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < duration) {
            var lerp = Vector3.Lerp(start, finish, (elapsedTime / duration));
            transform.position = new Vector3(lerp.x, transform.position.y, lerp.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector3(finish.x, transform.position.y, finish.z);

        _lastPosition = transform.position;
        MoveStatus = MoveCode.STATIONARY;
        _moveCoroutine = null;
    }
    //
    // Summary:
    //     Converts a serial input to a vectorised direction, based on the player's recognised input
    //     signals.
    //
    // Returns:
    //     The vectorised direction. If unrecognised signal, Vector3.zero is returned.
    private Vector3 SignalToDirection(int signal)
    {
        if (signal == ForwardSignal) {
            return Vector3.forward;
        } else if (signal == LeftSignal) {
            return Vector3.left;
        } else if (signal == RightSignal) {
            return Vector3.right;
        } else {
            return Vector3.zero;
        }
    }
    //
    // Summary:
    //     Converts a keycode to a vectorised direction, based on the player's recognised input
    //     signals.
    //
    // Returns:
    //     The vectorised direction. If unrecognised keycode, Vector3.zero is returned.
    private Vector3 KeyCodeToDirection(KeyCode keyCode)
    {
        if (keyCode == ForwardKey) {
            return Vector3.forward;
        } else if (keyCode == LeftKey) {
            return Vector3.left;
        } else if (keyCode == RightKey) {
            return Vector3.right;
        } else {
            return Vector3.zero;
        }
    }
    //
    // Summary:
    //     Queries all recognised key down inputs for the player. Inputs may be lost if players 
    //     press down on several keys within the same frame.
    //
    // Returns:
    //     The first received key down input. If none received, KeyCode.None is returned.
    private KeyCode KeyDownToKeyCode()
    {
        if (Input.GetKeyDown(ForwardKey)) {
            return ForwardKey;
        } else if (Input.GetKeyDown(LeftKey)) {
            return LeftKey;
        } else if (Input.GetKeyDown(RightKey)) {
            return RightKey;
        } else {
            return KeyCode.None;
        }
    }
}

public enum MoveCode
{
    STATIONARY, MOVING, REBOUNDING, FORCED
}