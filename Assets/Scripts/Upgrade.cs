using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public static Upgrade Instance;
    public UpgradeData[] upgrades;

    //UI varaibles
    public GameObject upgradePanel;
    public Transform optionParent;
    public GameObject upgradeOptionPrefab;

    public AudioSource audioSource;
    public AudioClip audioclip;
    
    void Awake()
    {
        Instance = this;

        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
        }
    }

    void Update()
    {
        if (StatsManager.Instance == null) return;
        if (upgradePanel == null) return;
        if (optionParent == null) return;
        if (upgradeOptionPrefab == null) return;

        if (GameEndManager.Instance != null && GameEndManager.Instance.IsGameEnded())
            return;

        if (Time.timeScale == 0f && !upgradePanel.activeSelf)
            return;

        if (StatsManager.Instance.xp >= StatsManager.Instance.xpNeeded) {
            if (audioSource != null && audioclip != null)
            {
                audioSource.volume = 0.6f;
                audioSource.PlayOneShot(audioclip);
            }

            UpgradePlayer();
            StatsManager.Instance.xp -= StatsManager.Instance.xpNeeded;
            StatsManager.Instance.xpNeeded *= 1.5f;
        }
    }

    public void UpgradePlayer()
    {
        if (upgrades == null || upgrades.Length < 3) return;

        List<UpgradeData> availableUpgrades = new List<UpgradeData>(upgrades);
        List<UpgradeData> selectedUpgrades = new List<UpgradeData>();

        for (int i = 0; i < 3; i++)
        {
            int upgradeIndex = Random.Range(0, availableUpgrades.Count);

            UpgradeData randomUpgrade = availableUpgrades[upgradeIndex];

            if(randomUpgrade.valueRange.Length != 2)
            {
                if (randomUpgrade.value == 0)
                {
                    randomUpgrade.value = 1;
                }
            } 
            else
            {
                if(randomUpgrade.isPercentage || randomUpgrade.isFloat)
                {
                    randomUpgrade.value = Mathf.Round(Random.Range(randomUpgrade.valueRange[0], randomUpgrade.valueRange[1]) * 100f) / 100f;
                } 
                else
                {
                    randomUpgrade.value = Mathf.Round(Random.Range(randomUpgrade.valueRange[0], randomUpgrade.valueRange[1]));
                }
            }
            
            selectedUpgrades.Add(randomUpgrade);
            availableUpgrades.RemoveAt(upgradeIndex);
        }

        upgradePanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        ShowUpgrades(selectedUpgrades);
    }

    void ShowUpgrades(List<UpgradeData> availableUpgrades)
    {
        foreach (Transform child in optionParent)
        {
            Destroy(child.gameObject);
        }

        foreach (UpgradeData upgrade in availableUpgrades)
        {
            Debug.Log(upgrade);
        }

        if (availableUpgrades.Count < 3) return;

        CreateOption(availableUpgrades[0]);
        CreateOption(availableUpgrades[1]);
        CreateOption(availableUpgrades[2]);
    }

    void CreateOption(UpgradeData data)
    {
        GameObject option = Instantiate(upgradeOptionPrefab, optionParent);

        Transform nameT = option.transform.Find("Name");
        Transform descT = option.transform.Find("Description");
        Transform iconT = option.transform.Find("Icon");

        if (nameT == null || descT == null || iconT == null)
        {
            Debug.LogWarning("Upgrade option prefab is missing Name, Description, or Icon child.");
            return;
        }

        TMP_Text nameText = nameT.GetComponent<TMP_Text>();
        TMP_Text descText = descT.GetComponent<TMP_Text>();
        Image iconImage = iconT.GetComponent<Image>();
        Button optionButton = option.GetComponent<Button>();

        if (nameText == null || descText == null || iconImage == null || optionButton == null)
        {
            Debug.LogWarning("Upgrade option prefab is missing TMP_Text, Image, or Button component.");
            return;
        }

        nameText.text = data.upgradeName;

        if (data.id == 8)
        {
            if (StatsManager.Instance != null && !StatsManager.Instance.blueFlareUnlocked)
            {
                descText.text = "Unlock blue flare";
            }
            else
            {
                descText.text = data.description + $" increase by {data.value}";
            }
        }
        else
        {
            descText.text = data.description + $" increase by {data.value}";
        }

        iconImage.sprite = data.icon;

        optionButton.onClick.RemoveAllListeners();
        optionButton.onClick.AddListener(() =>
        {
            ApplyUpgrade(data);
            upgradePanel.SetActive(false);
            Time.timeScale = 1f;
        });
    }

    void ApplyUpgrade(UpgradeData data)
    {
        if (StatsManager.Instance == null) return;

        switch (data.id)
        {
            case 0:
                StatsManager.Instance.maxHealth += data.value;
                StatsManager.Instance.curHealth += data.value;
                break;

            case 1:
                StatsManager.Instance.swordAttackSpeed *= data.value;
                break;

            case 2:
                StatsManager.Instance.moveSpeed += data.value;
                break;

            case 3:
                StatsManager.Instance.swordDamage += data.value;
                break;

            case 4: // bow damage
                StatsManager.Instance.bowDamage += data.value;
                break;

            case 5: // multishot (arrows + spread together)
                StatsManager.Instance.bowArrowsPerShot += Mathf.RoundToInt(data.value);
                StatsManager.Instance.bowArrowsPerShot = Mathf.Max(1, StatsManager.Instance.bowArrowsPerShot);

                if (StatsManager.Instance.bowArrowsPerShot > 1)
                    StatsManager.Instance.bowAngleBetweenArrows += 2f;

                break;

            case 6: // pierce
                StatsManager.Instance.bowPierceCount += Mathf.RoundToInt(data.value);
                StatsManager.Instance.bowPierceCount = Mathf.Max(0, StatsManager.Instance.bowPierceCount);
                break;

            case 7: // flare radius
                StatsManager.Instance.flareRadiusBonus += data.value;
                break;

            case 8: // blue flare unlock first, then increase damage
                if (!StatsManager.Instance.blueFlareUnlocked)
                {
                    StatsManager.Instance.blueFlareUnlocked = true;
                }
                else
                {
                    StatsManager.Instance.blueFlareDamagePerTick += data.value;
                }
                break;
        }
    }
}