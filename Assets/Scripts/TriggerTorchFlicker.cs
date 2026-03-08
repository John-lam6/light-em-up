using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTorchFlicker : MonoBehaviour
{
    public Light torchLight;
    public float flickerSpeed = 0.1f;
    public float minIntensity = 0.8f;
    public float maxIntensity = 1.0f;
    public float fadeSpeed = 0.02f;
    private bool flickering = false;
    private bool fading = false;
    private int flickerDirection = 1;
    void Start()
    {
        torchLight.intensity = 0.0f;
    }
    void Update()
    {
        if(flickering)
        {
            torchLight.intensity += flickerSpeed * flickerDirection;
            if(torchLight.intensity <= minIntensity || torchLight.intensity >= maxIntensity)
            {
                flickerDirection *= -1;
            }
        }
        else if(fading)
        {
            torchLight.intensity -= fadeSpeed;
            if(torchLight.intensity <= 0.0f)
            {
                fading = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if(player)
        {
            torchLight.intensity = minIntensity;
            flickerDirection = 1;
            flickering = true;
            fading = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if(player)
        {
            torchLight.intensity = maxIntensity;
            flickering = false;
            fading = true;
        }
    }
}
