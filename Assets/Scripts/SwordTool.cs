using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTool : MeleeTool
{   
    [Header("Hotbar")]
    [SerializeField] private int hotbarSlotIndex = 0; 


    [SerializeField] private Animator animator;
    [SerializeField] private SwordHitbox hitbox;

    [Header("Ability UI")]
    public Sprite berzerkIcon;

    private float swingClipLength;
    private float berzerkCooldown = 30f;
    private float berzerk_last_use_time = -999f;
    private float attackSpeed;
    public AudioSource audioSource;
    public AudioClip audioClip;
    public AudioClip beserkSound;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        attackSpeed = StatsManager.Instance.swordAttackSpeed;
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        if (clip.name == "HumanM@Attack1H01_R") { swingClipLength = clip.length; break; }

        cooldown  = swingClipLength / attackSpeed; 
        last_use_time = -cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0f) return;

        if (!equipped)
        {
            HideAbilityUI();
            return;
        }

        if (Input.GetMouseButton(0))
            Use();

        if (Input.GetMouseButton(1))
            if(StatsManager.Instance != null && StatsManager.Instance.swordUpgradeUnlocked)
            {
                UseSecondary();
            }

        UpdateAbilityUI();
    }

    public override void Equip()
    {
        equipped = true;
        gameObject.SetActive(true);
    }

    // UNEQUIP
    public override void Unequip()
    {
        equipped = false;

        if (hotbar != null)
            hotbar.HideAbilityCooldown();

        gameObject.SetActive(false);
    }

    public override void Use()
    {
        if (!equipped) return;
        if (!CanUse()) return;

        // Update cooldown
        last_use_time = Time.time;
        attackSpeed = StatsManager.Instance.swordAttackSpeed;
        cooldown  = swingClipLength  / attackSpeed; 

        // Start cooldown UI on hotbar
        if (hotbar != null)
            hotbar.StartCoroutine(hotbar.cooldownSlider(hotbarSlotIndex, cooldown));
        
        animator.SetFloat("AttackSpeed", attackSpeed);
        animator.SetTrigger("Swing");
        audioSource.volume = 0.8f;
        audioSource.PlayOneShot(audioClip);
    }

    void UseSecondary()
    {
        if (!equipped) return;
        if (Time.time < berzerk_last_use_time + berzerkCooldown) return;

        berzerk_last_use_time = Time.time;
        audioSource.volume = 0.25f;
        audioSource.PlayOneShot(beserkSound);
        if (hotbar != null)
        {
            hotbar.StartCoroutine(hotbar.cooldownSlider(hotbarSlotIndex, cooldown));
        }
        StartCoroutine(TemporaryAttackSpeedBoost(10f));

    }

    IEnumerator TemporaryAttackSpeedBoost(float duration)
    {
        Debug.Log("Sword Berzerk Activated!");

        if (StatsManager.Instance == null) yield break;
        float originalSpeed = StatsManager.Instance.swordAttackSpeed;
        StatsManager.Instance.swordAttackSpeed *= 2;
        yield return new WaitForSeconds(duration);
        StatsManager.Instance.swordAttackSpeed = originalSpeed;

        Debug.Log("Sword Berzerk Ended.");
    }

    void UpdateAbilityUI()
    {
        if (hotbar == null || StatsManager.Instance == null) return;

        bool show = equipped && StatsManager.Instance.swordUpgradeUnlocked;

        if (!show)
        {
            hotbar.HideAbilityCooldown();
            return;
        }

        float timeRemaining = Mathf.Max(0f, (berzerk_last_use_time + berzerkCooldown) - Time.time);

        hotbar.ShowAbilityCooldown(berzerkIcon, timeRemaining, berzerkCooldown);
    }

    void HideAbilityUI()
    {
        if (hotbar != null)
            hotbar.HideAbilityCooldown();
    }
}