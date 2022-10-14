using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMusic : MonoBehaviour
{
    private static BGMusic instance = null;
    public static BGMusic Instance { get { return instance; } }
    // Start is called before the first frame update
    void Awake()
    {
        //Debug.Log("Awake:" + SceneManager.GetActiveScene().name);
        if (instance != null && instance != this)
        {
            //not in scene 1
            Destroy(instance);
            return;
        }
        else
        {
            //in scene 1
            instance = this;
            
        }
        DontDestroyOnLoad(this.gameObject);


    }
    private void Start()
    {
        if (instance != null && instance != this && SceneManager.GetActiveScene().name == "Context Menu")
        {
            Debug.Log("Start:" + SceneManager.GetActiveScene().name);
            //not in scene 1
            Destroy(instance);
            return;
        }

    }
}
