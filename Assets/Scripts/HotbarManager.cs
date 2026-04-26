using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HotbarManager : MonoBehaviour
{
    [SerializeField] private ToolBase[] tools;
    [SerializeField] private Image[] slotImages;
    private int active_slot = 0;

    public Color normalColor;
    public Color activeColor;
    public Color abilityReadyColor = Color.blue;

    public float global_swap_time = 0.5f;
    private float last_swap_time;

    private bool paused = false;

    public Slider swap_slider;
    public AudioSource audioSource;
    public AudioClip audioclip;

    [Header("Ability Slot")]
    [SerializeField] private GameObject abilitySlotRoot;
    [SerializeField] private Image abilityIcon;
    [SerializeField] private Slider abilitySlider;
    [SerializeField] private TMPro.TMP_Text abilityText;

    private Tween swapTween;

    void Start()
    {
        if (tools == null || tools.Length < 3)
        {
            Debug.Log("Tools is null or has insufficient number of tools");
        }

        active_slot = 0;
        tools[active_slot].Equip();
        slotImages[active_slot].color = activeColor;
        last_swap_time = -global_swap_time;
    }

    void Update()
    {
        if (paused) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchSlot(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchSlot(2);
        }
    }

    public void Pause()
    {
        paused = true;
    }

    public void Unpause()
    {
        paused = false;
    }

    private void SwitchSlot(int newSlot)
    {
        if (!(Time.time > last_swap_time + global_swap_time)) return;
        if (active_slot == newSlot) return;

        if (audioSource != null && audioclip != null)
        {
            audioSource.volume = 0.3f;
            audioSource.PlayOneShot(audioclip);
        }

        if (swap_slider != null)
        {
            swap_slider.DOKill();
            swap_slider.gameObject.SetActive(true);
            swap_slider.value = 0;

            swapTween = swap_slider.DOValue(1, global_swap_time)
                .SetEase(Ease.Linear)
                .SetLink(swap_slider.gameObject)
                .OnComplete(() =>
                {
                    if (swap_slider == null) return;
                    swap_slider.gameObject.SetActive(false);
                });
        }

        last_swap_time = Time.time;

        tools[active_slot].Unequip();
        tools[newSlot].Equip();
        slotImages[active_slot].color = normalColor;
        slotImages[newSlot].color = activeColor;
        active_slot = newSlot;
    }

    public IEnumerator cooldownSlider(int activeSlot, float cooldown)
    {
        if (activeSlot < 0 || activeSlot >= slotImages.Length) yield break;
        if (slotImages[activeSlot] == null) yield break;

        Slider slider = slotImages[activeSlot].GetComponentInChildren<Slider>(true);
        if (slider == null) yield break;

        slider.DOKill();
        slider.gameObject.SetActive(true);
        slider.value = 0;

        slider.DOValue(1, cooldown)
            .SetEase(Ease.Linear)
            .SetLink(slider.gameObject)
            .OnComplete(() =>
            {
                if (slider == null) return;
                slider.gameObject.SetActive(false);
            });

        yield return null;
    }

    public bool IsOnCooldown(int slot)
    {
        if (slot < 0 || slot >= slotImages.Length) return false;
        if (slotImages[slot] == null) return false;

        Slider slider = slotImages[slot].GetComponentInChildren<Slider>(true);
        if (slider == null) return false;

        return slider.gameObject.activeSelf;
    }

    public void ShowAbilityCooldown(Sprite icon, float timeRemaining, float maxCooldown)
    {
        if (abilitySlotRoot != null && !abilitySlotRoot.activeSelf)
            abilitySlotRoot.SetActive(true);

        if (abilityIcon != null)
        {
            abilityIcon.sprite = icon;
            abilityIcon.color = abilityReadyColor;
        }

        if (abilitySlider != null)
        {
            abilitySlider.gameObject.SetActive(true);

            if (maxCooldown > 0f)
                abilitySlider.value = 1f - (timeRemaining / maxCooldown);
            else
                abilitySlider.value = 1f;

            if (abilitySlider.fillRect != null)
            {
                Image fillImage = abilitySlider.fillRect.GetComponent<Image>();
                if (fillImage != null)
                    fillImage.color = abilityReadyColor;
            }
        }
    }

    public void HideAbilityCooldown()
    {
        if (abilitySlotRoot != null)
            abilitySlotRoot.SetActive(false);
    }

    private void OnDestroy()
    {
        if (swap_slider != null)
            swap_slider.DOKill();

        if (slotImages != null)
        {
            for (int i = 0; i < slotImages.Length; i++)
            {
                if (slotImages[i] == null) continue;

                Slider slider = slotImages[i].GetComponentInChildren<Slider>(true);
                if (slider != null)
                    slider.DOKill();
            }
        }

        if (abilitySlider != null)
            abilitySlider.DOKill();
    }
}