using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
    public Light doorLight;
    
    // Start is called before the first frame update
    void Start()
    {
        doorLight = GetComponentInChildren<Light>(true);
    }

    public void turnOnLight() {
        doorLight.enabled = true;
    }

    public void turnOffLight() {
        doorLight.enabled = false;
    }
}
