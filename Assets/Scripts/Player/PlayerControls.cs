using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class PlayerControls : MonoBehaviour
{
    // Serial Port Comms from arduino
    
    int movement;

    static SerialPort sp = new SerialPort("COM3", 9600);

    private PlayerInfo playerInfo;
    // Player Controls
    [SerializeField]
    // Change int to KeyCode and enter keycodes 
    private int forwardKey, backKey, leftKey, rightKey, bombKey, itemKey;
    public int ForwardKey { get => forwardKey; }
    public int BackKey { get => backKey; }
    public int LeftKey { get => leftKey; }
    public int RightKey { get => rightKey; }
    public int BombKey { get => bombKey; }
    public int ItemKey { get => itemKey; }
    // Player Cooldowns
    private float timeToMove = 0.1f, timeToRebound = 0.25f;
    public float TimeToMove { get => timeToMove * (playerInfo.Exhausted ? 10 : 1); }
    public float TimeToRebound { get => timeToRebound * (playerInfo.Exhausted ? 10 : 1); }
    // Moving
    private int REBOUND_LAYER;
    private Vector3 origPosition, targetPosition;
    private IEnumerator moveCoroutine;
    private MoveCode moveStatus = MoveCode.STATIONARY;
    public MoveCode MoveStatus { get => moveStatus; }

    private void Start()
    {
        
        // Opens port and sets a timeout between reads
        sp.Open();
        sp.ReadTimeout = 1;
    }
    void Awake()
    {
        playerInfo = GetComponent<PlayerInfo>();
        REBOUND_LAYER = LayerMask.NameToLayer("Wall");
    }

    void Update()
    {
        
        if (playerInfo.Frozen) {
            return;
        }
        // Serial port inputs
        // COMMENT THIS SECTION IF YOU WANT TO PLAY WITH KEYBOARD
        if (sp.IsOpen)
        {
            //print(forwardKey);
            try
            {
                movement = (int) sp.ReadByte();
                print(movement);
                
                if (moveStatus == MoveCode.STATIONARY)
                {
                    Vector3 direction = Vector3.zero;
                    if (movement == forwardKey)
                    {
                        //print("hello");
                        direction = Vector3.forward;
                    } else if (movement == leftKey)
                    {
                        direction = Vector3.left;
                    } else if (movement == rightKey)
                    {
                        direction = Vector3.right;
                    }
                    //print(direction);
                    if (direction != Vector3.zero)
                    {
                        moveCoroutine = MovePlayer(direction);
                        StartCoroutine(moveCoroutine);
                    }
                }
            }
            catch (System.Exception)
            {

            }
        }
        

        // UNCOMMENT THIS IF YOU WANT TO PLAY WITH KEYBOARD
        // also have to change int to keycode in keycodes and assign codes
        /*
        // Keyboard input
        if (moveStatus == MoveCode.STATIONARY) {
            if (Input.GetKeyDown(bombKey)) {
                Instantiate(playerInfo.BombPrefab, transform.position, transform.rotation);
            }
            else if (Input.GetKeyDown(itemKey)) {
                var powerup = playerInfo.LoadedPowerup;
                if (powerup != null) powerup.Activate();
            }
            Vector3 direction = Vector3.zero;
            if (Input.GetKeyDown(forwardKey)) {
                direction = Vector3.forward;
            }
            else if (Input.GetKeyDown(leftKey)) {
                direction = Vector3.left;
            }
            else if (Input.GetKeyDown(backKey)) {
                direction = Vector3.back;
            }
            else if (Input.GetKeyDown(rightKey)) {
                direction = Vector3.right;
            }
            if (direction != Vector3.zero) {
                moveCoroutine = MovePlayer(direction);
                StartCoroutine(moveCoroutine);
            }
        }*/
    }
        

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer.Equals(REBOUND_LAYER) && moveStatus.Equals(MoveCode.MOVING) && !playerInfo.Ghosted) {
            if (!collision.gameObject.TryGetComponent<WallWeak>(out var wall) || wall.Health - playerInfo.Attack > 0) {
                StopCoroutine(moveCoroutine);
                StartCoroutine(ReboundPlayer());
            }
            collision.gameObject.GetComponent<IDestructible>()?.TakeDamage(playerInfo.Attack);
        }
    }

    private IEnumerator MovePlayer(Vector3 direction) {
        moveStatus = MoveCode.MOVING;
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
        moveStatus = MoveCode.STATIONARY;
    }

    private IEnumerator ReboundPlayer() {
        moveStatus = MoveCode.REBOUNDING;
        var reboundPosition = transform.position;

        float elapsedTime = 0f;
        while (elapsedTime < timeToRebound) {
            transform.position = Vector3.Lerp(reboundPosition, origPosition, (elapsedTime / timeToRebound));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = origPosition;
        moveStatus = MoveCode.STATIONARY;
    }   
}

public enum MoveCode {
        STATIONARY, MOVING, REBOUNDING
}