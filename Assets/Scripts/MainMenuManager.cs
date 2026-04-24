using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string firstLevelSceneName = "SampleScene";
    [SerializeField] private string upgradeSceneName = "UpgradeScene";
    [SerializeField] private LevelLoader levelLoader;

    void Start()
    {
        Time.timeScale = 1f;

        // top down game, keep mouse visible
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void NewRun()
    {
        if (StatsManager.Instance != null)
        {
            StatsManager.Instance.ResetForNewRun();
        }

        Time.timeScale = 1f;
        levelLoader.LoadLevel(1);
    }

    public void Upgrade()
    {
        int permCurrency = PlayerPrefs.GetInt("PermCurrency", 0);
        PlayerPrefs.DeleteAll();
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