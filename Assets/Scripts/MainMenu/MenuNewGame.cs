using UnityEngine;
using UnityEngine.SceneManagement;



public class MenuNewGame : MonoBehaviour
{
    [SerializeField]
    string gameScene;



    private void Awake()
    {
        //
        if (PlayerControls.UseSerialControls && !PlayerControls.SerialInputOpen)
        {
            PlayerControls.SerialInput.Open();
            PlayerControls.SerialInput.ReadTimeout = 1;
            PlayerControls.SerialInputOpen = true;
        }
        //
    }
    private void Update()
    {
        if (PlayerControls.UseSerialControls)
        {
            if (PlayerControls.SerialInput.BytesToRead > 0)
            {
                // PlayerControls.SerialInput.ReadByte();
                PlayerControls.SerialInput.ReadExisting();
                SceneManager.LoadScene(gameScene);

            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(gameScene);
        }

    }




}
