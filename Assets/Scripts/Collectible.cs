using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectible : MonoBehaviour
{
    private bool isCollectible = false;
    protected abstract void Collect();
    void Start()
    {
        StartCoroutine(SpawnDelay());
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && isCollectible)
        {
            Collect();
            Destroy(gameObject);
        }
    }

    IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(1.0f);
        isCollectible = true;
    }
}
