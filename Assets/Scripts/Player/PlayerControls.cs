using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private PlayerInfo playerInfo;
    // Player Controls
    [SerializeField]
    private KeyCode forwardKey, backKey, leftKey, rightKey, bombKey, itemKey;
    public KeyCode ForwardKey { get => forwardKey; }
    public KeyCode BackKey { get => backKey; }
    public KeyCode LeftKey { get => leftKey; }
    public KeyCode RightKey { get => rightKey; }
    public KeyCode BombKey { get => bombKey; }
    public KeyCode ItemKey { get => itemKey; }
    // Player Cooldowns
    private float timeToMove = 0.1f, timeToRebound = 0.25f;
    public float TimeToMove { get => timeToMove * playerInfo.CooldownMultiplier; }
    public float TimeToRebound { get => timeToRebound * playerInfo.CooldownMultiplier; }
    // Moving
    private int REBOUND_LAYER;
    private Vector3 origPosition, targetPosition;
    private IEnumerator moveCoroutine;
    private MoveCode moveStatus = MoveCode.STATIONARY;
    public MoveCode MoveStatus { get => moveStatus; }

    void Awake()
    {
        playerInfo = GetComponent<PlayerInfo>();
        REBOUND_LAYER = LayerMask.NameToLayer("Wall");
    }

    void Update()
    {
        if (moveStatus == MoveCode.STATIONARY) {
            if (Input.GetKeyDown(bombKey)) {
                Instantiate(playerInfo.BombPrefab, transform.position, transform.rotation);
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
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer.Equals(REBOUND_LAYER) && moveStatus.Equals(MoveCode.MOVING)) {
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