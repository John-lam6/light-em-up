using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarManager : MonoBehaviour {
    [SerializeField] private ToolBase[] tools;
    private int active_slot = 0;
    
    private bool paused = false;

    void Start() {
        if (tools == null || tools.Length < 3) {
            Debug.Log ("Tools is null or has insufficient number of tools");
        }
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
        if (active_slot == newSlot) return; 
        tools[active_slot].Unequip();
        tools[newSlot].Equip();
        active_slot = newSlot;
    }
}
