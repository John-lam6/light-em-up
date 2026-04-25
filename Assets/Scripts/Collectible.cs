using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectible : MonoBehaviour
{
    [SerializeField] private AudioClip pickupSound;
    private AudioSource audioSource;
    private bool isCollectible = false;
    protected abstract void Collect();
    void Start()
    {
        audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();

        StartCoroutine(SpawnDelay());
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && isCollectible)
        {
            Collect();
            audioSource.PlayOneShot(pickupSound);
            Destroy(gameObject);
        }
    }

    IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(1.0f);
        isCollectible = true;
    }
}
