using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyDisplayer : MonoBehaviour
{
    public TextMeshProUGUI text;
    void Update()
    {
        text.text = StatsManager.Instance.currency.ToString();       
    }
}
