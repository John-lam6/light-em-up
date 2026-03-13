using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeTool : ToolBase
{
    public float damage = 10f;
    public float range = 5f;
    
    private bool equipped = false;
    
    public HotbarManager hotbar;

    void Start() {
        cooldown = 1f;
        last_use_time = -cooldown;
        hotbar = FindObjectOfType<HotbarManager>();
    }
    
    void Update()
    {
        if (equipped && Input.GetMouseButton(0) && CanUse()) {
            Use();
        }
    }
    
    public override void Equip() {
        equipped = true;
    }

    public override void Unequip() {
        equipped = false;
    }
    
    public override void Use() {
        last_use_time = Time.time;
        hotbar.StartCoroutine(hotbar.cooldownSlider(0, cooldown));
    }
}
