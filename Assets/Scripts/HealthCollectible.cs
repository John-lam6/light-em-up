using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : Collectible
{
    [SerializeField] private int percentHealing;
    protected override void Collect()
    {
        StatsManager.Instance.Heal((int) (StatsManager.Instance.maxHealth * (percentHealing / 100.0f)));
    }
}
