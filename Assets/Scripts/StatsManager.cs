using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;

    private int totalPermUpgradeIDs = 12;

    [Header("Default Combat Stats")]
    public float defaultSwordDamage = 20f;
    public float defaultSwordAttackSpeed = 1f;
    public float defaultBowDamage;
    public float defaultBowAttackSpeed;

    [Header("Default Bow Upgrade Stats")]
    public int defaultBowArrowsPerShot = 1;
    public int defaultBowPierceCount = 0;
    public float defaultBowAngleBetweenArrows = 0f;
    public float defaultBowMultishotCooldown = 3f;

    [Header("Default Flare Upgrade Stats")]
    public float defaultFlareRadiusBonus = 0f;
    public float defaultBlueFlareCooldown = 20f;
    public float defaultBlueFlareSlowMultiplier = 0.85f;
    public float defaultBlueFlareDamagePerTick = 1f;
    public float defaultBlueFlareTickRate = 1f;

    [Header("Default Player Stats")]
    public float defaultMoveSpeed;
    public int defaultMaxHealth = 100;
    public int defaultHpRegen = 0;
    public float defaultXpNeeded = 10f;
    public float defaultLevel = 1f;

    [Header("Sword Stats")]
    public float swordDamage;
    public bool swordUpgradeUnlocked = false;
    public float swordAttackSpeed;
    public float bowDamage;
    public float bowAttackSpeed;

    [Header("Bow Upgrade Stats")]
    public int bowArrowsPerShot = 1;
    public int bowPierceCount = 0;
    public bool bowMultishotUnlocked = false;
    public float bowAngleBetweenArrows = 0f;
    public float bowMultishotCooldown = 3f;

    [Header("Flare Upgrade Stats")]
    public float flareRadiusBonus = 0f;
    public bool blueFlareUnlocked = false;
    public float blueFlareCooldown = 20f;
    public float blueFlareSlowMultiplier = 0.85f;
    public float blueFlareDamagePerTick = 1f;
    public float blueFlareTickRate = 1f;

    [Header("Player Stats")]
    public float moveSpeed;
    public int maxHealth;
    public int curHealth;
    public int hpRegen;

    [Header("Misc")]
    public float xp;
    public float xpNeeded; //for level up
    public float level;
    public float totalKilled;
    public int currency = 9999;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        ResetForNewRun();
    }

    public void ResetForNewRun()
    {

        // combat
        swordDamage = defaultSwordDamage;
        swordAttackSpeed = defaultSwordAttackSpeed;
        bowDamage = defaultBowDamage;
        bowAttackSpeed = defaultBowAttackSpeed;

        // bow upgrades
        bowArrowsPerShot = defaultBowArrowsPerShot;
        bowPierceCount = defaultBowPierceCount;
        bowAngleBetweenArrows = defaultBowAngleBetweenArrows;
        bowMultishotCooldown = defaultBowMultishotCooldown;
        bowMultishotUnlocked = false;

        // flare upgrades
        flareRadiusBonus = defaultFlareRadiusBonus;
        blueFlareCooldown = defaultBlueFlareCooldown;
        blueFlareSlowMultiplier = defaultBlueFlareSlowMultiplier;
        blueFlareDamagePerTick = defaultBlueFlareDamagePerTick;
        blueFlareTickRate = defaultBlueFlareTickRate;
        blueFlareUnlocked = false;

        // sword upgrades
        swordUpgradeUnlocked = false;

        // player
        moveSpeed = defaultMoveSpeed;
        maxHealth = defaultMaxHealth;
        curHealth = maxHealth;
        hpRegen = defaultHpRegen;

        for (int i = 0; i <= totalPermUpgradeIDs; i++)
        {
            if (PlayerPrefs.GetInt(i.ToString(), 0) == 1)
            {
                if (i <= 4){
                    maxHealth += 2;
                } else if (i <= 9)
                {
                    hpRegen += 1;
                } else if (i == 10)
                {
                    swordUpgradeUnlocked = true;
                } else if (i == 11)
                {
                    bowMultishotUnlocked = true;
                    bowArrowsPerShot = 3;
                    bowAngleBetweenArrows += 2f;
                } else if(i == 12)
                {
                    blueFlareUnlocked = true;
                }
            }
        }

        // misc
        xp = 0f;
        xpNeeded = defaultXpNeeded; //for level up
        level = defaultLevel;
        totalKilled = 0f;
    }
}