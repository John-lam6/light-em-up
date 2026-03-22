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

    [Header("Player Stats")]
    public float moveSpeed;
    public float maxHealth;
    public float curHealth;
    public float hpRegen;

    [Header("Misc")]
    public float enemiesKilled;
    public float totalKilled;
    public float enemiesNeeded; //for level up
    public float level;

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
