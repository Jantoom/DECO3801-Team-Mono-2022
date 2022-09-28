using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void exitGame()
    {    
        #if UNITY_EDITOR
        /*This one works in unity editor (for testing purposes)*/
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        /*In real game outside of unity editor*/
            Application.Quit();
    }
}
