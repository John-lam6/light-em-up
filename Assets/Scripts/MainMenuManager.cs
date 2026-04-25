using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string firstLevelSceneName = "SampleScene";
    [SerializeField] private string upgradeSceneName = "UpgradeScene";
    [SerializeField] private LevelLoader levelLoader;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip menuMusic;

    void Start()
    {
        Time.timeScale = 1f;

        // top down game, keep mouse visible
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (audioSource != null && menuMusic != null)
        {
            audioSource.clip = menuMusic;
            audioSource.loop = true;
            audioSource.volume = 0.8f;
            audioSource.Play();
        }
    }

    public void NewRun()
    {
        if (audioSource != null)
            audioSource.Stop();

        if (StatsManager.Instance != null)
        {
            StatsManager.Instance.ResetForNewRun();
        }

        Time.timeScale = 1f;
        if(PlayerPrefs.GetInt("hasBeatTutorial") != 1)
        {
            SceneManager.LoadScene("Tutorial");
        } else
        {
            levelLoader.LoadLevel(1);    
        }
    }

    public void Upgrade()
    {
        int permCurrency = PlayerPrefs.GetInt("PermCurrency", 0);
        PlayerPrefs.SetInt("PermCurrency", permCurrency);
        PlayerPrefs.Save();
        SceneManager.LoadScene(upgradeSceneName);
    }

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}