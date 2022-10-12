using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO.Ports;

public class PlayerControls : MonoBehaviour
{
    private PlayerInfo playerInfo;
    // Control Type
    [field: SerializeField] public bool UseSerialControls { get; private set; } = false;
    // Serial Controls from Arduino
    private static SerialPort serialInput = new SerialPort("COM3", 9600);
    [field: SerializeField] public int ForwardSignal { get; private set; }
    [field: SerializeField] public int BackSignal { get; private set; }
    [field: SerializeField] public int LeftSignal { get; private set; }
    [field: SerializeField] public int RightSignal { get; private set; }
    [field: SerializeField] public int NoSignal { get; private set; }
    // Key Controls from Keyboard
    [field: SerializeField] public KeyCode ForwardKey { get; private set; }
    [field: SerializeField] public KeyCode BackKey { get; private set; }
    [field: SerializeField] public KeyCode LeftKey { get; private set; }
    [field: SerializeField] public KeyCode RightKey { get; private set; }
    // Player Cooldowns
    public static float ITEM_WAIT_TIME = 0.15f;
    private float timeAtLastInput = 0.0f, timeToMove = 0.2f, timeToRebound = 0.4f;
    public float TimeToMove { get => timeToMove * (playerInfo.Exhausted ? 10 : 1); }
    public float TimeToRebound { get => timeToRebound * (playerInfo.Exhausted ? 10 : 1); }
    // Moving
    public MoveCode MoveStatus { get; private set; } = MoveCode.STATIONARY;
    private Vector3 origPosition, targetPosition;
    private Vector3[] lastDirections;
    private IEnumerator moveCoroutine = null;

    void Awake()
    {
        playerInfo = GetComponent<PlayerInfo>();
        // Opens port and sets a timeout between reads
        if (UseSerialControls) {
            serialInput.Open();
            serialInput.ReadTimeout = 1;
        }
        lastDirections = new Vector3[2] { Vector3.zero, Vector3.zero };
    }

    void Update()
    {
        if (playerInfo.Frozen) {
            return;
        }
        if (UseSerialControls) {
            ProcessSerialInput();
        } else {
            ProcessKeyboardInput();
        }
    }

    private void ProcessSerialInput() {
        var signal = serialInput.BytesToRead > 0 ? serialInput.ReadByte() : 0;
        var direction = SignalToDirection(signal);

        if (signal != 0) {
            if (((signal == LeftSignal && lastDirections[0] == Vector3.right) ||
                (signal == RightSignal && lastDirections[0] == Vector3.left)) &&
                Time.time - timeAtLastInput < ITEM_WAIT_TIME) {
                // Ensure proper rotation
                transform.LookAt(transform.position + lastDirections[1]);
                ActivateItem();
            } else if (MoveStatus == MoveCode.STATIONARY && direction != Vector3.zero) {
                moveCoroutine = MovePlayer(direction, false);
                StartCoroutine(moveCoroutine);
                lastDirections[1] = lastDirections[0];
                lastDirections[0] = direction;
            }
            timeAtLastInput = Time.time;
        }
    }

    private void ProcessKeyboardInput() {
        var downKey = KeyDownToKeyCode();
        var direction = KeyCodeToDirection(downKey);

        if (downKey != KeyCode.None) {
            if (((Input.GetKeyDown(LeftKey) && Input.GetKeyDown(RightKey)) ||
                (Input.GetKeyDown(LeftKey) && lastDirections[0] == Vector3.right) ||
                (Input.GetKeyDown(RightKey) && lastDirections[0] == Vector3.left)) &&
                Time.time - timeAtLastInput < ITEM_WAIT_TIME) {
                // Ensure proper rotation
                if (Input.GetKeyDown(LeftKey) && Input.GetKeyDown(RightKey)) {
                    transform.LookAt(transform.position + lastDirections[0]);
                } else {
                    transform.LookAt(transform.position + lastDirections[1]);
                }
                ActivateItem();                
            } else if (MoveStatus == MoveCode.STATIONARY && direction != Vector3.zero) {
                moveCoroutine = MovePlayer(direction, false);
                StartCoroutine(moveCoroutine);
                lastDirections[1] = lastDirections[0];
                lastDirections[0] = direction;
            }
            timeAtLastInput = Time.time;
        }
    }

    private Vector3 SignalToDirection(int signal) {
        if (signal == ForwardSignal) {
            return Vector3.forward;
        } else if (signal == LeftSignal) {
            return Vector3.left;
        } else if (signal == RightSignal) {
            return Vector3.right;
        } else if (signal == BackSignal) {
            return Vector3.back;
        } else {
            return Vector3.zero;
        }
    }

