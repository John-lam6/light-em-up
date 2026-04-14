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
        upgradePanel.SetActive(false);
    }

    void Update()
    {
        if (StatsManager.Instance.xp >= StatsManager.Instance.xpNeeded) {
            audioSource.volume = 0.6f;
            audioSource.PlayOneShot(audioclip);
            UpgradePlayer();
            StatsManager.Instance.xp -= StatsManager.Instance.xpNeeded;
            StatsManager.Instance.xpNeeded *= 1.5f;
        }
    }
    public void UpgradePlayer()
    {
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

        option.transform.Find("Name").GetComponent<TMP_Text>().text = data.upgradeName;

        TMP_Text descText = option.transform.Find("Description").GetComponent<TMP_Text>();

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

        option.transform.Find("Icon").GetComponent<Image>().sprite = data.icon;

        option.GetComponent<Button>().onClick.AddListener(() =>
        {
            ApplyUpgrade(data);
            upgradePanel.SetActive(false);
            Time.timeScale = 1f;
        });
    }

    void ApplyUpgrade(UpgradeData data)
    {

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
