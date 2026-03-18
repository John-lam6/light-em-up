using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ToolBase : MonoBehaviour {
    public float cooldown;
    protected float last_use_time;
    
    public abstract void Equip();
    public abstract void Unequip();
    public abstract void Use();

    // returns whether cooldown has passed
    public virtual bool CanUse() {
        return Time.time > last_use_time + cooldown;
    }
}
