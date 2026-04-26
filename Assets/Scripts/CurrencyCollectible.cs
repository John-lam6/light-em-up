using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyCollectible : Collectible
{
    [SerializeField] private int currencyGain;
    private CurrencyManager currencyManager;
    void Awake()
    {
        currencyManager = GameObject.Find("CurrencyManager").GetComponent<CurrencyManager>();
    }
    protected override void Collect()
    {
        StatsManager.Instance.currency += currencyGain;
        currencyManager.UpdateDisplay(currencyGain);
    }
}
