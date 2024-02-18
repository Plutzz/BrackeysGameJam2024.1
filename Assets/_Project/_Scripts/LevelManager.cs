using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.UI;

public class LevelManager : Singleton<LevelManager> 
{
    [SerializeField] private LevelDataScriptableObject levelData;
    [SerializeField] private CinemachineVirtualCamera[] virtualCameras;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject door;

    [SerializeField] private Image overlay; //temp
    [SerializeField, Min(.01f)] private float overlayFadeTime;

    [SerializeField] private GlitchController glitchEffect;

    public event Action reloadingScene;

    private void OnEnable()
    {
        player.transform.position = levelData.respawnLocation;
        if (levelData.respawnWithDoor)
        {
            door.transform.position = levelData.respawnLocation;
        }

        foreach (var cam in virtualCameras)
        {
            cam.enabled = false;
        }
        virtualCameras[levelData.cameraIndex].enabled = true;
    }

    public void ResetLevel() {

        reloadingScene?.Invoke();
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(string sceneName)
    {
        reloadingScene?.Invoke();

        SceneManager.LoadScene(sceneName);
    }

    public void SetRespawnLocation(Vector3 newRespawnLocation, CinemachineVirtualCamera activeCamera, bool respawnWithDoor) {
        levelData.respawnLocation = newRespawnLocation;
        levelData.respawnWithDoor = respawnWithDoor;
        for (int i = 0; i < virtualCameras.Length; i++) { 
            if (virtualCameras[i] == activeCamera)
            {
                levelData.cameraIndex = i;
                break;
            }
        }
    }

    public void ScenicResetLevel() {
        StartCoroutine(FadeInOverlay());
        StartCoroutine(GlitchOverlay());
    }

    IEnumerator FadeInOverlay() {

        yield return new WaitForSeconds(0.5f);

        Color color = overlay.color;
        color.a = 0f; // Start with alpha 0

        float timer = 0f;
        while (timer < overlayFadeTime)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, timer / overlayFadeTime); // Lerp from 0 to 1 over time
            overlay.color = color;
            yield return null;
        }

        // Ensure alpha is exactly 1 at the end
        color.a = 1f;
        overlay.color = color;
        ResetLevel();
    }

    IEnumerator GlitchOverlay()
    {
        glitchEffect.GetComponent<Animation>().Play();
        yield return new WaitForSeconds(glitchEffect.GetComponent<Animation>().clip.length);
        //ResetLevel();
    }

    public void ExitGame()
    {
        Application.Quit();
    }


}
