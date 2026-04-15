using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public TMP_Text healthText;
    public Slider healthSlider;

    public int maxHealth = 8;
    public int currentHealth;
    public float sliderEaseTime = 0.15f;
    
    public AudioSource audioSource;
    public AudioClip audioClip;
    
    // Start is called before the first frame update
    void Start() {
        currentHealth = maxHealth;

        if (StatsManager.Instance != null) {
            StatsManager.Instance.maxHealth = maxHealth;
            StatsManager.Instance.curHealth = currentHealth;
        }
    }

    void Update() {
        healthText.text = currentHealth + " / " + maxHealth;
    }

    public void Reset() {
        currentHealth = maxHealth;
        healthSlider.value = maxHealth;

        if (StatsManager.Instance != null) {
            StatsManager.Instance.curHealth = currentHealth;
        }
    }

    public IEnumerator Damage(int damage) {
        audioSource.volume = 0.35f;
        audioSource.PlayOneShot(audioClip);

        currentHealth -= damage;
        if (currentHealth <= 0) currentHealth = 0;

        if (StatsManager.Instance != null) {
            StatsManager.Instance.curHealth = currentHealth;
        }
        
        healthSlider.DOKill();
        healthSlider.DOValue((float)currentHealth / maxHealth, sliderEaseTime).SetEase(Ease.Linear);

        yield return new WaitForSeconds(sliderEaseTime);

        if (currentHealth == 0) {
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