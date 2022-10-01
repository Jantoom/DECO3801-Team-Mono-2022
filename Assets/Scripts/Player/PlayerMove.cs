using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private float timeToMove, timeToRebound;
    private PlayerInfo playerInfo;
    private Vector3 origPosition, targetPosition;
    private MoveCode moveStatus = MoveCode.STATIONARY;
    private IEnumerator moveCoroutine;
    private int REBOUND_LAYER;
    public MoveCode MoveStatus { get => moveStatus; }

    void Awake()
    {
        REBOUND_LAYER = LayerMask.NameToLayer("Wall");
        playerInfo = GetComponent<PlayerInfo>();
    }

    void Update()
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetKeyDown(playerInfo.ForwardKey)) {
            direction = Vector3.forward;
        }
        else if (Input.GetKeyDown(playerInfo.LeftKey)) {
            direction = Vector3.left;
        }
        else if (Input.GetKeyDown(playerInfo.BackKey)) {
            direction = Vector3.back;
        }
        else if (Input.GetKeyDown(playerInfo.RightKey)) {
            direction = Vector3.right;
        }

        if (direction != Vector3.zero && moveStatus == MoveCode.STATIONARY) {
            moveCoroutine = MovePlayer(direction);
            StartCoroutine(moveCoroutine);
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
        targetPosition = new Vector3(Mathf.Round(targetPosition.x), Mathf.Round(targetPosition.y), Mathf.Round(targetPosition.z));
        
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