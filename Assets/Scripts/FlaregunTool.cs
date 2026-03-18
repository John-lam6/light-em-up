using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlaregunTool : ToolBase
{
    public float radius = 10f;
    public float duration = 7f;
    public HotbarManager hotbar;
    
    private bool equipped = false;

    void Start() {
        cooldown = 6f;
        last_use_time = -cooldown;
        hotbar = FindObjectOfType<HotbarManager>();
    }


    void Update() {
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
        hotbar.StartCoroutine(hotbar.cooldownSlider(2, cooldown));
    }
    
}
