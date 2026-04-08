using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaseBreak : MonoBehaviour
{
    public AudioClip vase_break;
    public GameObject particlePrefab; 
    // Update is called once per frame
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            breakVase();
        }
    }

    void breakVase()
    {
        AudioSource.PlayClipAtPoint(vase_break, transform.position);
        Instantiate(particlePrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}