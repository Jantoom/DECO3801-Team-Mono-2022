using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContextMenu : MonoBehaviour
{
    [SerializeField] string gameScene;
    private bool readyToGo;

    private void Awake()
    {
        if (PlayerControls.UseSerialControls && !PlayerControls.SerialInputOpen) {
            PlayerControls.SerialInput.Open();
            PlayerControls.SerialInput.ReadTimeout = 1;
            PlayerControls.SerialInputOpen = true;
        }
    }
    private void Start()
    {
        readyToGo = false;
        StartCoroutine(WaitSec());
    }
    private void Update()
    {
        if (readyToGo == true) {
            if (PlayerControls.UseSerialControls) {
                if (PlayerControls.SerialInput.BytesToRead > 0) {
                    PlayerControls.SerialInput.ReadExisting();
                    SceneManager.LoadScene(gameScene);
                }
            } else if (Input.GetKeyDown(KeyCode.Space)) {
                SceneManager.LoadScene(gameScene);
            }
        }
    }
    IEnumerator WaitSec()
    {
        yield return new WaitForSeconds(3);
        readyToGo = true;
    }
}
