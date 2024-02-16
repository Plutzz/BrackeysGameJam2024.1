using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager> 
{
    [SerializeField] private LevelDataScriptableObject levelData;

    [SerializeField] private GameObject player;

    private void OnEnable()
    {
        player.transform.position = levelData.respawnLocation;
    }

    public void ResetLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetRespawnLocation(Vector3 newRespawnLocation) {
        levelData.respawnLocation = newRespawnLocation;
    }
    

}
