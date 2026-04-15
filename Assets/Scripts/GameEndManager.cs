using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameEndManager : MonoBehaviour
{
    public static GameEndManager Instance;

    [Header("UI Panels")]
    public GameObject victoryPanel;
    public GameObject deathPanel;

    [Header("Death Overlay")]
    public Image deathOverlay;
    public float deathOverlayPulseAlpha = 0.45f;
    public float deathOverlayPulseTime = 0.25f;

    [Header("Scene Names")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string firstLevelSceneName = "SampleScene";

    private bool gameEnded = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        if (deathPanel != null)
            deathPanel.SetActive(false);

        if (deathOverlay != null)
        {
            deathOverlay.DOKill();
            deathOverlay.color = new Color(1f, 0f, 0f, 0f);
        }
    }

    public void ShowVictory()
    {
        if (gameEnded) return;
        gameEnded = true;

        Time.timeScale = 0f;

        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        if (deathPanel != null)
            deathPanel.SetActive(false);

        if (deathOverlay != null)
        {
            deathOverlay.DOKill();
            deathOverlay.color = new Color(1f, 0f, 0f, 0f);
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ShowDeath()
    {
        if (gameEnded) return;
        gameEnded = true;

        if (deathOverlay != null)
        {
            deathOverlay.DOKill();
            deathOverlay.color = new Color(1f, 0f, 0f, 0f);

            // heartbeat pulse effect
            deathOverlay.DOFade(deathOverlayPulseAlpha, deathOverlayPulseTime)
                .SetEase(Ease.InOutSine)
                .SetLoops(3, LoopType.Yoyo)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    if (deathOverlay != null)
                    {
                        deathOverlay.DOFade(0.4f, 0.20f)
                            .SetEase(Ease.OutSine)
                            .SetUpdate(true);
                    }
                });
        }

        if (deathPanel != null)
            deathPanel.SetActive(true);

        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        if (deathOverlay != null)
        {
            deathOverlay.DOKill();
            deathOverlay.color = new Color(1f, 0f, 0f, 0f);
        }

        if (StatsManager.Instance != null)
            StatsManager.Instance.ResetForNewRun();

        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void RestartRun()
    {
        Time.timeScale = 1f;

        if (deathOverlay != null)
        {
            deathOverlay.DOKill();
            deathOverlay.color = new Color(1f, 0f, 0f, 0f);
        }

        if (StatsManager.Instance != null)
            StatsManager.Instance.ResetForNewRun();

        SceneManager.LoadScene(firstLevelSceneName);
    }

    private void OnDestroy()
    {
        if (deathOverlay != null)
            deathOverlay.DOKill();
    }

    public bool IsGameEnded()
    {
        return gameEnded;
    }
    
}