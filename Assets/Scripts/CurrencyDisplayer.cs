using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyDisplayer : MonoBehaviour
{
    public TextMeshProUGUI text;
    void Update()
    {
        text.text = PlayerPrefs.GetInt("PermCurrency", 0).ToString();       
    }
}
