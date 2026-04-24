using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public TMP_Text healthText;
    public Slider healthSlider;

    public float sliderEaseTime = 0.15f;
    
    public AudioSource audioSource;
    public AudioClip audioClip;
    public AudioClip deathSound;

    private int lastHp = -1;

    void Update() {
        healthText.text = StatsManager.Instance.curHealth + " / " + StatsManager.Instance.maxHealth;
        if(lastHp != StatsManager.Instance.curHealth)
        {
            healthSlider.DOValue((float)StatsManager.Instance.curHealth / StatsManager.Instance.maxHealth, sliderEaseTime).SetEase(Ease.Linear);
            lastHp = StatsManager.Instance.curHealth;
        }
    }

    public void Reset() {
        StatsManager.Instance.curHealth = StatsManager.Instance.maxHealth;
        healthSlider.value = StatsManager.Instance.maxHealth;
    }

    public IEnumerator Damage(int damage) {
        audioSource.volume = 0.3f;
        audioSource.PlayOneShot(audioClip);

        StatsManager.Instance.curHealth -= damage;
        if (StatsManager.Instance.curHealth <= 0) StatsManager.Instance.curHealth = 0;
        
        healthSlider.DOKill();
        healthSlider.DOValue((float)StatsManager.Instance.curHealth / StatsManager.Instance.maxHealth, sliderEaseTime).SetEase(Ease.Linear);

        yield return new WaitForSeconds(sliderEaseTime);

        if (StatsManager.Instance.curHealth == 0) {
            audioSource.PlayOneShot(deathSound);
            // END GAME
            if (GameEndManager.Instance != null) {
                GameEndManager.Instance.ShowDeath();
            }
        }
    }

    void OnDestroy() {
        if (healthSlider != null) {
            healthSlider.DOKill();
        }
    }
}