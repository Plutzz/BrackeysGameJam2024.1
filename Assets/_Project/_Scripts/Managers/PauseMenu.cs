using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject firstSelected;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private AudioMixer audioMixer;

    private void Awake()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SetMusicVolume(musicSlider.value);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        SetSFXVolume(sfxSlider.value);
        pauseMenu.SetActive(false);
    }

    private void OnEnable()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SetMusicVolume(musicSlider.value);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        SetSFXVolume(sfxSlider.value);
    }

    public void OpenPauseMenu() {
        pauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void ResetLevel() {
        Time.timeScale = 1.0f;
        LevelManager.Instance.ScenicResetLevel();
    }

    public void QuitGame() { 
        LevelManager.Instance.ExitGame();
    }

    public void SetMusicVolume(float volume)
    {
        if (volume <= musicSlider.minValue) {
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
}