    private Vector3 KeyCodeToDirection(KeyCode keyCode) {
        if (keyCode == ForwardKey) {
            return Vector3.forward;
        } else if (keyCode == LeftKey) {
            return Vector3.left;
        } else if (keyCode == RightKey) {
            return Vector3.right;
        } else if (keyCode == BackKey) {
            return Vector3.back;
        } else {
            return Vector3.zero;
        }
    }

    private KeyCode KeyDownToKeyCode() {
        if (Input.GetKeyDown(ForwardKey)) {
            return ForwardKey;
        } else if (Input.GetKeyDown(LeftKey)) {
            return LeftKey;
        } else if (Input.GetKeyDown(RightKey)) {
            return RightKey;
        } else if (Input.GetKeyDown(BackKey)) {
            return BackKey;
        } else {
            return KeyCode.None;
        }
    }
    
    private void ActivateItem() {
        // Stop current movement
        if (moveCoroutine != null) {
            HaltPlayer();
        }
        // Item Activation
        var powerup = playerInfo.LoadedPowerup;
        if (powerup != null) powerup.Activate();
        // If player has a weapon, use it
        var weapon = playerInfo.WeaponPrefab;
        if (weapon != null) Instantiate(weapon, transform.position, transform.rotation);
    }

    void OnCollisionEnter(Collision collision) {
        if (!playerInfo.Ghosted && collision.gameObject.CompareTag("Wall") && MoveStatus.Equals(MoveCode.MOVING)) {
            if (!collision.gameObject.TryGetComponent<WallWeak>(out var wall) || wall.Health - playerInfo.Attack > 0) {
                StopCoroutine(moveCoroutine);
                StartCoroutine(ReboundPlayer());
            }
            collision.gameObject.GetComponent<IDestructible>()?.TakeDamage(playerInfo.Attack);
        }
    }

    void OnCollisionStay(Collision collision) {
        if (!playerInfo.Ghosted && collision.gameObject.CompareTag("Wall") && MoveStatus.Equals(MoveCode.STATIONARY) && collision.collider.attachedRigidbody.useGravity == false) {
            var directions = new Vector3[] { Vector3.forward, Vector3.left, Vector3.right };
            for (var range = 1; range < 5; range++) {
                foreach (var direction in directions) {
                    var overlaps = Physics.OverlapSphere(transform.position + direction * range, 0.4f);
                    if (!overlaps.Any(x => x.CompareTag("Wall")) && overlaps.Any(x => x.CompareTag("Tile")) && overlaps.All(x => x.attachedRigidbody.useGravity == false)) {
                        StartCoroutine(MovePlayer(direction * range, true));
                        return;
                    }
                }
            }
            PlayerGenerator.Spawn(gameObject, true);
        }
    }

    private IEnumerator MovePlayer(Vector3 direction, bool forced) {
        MoveStatus = forced ? MoveCode.FORCED : MoveCode.MOVING;
        origPosition = transform.position;
        targetPosition = origPosition + direction;
        transform.LookAt(targetPosition);

        float elapsedTime = 0f;
        while (elapsedTime < timeToMove) {
            var lerpPosition = Vector3.Lerp(origPosition, targetPosition, (elapsedTime / timeToMove));
            transform.position = new Vector3(lerpPosition.x, transform.position.y, lerpPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        MoveStatus = MoveCode.STATIONARY;
    }

    private IEnumerator ReboundPlayer() {
        MoveStatus = MoveCode.REBOUNDING;
        var reboundPosition = transform.position;

        float elapsedTime = 0f;
        while (elapsedTime < TimeToRebound) {
            var lerpPosition = Vector3.Lerp(reboundPosition, origPosition, (elapsedTime / TimeToRebound));
            transform.position = new Vector3(lerpPosition.x, transform.position.y, lerpPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(origPosition.x, transform.position.y, origPosition.z);
        MoveStatus = MoveCode.STATIONARY;
        moveCoroutine = null;
    }

    private void HaltPlayer() {
        StopCoroutine(moveCoroutine);
        transform.position = new Vector3(origPosition.x, transform.position.y, origPosition.z);
        MoveStatus = MoveCode.STATIONARY;
        moveCoroutine = null;
    }
}

public enum MoveCode {
        STATIONARY, MOVING, REBOUNDING, FORCED
}