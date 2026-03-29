using UnityEngine;

public class RangedTool : ToolBase
{
    public float damage = 10f;
    public float range = 50f;
    public int pierce = 0;
    public float projectile_speed = 35f;

    protected bool equipped = false;
    public HotbarManager hotbar;

    protected virtual void Start()
    {
        cooldown = 1.5f;
        last_use_time = -cooldown;
        hotbar = FindObjectOfType<HotbarManager>();
    }

    public override void Equip()
    {
        equipped = true;
        gameObject.SetActive(true);
    }

    public override void Unequip()
    {
        equipped = false;
        gameObject.SetActive(false);
    }

    public override void Use()
    {
        last_use_time = Time.time;
    }
}