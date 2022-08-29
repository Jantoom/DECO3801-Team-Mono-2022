using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private float timeToMove, timeToRebound;
    private int playerNumber;
    private Vector3 origPosition, targetPosition;
    private MoveCode moveStatus = MoveCode.STATIONARY;
    private IEnumerator moveCoroutine;
    private int REBOUND_LAYER;
    public MoveCode MoveStatus { get => moveStatus; }

    internal void SetPlayerNumber(int playerNum)
    {
        playerNumber = playerNum;
    }
    void Awake()
    {
        REBOUND_LAYER = LayerMask.NameToLayer("Wall");
    }

    void Update()
    {
        if (playerNumber.Equals(1)) { 
            if (moveStatus.Equals(MoveCode.STATIONARY)) {
                Vector3 direction;
                if (Input.GetKeyDown(KeyCode.W)) {
                    direction = Vector3.forward;
                }
                else if (Input.GetKeyDown(KeyCode.A)) {
                    direction = Vector3.left;
                }
                else if (Input.GetKeyDown(KeyCode.S)) {
                    direction = Vector3.back;
                }
                else if (Input.GetKeyDown(KeyCode.D)) {
                    direction = Vector3.right;
                }
                else {
                    direction = Vector3.zero;
                }

                if (direction != Vector3.zero) {
                    moveCoroutine = MovePlayer(direction);
                    StartCoroutine(moveCoroutine);
                }
            }
        } else if (playerNumber.Equals(2)) {
            if (moveStatus.Equals(MoveCode.STATIONARY)) {
                Vector3 direction;
                if (Input.GetKeyDown(KeyCode.UpArrow)) {
                    direction = Vector3.forward;
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                    direction = Vector3.left;
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow)) {
                    direction = Vector3.back;
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                    direction = Vector3.right;
                }
                else {
                    direction = Vector3.zero;
                }

                if (direction != Vector3.zero) {
                    moveCoroutine = MovePlayer(direction);
                    StartCoroutine(moveCoroutine);
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer.Equals(REBOUND_LAYER) && moveStatus.Equals(MoveCode.MOVING)) {
            StopCoroutine(moveCoroutine);
            StartCoroutine(ReboundPlayer());
        }
    }

    private IEnumerator MovePlayer(Vector3 direction) {
        moveStatus = MoveCode.MOVING;
        origPosition = transform.position;
        targetPosition = origPosition + direction;
        
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