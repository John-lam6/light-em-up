using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HotbarManager : MonoBehaviour {
    [SerializeField] private ToolBase[] tools;
    [SerializeField] private Image[] slotImages;
    private int active_slot = 0;
    public Color normalColor;
    public Color activeColor;

    public float global_swap_time = 0.5f;
    private float last_swap_time;
    
    private bool paused = false;

    public Slider swap_slider;
    
    void Start() {
        if (tools == null || tools.Length < 3) {
            Debug.Log ("Tools is null or has insufficient number of tools");
        }

        active_slot = 0;
        tools[active_slot].Equip();
        slotImages[active_slot].color = activeColor;
        last_swap_time = -global_swap_time;
    }

    // Update is called once per frame
    void Update() {
        if (paused) return;

        // if numrow 1 is down
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            SwitchSlot(0);
        }
        // if numrow 2 is down
        else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            SwitchSlot(1);
        }
        // if numrow 3 is down
        else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            SwitchSlot(2);
        }
    }

    public void Pause() {
        paused = true;
    }

    public void Unpause() {
        paused = false;
    }

    private void SwitchSlot(int newSlot) {
        if (!(Time.time > last_swap_time + global_swap_time)) return;
        swap_slider.gameObject.SetActive(true);
        swap_slider.value = 0;
        swap_slider.DOValue (1, global_swap_time).SetEase (Ease.Linear).OnComplete(() => swap_slider.gameObject.SetActive(false));
        last_swap_time = Time.time;
        if (active_slot == newSlot) return; 
        tools[active_slot].Unequip();
        tools[newSlot].Equip();
        slotImages[active_slot].color = normalColor;
        slotImages[newSlot].color = activeColor;
        active_slot = newSlot;
    }

    public IEnumerator cooldownSlider(int activeSlot, float cooldown) {
        Slider slider = slotImages[activeSlot].GetComponentInChildren<Slider>(true);
        slider.gameObject.SetActive(true);
        slider.value = 0;
        slider.DOValue (1, cooldown).SetEase (Ease.Linear).OnComplete(() => slider.gameObject.SetActive(false));
        yield return null;
    }
}
