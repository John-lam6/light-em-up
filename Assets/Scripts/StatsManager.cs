using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;

    [Header("Default Combat Stats")]
    public float defaultSwordDamage;
    public float defaultSwordRange;
    public float defaultSwordAttackSpeed;
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
    public float defaultMaxHealth;
    public float defaultHpRegen;
    public float defaultXpNeeded = 10f;
    public float defaultLevel = 1f;

    [Header("Combat Stats")]
    public float swordDamage;
    public float swordRange;
    public float swordAttackSpeed;
    public float bowDamage;
    public float bowAttackSpeed;

    [Header("Bow Upgrade Stats")]
    public int bowArrowsPerShot = 1;
    public int bowPierceCount = 0;
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
    public float maxHealth;
    public float curHealth;
    public float hpRegen;

    [Header("Misc")]
    public float xp;
    public float xpNeeded; //for level up
    public float level;
    public float totalKilled;

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
        // keep permanent ability unlock booleans
        bool savedBlueFlareUnlocked = blueFlareUnlocked;

        // combat
        swordDamage = defaultSwordDamage;
        swordRange = defaultSwordRange;
        swordAttackSpeed = defaultSwordAttackSpeed;
        bowDamage = defaultBowDamage;
        bowAttackSpeed = defaultBowAttackSpeed;

        // bow upgrades
        bowArrowsPerShot = defaultBowArrowsPerShot;
        bowPierceCount = defaultBowPierceCount;
        bowAngleBetweenArrows = defaultBowAngleBetweenArrows;
        bowMultishotCooldown = defaultBowMultishotCooldown;

        // flare upgrades
        flareRadiusBonus = defaultFlareRadiusBonus;
        blueFlareCooldown = defaultBlueFlareCooldown;
        blueFlareSlowMultiplier = defaultBlueFlareSlowMultiplier;
        blueFlareDamagePerTick = defaultBlueFlareDamagePerTick;
        blueFlareTickRate = defaultBlueFlareTickRate;

        // restore permanent unlock
        blueFlareUnlocked = savedBlueFlareUnlocked;

        // player
        moveSpeed = defaultMoveSpeed;
        maxHealth = defaultMaxHealth;
        curHealth = maxHealth;
        hpRegen = defaultHpRegen;

        // misc
        xp = 0f;
        xpNeeded = defaultXpNeeded; //for level up
        level = defaultLevel;
        totalKilled = 0f;
    }
}