using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float timeToMove, timeToRebound;
    private Vector3 origPosition, targetPosition;
    private MoveStatus moveStatus = MoveStatus.STATIONARY;
    private IEnumerator moveCoroutine;
    private int REBOUND_LAYER;

    void Awake()
    {
        REBOUND_LAYER = LayerMask.NameToLayer("Wall");
    }

    void Update()
    {
        if (moveStatus.Equals(MoveStatus.STATIONARY)) {
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
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer.Equals(REBOUND_LAYER) && moveStatus.Equals(MoveStatus.MOVING)) {
            StopCoroutine(moveCoroutine);
            StartCoroutine(ReboundPlayer());
        }
    }

    private IEnumerator MovePlayer(Vector3 direction) {
        moveStatus = MoveStatus.MOVING;
        origPosition = transform.position;
        targetPosition = origPosition + direction;
        
        float elapsedTime = 0f;
        while (elapsedTime < timeToMove) {
            transform.position = Vector3.Lerp(origPosition, targetPosition, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        moveStatus = MoveStatus.STATIONARY;
    }

    private IEnumerator ReboundPlayer() {
        moveStatus = MoveStatus.REBOUNDING;
        var reboundPosition = transform.position;

        float elapsedTime = 0f;
        while (elapsedTime < timeToRebound) {
            transform.position = Vector3.Lerp(reboundPosition, origPosition, (elapsedTime / timeToRebound));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = origPosition;
        moveStatus = MoveStatus.STATIONARY;
    }

    private enum MoveStatus {
        STATIONARY, MOVING, REBOUNDING
    }
}
