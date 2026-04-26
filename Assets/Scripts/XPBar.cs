using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPBar : MonoBehaviour
{
    public Image fillImage;
    public TMP_Text xpText;
    private float currentFill;
    void Update()
    {
        if (StatsManager.Instance != null)
        {  
            float targetFill = StatsManager.Instance.xp / StatsManager.Instance.xpNeeded;
            currentFill = Mathf.MoveTowards(currentFill, targetFill, Time.deltaTime * 1f);
            if (xpText != null) xpText.text = $"{StatsManager.Instance.xp:F0} / {StatsManager.Instance.xpNeeded:F0}";
            fillImage.fillAmount = currentFill;
        }
    }    
}

