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
    
    // Start is called before the first frame update
    void Start() {
        currentHealth = maxHealth;
    }

    void Update() {
        healthText.text = currentHealth + " / " + maxHealth;
    }

    public void Reset() {
        currentHealth = maxHealth;
        healthSlider.value = maxHealth;
    }

    public IEnumerator Damage(int damage) {
        currentHealth -= damage;
        healthSlider.DOValue((float)currentHealth / maxHealth, sliderEaseTime).SetEase(Ease.Linear);

        yield return new WaitForSeconds(sliderEaseTime);

        if (currentHealth <= 0) {
            currentHealth = 0;
            // END GAME
        }
    }
}