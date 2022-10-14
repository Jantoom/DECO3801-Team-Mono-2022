using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.SceneManagement;



public class MenuNewGame : MonoBehaviour
{
    [SerializeField]
    string gameScene;



    private void Awake()
    {

    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(gameScene);
        }
    }




}
