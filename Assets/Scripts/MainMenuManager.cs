using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string firstLevelSceneName = "SampleScene";
    [SerializeField] private string upgradeSceneName = "UpgradeScene";

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
        SceneManager.LoadScene(firstLevelSceneName);
    }

    public void Upgrade()
    {
        PlayerPrefs.DeleteAll();
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