using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContextMenu : MonoBehaviour
{
    [SerializeField]
    string gameScene;
    private bool readyToGo;
    private void Start()
    {
        readyToGo = false;
        StartCoroutine(WaitSec());
    }
    private void Update()
    {
        if (readyToGo == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(gameScene);
            }
        }

    }
    IEnumerator WaitSec()
    {
        yield return new WaitForSeconds(5);
        readyToGo = true;

    }
}
