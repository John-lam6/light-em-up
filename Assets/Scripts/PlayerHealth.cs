using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public TMP_Text healthText;
    public Slider healthSlider;

    public int maxHealth;
    public int currentHealth;
    public float sliderEaseTime = 0.15f;
    
    public AudioSource audioSource;
    public AudioClip audioClip;
    public AudioClip deathSound;
    
    // Start is called before the first frame update
    void Start() {
        if (StatsManager.Instance != null) {
            maxHealth = StatsManager.Instance.maxHealth;
            currentHealth = StatsManager.Instance.curHealth;
        }
        currentHealth = maxHealth;
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
        audioSource.volume = 0.3f;
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