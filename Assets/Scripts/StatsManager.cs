using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;

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
        DontDestroyOnLoad(gameObject);
        if(Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }
}
