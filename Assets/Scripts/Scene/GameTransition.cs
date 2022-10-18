using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTransition : MonoBehaviour
{
    [field: SerializeField] public string TransitionScene { get; private set; } = "";
    [field: SerializeField] public float InitialWaitDelay { get; private set; } = 0.0f;
    private bool _readyToTransition = false;

    void Awake()
    {
        if (PlayerControls.UseSerialControls && !PlayerControls.SerialInputOpen) {
            PlayerControls.SerialInput.Open();
            PlayerControls.SerialInput.ReadTimeout = 1;
            PlayerControls.SerialInputOpen = true;
        }
    }
    IEnumerator Start()
    {
        yield return new WaitForSeconds(InitialWaitDelay);
        _readyToTransition = true;
    }
    void Update()
    {
        if (_readyToTransition) {
            if (PlayerControls.UseSerialControls) {
                if (PlayerControls.SerialInput.BytesToRead > 0) {
                    PlayerControls.SerialInput.ReadExisting();
                    GameObject.Find("AudioManager").GetComponent<AudioManager>().OnSceneTransition();
                    SceneManager.LoadScene(TransitionScene);
                }
            } else if (Input.GetKeyDown(KeyCode.Space)) {
                GameObject.Find("AudioManager").GetComponent<AudioManager>().OnSceneTransition();
                SceneManager.LoadScene(TransitionScene);
            }
        }
    }
}
