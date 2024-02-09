using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : Singleton<PauseManager>
{
    //TODO distinguish between in game pauses for dialogue and player controlled pauses


    public static bool isPaused;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { 
            TogglePause();
        }    
    }

    public void TogglePause() {
        if (isPaused)
        {
            UnPause();
        }
        else { 
            Pause();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        isPaused = true;
    }

    public void UnPause() { 
        Time.timeScale = 1;
        isPaused = false;
    }

}
