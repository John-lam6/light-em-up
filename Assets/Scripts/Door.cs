using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour {
    public Light doorLight;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private LevelLoader levelLoader;
    // Start is called before the first frame update
    void Start()
    {
        doorLight = GetComponentInChildren<Light>(true);
    }

    public void turnOnLight() {
        doorLight.enabled = true;
    }

    public void turnOffLight() {
        doorLight.enabled = false;
    }

    public void LoadNextLevel()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName.StartsWith("Level 3."))
        {
            SceneManager.LoadScene("VictoryScreen");
            return;
        }

        spawnManager.currLevel++;
        if(spawnManager.currLevel == 1)
        {
            PlayerPrefs.SetInt("hasBeatTutorial", 1);
        }
        PlayerPrefs.Save();
        levelLoader.LoadLevel(spawnManager.currLevel);
        StatsManager.Instance.HealAfterLevel();
    } 
}
