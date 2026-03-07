using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedTool : ToolBase
{
    public float damage = 10f;
    public float range = 50f;
    public int pierce = 0;
    public float projectile_speed = 35f;
    
    
    void Start() {
        cooldown = 0.8f;
    }
    
    void Update()
    {
        
    }
    
    public override void Equip() {
        
    }

    public override void Unequip() {
        
    }
    
    public override void Use() {
        
    }
}
