using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMenuMusic : MonoBehaviour
{
    private static BGMenuMusic bgAudioInstance = null;
    private bool isMusicDetroyed = false;
    public static BGMenuMusic BGAudioInstance { get { return bgAudioInstance; } }

    void Awake()
    {
        //Debug.Log("Awake:" + SceneManager.GetActiveScene().name);
        if (bgAudioInstance != null && bgAudioInstance != this) {
            //not in scene 1
            Destroy(bgAudioInstance);
            return;
        } else {
            //in scene 1
            bgAudioInstance = this;

        }
        DontDestroyOnLoad(this.gameObject);
    }
    private void Update()
    {
        if (!isMusicDetroyed)
        {
            DestroyMusic();
        }
    }
    //
    // Summary:
    //     Destory the music when the active scene is not the menu pages; main menu or context menu.
    //
    private void DestroyMusic()
    {
        if (SceneManager.GetActiveScene().name != "Context Menu" &&
            SceneManager.GetActiveScene().name != "Main Menu") {
            Destroy(this.gameObject);
            isMusicDetroyed = true;
            return;
        }
    }
}
