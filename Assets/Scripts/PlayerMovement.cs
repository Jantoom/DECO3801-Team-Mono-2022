using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float timeToMove = 0.2f;
    private Vector3 origPosition, targetPosition;
    private bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && !isMoving)
            StartCoroutine(MovePlayer(Vector3.forward));
        if (Input.GetKeyDown(KeyCode.A) && !isMoving)
            StartCoroutine(MovePlayer(Vector3.left));
        if (Input.GetKeyDown(KeyCode.S) && !isMoving)
            StartCoroutine(MovePlayer(Vector3.back));
        if (Input.GetKeyDown(KeyCode.D) && !isMoving)
            StartCoroutine(MovePlayer(Vector3.right));
    }

    private IEnumerator MovePlayer(Vector3 direction) {
        // isMoving is set to true at the start of the function and back to false
        // at the end so the character cannot move in a different direction until
        // this movement is finished
        isMoving = true;

        float elapsedTime = 0;

        origPosition = transform.position;
        targetPosition = origPosition + direction;

        while (elapsedTime < timeToMove) {
            transform.position = Vector3.Lerp(origPosition, targetPosition, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        isMoving = false;
    }
}
