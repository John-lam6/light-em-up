using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class PermaUpgrade : MonoBehaviour
{
    public string upgradeID;
    public int cost;
    public TMP_Text costText;
    private Color normalColor = Color.white;
    private Color errorColor = Color.red;
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();

        if (PlayerPrefs.GetInt(upgradeID, 0) == 1)
        {
            SetPurchasedState();
        }
    }

    public void Purchase()
    {
        if (PlayerPrefs.GetInt(upgradeID, 0) == 1)
        {
            Debug.Log("Upgrade already purchased: " + upgradeID);
            return;
        }
            

        if (PlayerPrefs.GetInt("PermCurrency", 0) < cost)
        {
            Debug.Log("Not enough currency to purchase: " + upgradeID);
            costText.DOColor(errorColor, 0.15f).OnComplete(() =>
            {
                costText.DOColor(normalColor, 0.3f);
            });
            return;
        }

        PlayerPrefs.SetInt("PermCurrency", PlayerPrefs.GetInt("PermCurrency", 0) - cost);
        Debug.Log("Purchased upgrade: " + upgradeID);
        PlayerPrefs.SetInt(upgradeID, 1);
        PlayerPrefs.Save();

        SetPurchasedState();
    }

    void SetPurchasedState()
    {
        button.interactable = false;
        DOTween.Kill(gameObject, complete: false);
    }

}