using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    public GameObject display;
    public TextMeshProUGUI currencyText;

    void Awake()
    {
        Instance = this; 
    }

    public void UpdateDisplay(int amount)
    {
        StartCoroutine(displayChange(amount));
    }

            

    IEnumerator displayChange(int amount)
    {
        display.SetActive(true);
        currencyText.text = $"+{amount}\t" + PlayerPrefs.GetInt("PermCurrency", 0).ToString();
        yield return new WaitForSeconds(5f);
        display.SetActive(false);
        
    }
}
