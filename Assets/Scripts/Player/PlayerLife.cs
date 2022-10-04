using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    float speed = 1.0f;
    //float GENERATION_DELAY = 3.0f;
    // Start is called before the first frame update
   /* void Start()
    {
        //Start the coroutine we define below named ExampleCoroutine.
        StartCoroutine(ExampleCoroutine());
    }

    IEnumerator ExampleCoroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(GENERATION_DELAY);
        MoveObj();

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }*/

    // Update is called once per frame
     void Update()
    {
        MoveObj();

    }
    void MoveObj() {
        var move = new Vector3(0, 0, 1);
        transform.position = transform.position + (move * speed * Time.deltaTime);
    }
}
