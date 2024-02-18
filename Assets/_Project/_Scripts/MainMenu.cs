using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenu;

    [SerializeField] private PresetSpawnPositions[] spawnPositions;

    public void StartGame()
    {
        ResetSOs();
        AudioManager.Instance.PlaySong(AudioManager.Songs.HaroldsPerplection);
        SceneManager.LoadScene(2);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenSettingsMenu()
    {
        settingsMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    private void ResetSOs() {
        foreach (var SO in spawnPositions) {
            SO.levelData.respawnLocation = SO.originalSpawnpoint;
            SO.levelData.cameraIndex = SO.originalCamera;
            SO.levelData.respawnWithDoor = false;
        }
    }
}

[Serializable]
public class PresetSpawnPositions {
    public LevelDataScriptableObject levelData;
    public Vector3 originalSpawnpoint;
    public int originalCamera;
}
