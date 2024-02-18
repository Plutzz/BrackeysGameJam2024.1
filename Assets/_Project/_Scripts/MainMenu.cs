using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenu;

    [SerializeField] private PresetSpawnPositions[] spawnPositions;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private AudioMixer audioMixer;

    private void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SetMusicVolume(musicSlider.value);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        SetSFXVolume(sfxSlider.value);

        ResetSOs();
    }

    public void StartGame(string firstLevel)
    {
        ResetSOs();
        AudioManager.Instance?.PlaySong(AudioManager.Songs.HaroldsPerplection);
        SceneManager.LoadScene(firstLevel);
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





    public void SetMusicVolume(float volume)
    {
        if (volume <= musicSlider.minValue)
        {
            volume = -100;
        }

        audioMixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    // Adjust the SFX volume
    public void SetSFXVolume(float volume)
    {
        if (volume <= sfxSlider.minValue)
        {
            volume = -100;
        }

        audioMixer.SetFloat("SFXVolume", volume);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    private void ResetSOs() {
        foreach (var SO in spawnPositions) {
            SO.levelData.respawnLocation = SO.originalSpawnpoint;
            SO.levelData.respawnDoorLocation = SO.originalDoorSpawnpoint;
            SO.levelData.cameraIndex = SO.originalCamera;
            
        }
    }
}

[Serializable]
public class PresetSpawnPositions {
    public LevelDataScriptableObject levelData;
    public Vector3 originalSpawnpoint;
    public Vector3 originalDoorSpawnpoint;
    public int originalCamera;
}
