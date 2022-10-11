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
    private int lastSignal;
    // Key Controls from Keyboard
    [field: SerializeField] public KeyCode ForwardKey { get; private set; }
    [field: SerializeField] public KeyCode BackKey { get; private set; }
    [field: SerializeField] public KeyCode LeftKey { get; private set; }
    [field: SerializeField] public KeyCode RightKey { get; private set; }
    private KeyCode lastKeyCode;
    // Player Cooldowns
    public static float ITEM_WAIT_TIME = 0.35f;
    private float timeSinceLastInput = 0.0f, timeToMove = 0.1f, timeToRebound = 0.25f;
    public float TimeToMove { get => timeToMove * (playerInfo.Exhausted ? 10 : 1); }
    public float TimeToRebound { get => timeToRebound * (playerInfo.Exhausted ? 10 : 1); }
    // Moving
    public MoveCode MoveStatus { get; private set; } = MoveCode.STATIONARY;
    private Vector3 origPosition, targetPosition;
    private IEnumerator moveCoroutine = null;

    void Awake()
    {
        playerInfo = GetComponent<PlayerInfo>();
        // Opens port and sets a timeout between reads
        if (UseSerialControls) {
            serialInput.Open();
            serialInput.ReadTimeout = 1;
        }
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
        if (serialInput.BytesToRead > 0) {
            var signal = serialInput.ReadByte();
            if (MoveStatus == MoveCode.STATIONARY) {
                Vector3 direction = Vector3.zero;
                if (signal == ForwardSignal) {
                    direction = Vector3.forward;
                } else if (signal == LeftSignal) {
                    direction = Vector3.left;
                } else if (signal == RightSignal) {
                    direction = Vector3.right;
                } else if (signal == BackSignal) {
                    direction = Vector3.back;
                }
                if (direction != Vector3.zero) {
                    moveCoroutine = MovePlayer(direction, false);
                    StartCoroutine(moveCoroutine);
                }
            }
            if (((signal == LeftSignal && lastSignal == RightSignal) ||
                (signal == RightSignal && lastSignal == LeftSignal)) &&
                Time.time - timeSinceLastInput < ITEM_WAIT_TIME) {
                // Item Activation
                var powerup = playerInfo.LoadedPowerup;
                if (powerup != null) powerup.Activate();

                if (moveCoroutine != null) {
                    StopCoroutine(moveCoroutine);
                    StartCoroutine(ReboundPlayer());
                }
            }
            lastSignal = signal;
            timeSinceLastInput = Time.time;
        }
    }

    private void ProcessKeyboardInput() {
        var downKey = KeyCode.None;
        if (MoveStatus == MoveCode.STATIONARY) {
            Vector3 direction = Vector3.zero;
            if (Input.GetKeyDown(ForwardKey)) {
                downKey = ForwardKey;
                direction = Vector3.forward;
            } else if (Input.GetKeyDown(LeftKey)) {
                downKey = LeftKey;
                direction = Vector3.left;
            } else if (Input.GetKeyDown(RightKey)) {
                downKey = RightKey;
                direction = Vector3.right;
            } else if (Input.GetKeyDown(BackKey)) {
                downKey = BackKey;
                direction = Vector3.back;
            }
            if (direction != Vector3.zero) {
                moveCoroutine = MovePlayer(direction, false);
                StartCoroutine(moveCoroutine);
            }
        }
        if (((Input.GetKeyDown(LeftKey) && Input.GetKeyDown(RightKey)) ||
            (Input.GetKeyDown(LeftKey) && lastKeyCode == RightKey) ||
            (Input.GetKeyDown(RightKey) && lastKeyCode == LeftKey)) &&
            Time.time - timeSinceLastInput < ITEM_WAIT_TIME) {
            // Item Activation
            var powerup = playerInfo.LoadedPowerup;
            if (powerup != null) powerup.Activate();

            if (moveCoroutine != null) {
                StopCoroutine(moveCoroutine);
                StartCoroutine(ReboundPlayer());
            }
        }
        if (Input.GetKeyUp(ForwardKey) || Input.GetKeyUp(LeftKey) || Input.GetKeyUp(RightKey)) {
            lastKeyCode = KeyCode.None;
        } else if (downKey != KeyCode.None) {
            lastKeyCode = downKey;
        }
        timeSinceLastInput = Time.time;        
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
                    var overlaps = Physics.OverlapSphere(transform.position + direction * range + Vector3.down * 0.5f, 0.4f);
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
        targetPosition = new Vector3(Mathf.Round(targetPosition.x), Mathf.Round(targetPosition.y), Mathf.Round(targetPosition.z));
        transform.LookAt(targetPosition);

        float elapsedTime = 0f;
        while (elapsedTime < timeToMove) {
            transform.position = Vector3.Lerp(origPosition, targetPosition, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        MoveStatus = MoveCode.STATIONARY;
    }

    private IEnumerator ReboundPlayer() {
        MoveStatus = MoveCode.REBOUNDING;
        var reboundPosition = transform.position;

        float elapsedTime = 0f;
        while (elapsedTime < timeToRebound) {
            transform.position = Vector3.Lerp(reboundPosition, origPosition, (elapsedTime / timeToRebound));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = origPosition;
        MoveStatus = MoveCode.STATIONARY;
        moveCoroutine = null;
    }   
}

public enum MoveCode {
        STATIONARY, MOVING, REBOUNDING, FORCED
}