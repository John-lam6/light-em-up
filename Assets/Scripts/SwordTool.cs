using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTool : MeleeTool
{   
    [Header("Hotbar")]
    [SerializeField] private int hotbarSlotIndex = 0; 


    [SerializeField] private Animator animator;
    [SerializeField] private SwordHitbox hitbox;

    private float swingClipLength;

    private float attackSpeed;

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
        if (!equipped) return; // Only work if sword is equipped

        if (Input.GetButtonDown("Fire1"))
            Use();
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
    }

}
