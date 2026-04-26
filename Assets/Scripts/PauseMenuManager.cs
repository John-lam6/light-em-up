using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject pausePanel;
    public GameObject confirmExitPanel;

    [Header("Scene Names")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private bool isPaused = false;

    void Start()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        if (confirmExitPanel != null)
        {
            confirmExitPanel.SetActive(false);
        }

        // top down game, keep mouse visible
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (confirmExitPanel != null && confirmExitPanel.activeSelf)
            {
                CancelExitRun();
            }
            else if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }

        if (confirmExitPanel != null)
        {
            confirmExitPanel.SetActive(false);
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        if (confirmExitPanel != null)
        {
            confirmExitPanel.SetActive(false);
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void AskExitRun()
    {
        if (confirmExitPanel != null)
        {
            confirmExitPanel.SetActive(true);
        }
    }

    public void CancelExitRun()
    {
        if (confirmExitPanel != null)
        {
            confirmExitPanel.SetActive(false);
        }
    }

    public void ConfirmExitRun()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (StatsManager.Instance != null)
        {
            StatsManager.Instance.ResetForNewRun();
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        PlayerPrefs.Save();
        SceneManager.LoadScene(mainMenuSceneName);
    }
}