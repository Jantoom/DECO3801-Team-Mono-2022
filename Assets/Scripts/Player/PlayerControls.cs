using System.Collections;
using System.Collections.Generic;
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
    public static float ITEM_WAIT_TIME = 0.8f;
    private float timeSinceLastInput = 0.0f, timeToMove = 0.1f, timeToRebound = 0.25f;
    public float TimeToMove { get => timeToMove * (playerInfo.Exhausted ? 10 : 1); }
    public float TimeToRebound { get => timeToRebound * (playerInfo.Exhausted ? 10 : 1); }
    // Moving
    public MoveCode MoveStatus { get; private set; } = MoveCode.STATIONARY;
    private int REBOUND_LAYER;
    private Vector3 origPosition, targetPosition;
    private IEnumerator moveCoroutine;

    void Awake()
    {
        playerInfo = GetComponent<PlayerInfo>();
        REBOUND_LAYER = LayerMask.NameToLayer("Wall");
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
                    moveCoroutine = MovePlayer(direction);
                    StartCoroutine(moveCoroutine);
                }
            } else if (((signal == LeftSignal && lastSignal == RightSignal) ||
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
                moveCoroutine = MovePlayer(direction);
                StartCoroutine(moveCoroutine);
            }
        } else if (((Input.GetKeyDown(LeftKey) && lastKeyCode == RightKey) ||
            (Input.GetKeyDown(RightKey) && lastKeyCode == LeftKey)) &&
            Time.time - timeSinceLastInput < ITEM_WAIT_TIME) {
            // Item Activation
            Debug.Log("We not stationary yo");
            var powerup = playerInfo.LoadedPowerup;
            if (powerup != null) powerup.Activate();

            if (moveCoroutine != null) {
                StopCoroutine(moveCoroutine);
                StartCoroutine(ReboundPlayer());
            }
        }
        if (Input.GetKeyUp(ForwardKey) || Input.GetKeyUp(LeftKey) || Input.GetKeyUp(RightKey)) {
            lastKeyCode = KeyCode.None;
        } else {
            lastKeyCode = downKey;
        }
        timeSinceLastInput = Time.time;        
    }
        

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer.Equals(REBOUND_LAYER) && MoveStatus.Equals(MoveCode.MOVING) && !playerInfo.Ghosted) {
            if (!collision.gameObject.TryGetComponent<WallWeak>(out var wall) || wall.Health - playerInfo.Attack > 0) {
                StopCoroutine(moveCoroutine);
                StartCoroutine(ReboundPlayer());
            }
            collision.gameObject.GetComponent<IDestructible>()?.TakeDamage(playerInfo.Attack);
        }
    }

    private IEnumerator MovePlayer(Vector3 direction) {
        MoveStatus = MoveCode.MOVING;
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
        STATIONARY, MOVING, REBOUNDING
}