using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : Singleton<PauseManager>
{
    //TODO distinguish between in game pauses for dialogue and player controlled pauses
    public bool isPaused;
    public GameObject PauseMenu;


    private void Start()
    {
        InputHandler.Instance.playerInputActions.Universal.Pause.performed += TogglePause;
    }

    public void TogglePause(InputAction.CallbackContext context)
    {
        if (isPaused)
        {
            UnPause();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        PauseMenu.SetActive(true);

        // Relinquishes player input and enables UI input
        InputHandler.Instance.playerInputActions.Player.Disable();
        InputHandler.Instance.playerInputActions.UI.Enable();


        isPaused = true;
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);

        // Relinquishes UI input and enables player input
        InputHandler.Instance.playerInputActions.Player.Enable();
        InputHandler.Instance.playerInputActions.UI.Disable();

        isPaused = false;
    }

}
