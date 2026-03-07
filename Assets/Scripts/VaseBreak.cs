using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaseBreak : MonoBehaviour
{
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
        Instantiate(particlePrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}