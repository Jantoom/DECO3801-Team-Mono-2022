using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Move : MonoBehaviour
{
    [SerializeField]
    private float timeToMove, timeToRebound;
    private Vector3 origPosition, targetPosition;
    private MoveCode moveStatus = MoveCode.STATIONARY;
    private IEnumerator moveCoroutine;
    private int REBOUND_LAYER;
    public MoveCode MoveStatus { get => moveStatus; }

    void Awake()
    {
        REBOUND_LAYER = LayerMask.NameToLayer("Wall");
    }

    void Update()
    {
        if (moveStatus.Equals(MoveCode.STATIONARY))
        {
            Vector3 direction;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                direction = Vector3.forward;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                direction = Vector3.left;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                direction = Vector3.back;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                direction = Vector3.right;
            }
            else
            {
                direction = Vector3.zero;
            }

            if (direction != Vector3.zero)
            {
                moveCoroutine = MovePlayer(direction);
                StartCoroutine(moveCoroutine);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer.Equals(REBOUND_LAYER) && moveStatus.Equals(MoveCode.MOVING))
        {
            StopCoroutine(moveCoroutine);
            StartCoroutine(ReboundPlayer());
        }
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        moveStatus = MoveCode.MOVING;
        origPosition = transform.position;
        targetPosition = origPosition + direction;

        float elapsedTime = 0f;
        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(origPosition, targetPosition, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        moveStatus = MoveCode.STATIONARY;
    }

    private IEnumerator ReboundPlayer()
    {
        moveStatus = MoveCode.REBOUNDING;
        var reboundPosition = transform.position;

        float elapsedTime = 0f;
        while (elapsedTime < timeToRebound)
        {
            transform.position = Vector3.Lerp(reboundPosition, origPosition, (elapsedTime / timeToRebound));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = origPosition;
        moveStatus = MoveCode.STATIONARY;
    }
}

