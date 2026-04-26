using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpawnManager spawnManager;

    [Header("UI Elements")]
    [SerializeField] private GameObject introUI;
    [SerializeField] private GameObject flareUI;
    [SerializeField] private GameObject afterFlareUI;
    [SerializeField] private GameObject waveUI;
    [SerializeField] private GameObject bossUI;
    [SerializeField] private GameObject exitUI;
    private bool hasFlare = false;
    private bool lookingForFlare = false;
    void Start()
    {
        spawnManager.currLevel = 0;

        introUI.SetActive(true);
        flareUI.SetActive(false);
        afterFlareUI.SetActive(false);
        waveUI.SetActive(false);
        bossUI.SetActive(false);
        exitUI.SetActive(false);

        StartCoroutine(IntroPhase());
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            hasFlare = false;
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            hasFlare = true;
        }

        if(lookingForFlare && hasFlare && Input.GetMouseButtonDown(0))
        {
            flareUI.SetActive(false);
            afterFlareUI.SetActive(true);
            lookingForFlare = false;
            StartCoroutine(AfterFlaresPhase());
        }
    }

    public void BeginBossPhase()
    {
        StartCoroutine(BossPhase());
    }
    public void BeginExitPhase()
    {
        StartCoroutine(ExitPhase());
    }

    IEnumerator IntroPhase()
    {
        yield return new WaitForSeconds(5.0f);
        introUI.SetActive(false);
        flareUI.SetActive(true);
        lookingForFlare = true;
    }

    IEnumerator AfterFlaresPhase()
    {
        yield return new WaitForSeconds(5.0f);
        afterFlareUI.SetActive(false);
        waveUI.SetActive(true);
        StartCoroutine(spawnManager.StartTutorialWave());

        yield return new WaitForSeconds(5.0f);
        waveUI.SetActive(false);
    }

    IEnumerator BossPhase()
    {
        bossUI.SetActive(true);

        yield return new WaitForSeconds(5.0f);
        bossUI.SetActive(false);
    }

    IEnumerator ExitPhase()
    {
        exitUI.SetActive(true);

        yield return new WaitForSeconds(5.0f);
        exitUI.SetActive(false);
    }

}
