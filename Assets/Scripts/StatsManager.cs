using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;

    private int totalPermUpgradeIDs = 13;

    [Header("Default Combat Stats")]
    private float defaultSwordDamage = 15f;
    public float defaultSwordAttackSpeed = 1f;
    private float defaultBowDamage = 30f;

    [Header("Default Bow Upgrade Stats")]
    public int defaultBowArrowsPerShot = 1;
    public int defaultBowPierceCount = 0;
    public float defaultBowAngleBetweenArrows = 0f;
    public float defaultBowMultishotCooldown = 3f;

    [Header("Default Flare Upgrade Stats")]
    public float defaultFlareRadiusBonus = 0f;
    public float defaultBlueFlareCooldown = 20f;
    public float defaultBlueFlareSlowMultiplier = 0.6f;

    [Header("Default Player Stats")]
    public float defaultMoveSpeed = 12f;
    public int defaultMaxHealth = 10;
    public int defaultHpRegen = 0;
    public float defaultXpNeeded = 20f;
    public float defaultLevel = 0f;

    [Header("Sword Stats")]
    public float swordDamage;
    public bool swordUpgradeUnlocked = false;
    public float swordAttackSpeed;
    
    [Header("Bow Stats")]
    public float bowDamage;

    [Header("Bow Upgrade Stats")]
    public int bowArrowsPerShot = 1;
    public int bowPierceCount = 0;
    public bool bowMultishotUnlocked = false;
    public float bowAngleBetweenArrows = 0f;
    public float bowMultishotCooldown = 3f;

    [Header("Flare Upgrade Stats")]
    public float flareRadiusBonus = 0f;
    public bool blueFlareUnlocked = false;
    public float blueFlareCooldown = 30f;
    public float blueFlareSlowMultiplier = 0.6f;

    [Header("Player Stats")]
    public float moveSpeed;
    public int maxHealth;
    public int curHealth;
    public int hpRegen;

    [Header("Misc")]
    public float xp;
    public float xpNeeded;
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
        // sword combat
        swordDamage = defaultSwordDamage;
        swordAttackSpeed = defaultSwordAttackSpeed;

        // bow combat
        bowDamage = defaultBowDamage;

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
                    curHealth = maxHealth;
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
                } else if (i == 13)
                {
                    bowPierceCount = 1;
                }
            }
        }

        // misc
        xp = 0f;
        xpNeeded = defaultXpNeeded;
        level = defaultLevel;
        totalKilled = 0f;
    }

    public void Heal(int amount)
    {
        curHealth = Math.Min(curHealth + amount, maxHealth);
    }
    public void HealAfterLevel()
    {
        Heal(hpRegen);
    }
}