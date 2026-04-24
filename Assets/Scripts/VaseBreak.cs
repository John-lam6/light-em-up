using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaseBreak : MonoBehaviour
{
    public AudioClip vase_break;
    public GameObject particlePrefab; 
    [SerializeField] private List<GameObject> droppables; 
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
        AudioSource.PlayClipAtPoint(vase_break, transform.position, 50f);
        Instantiate(particlePrefab, transform.position, transform.rotation);
        Instantiate(droppables[Random.Range(0, droppables.Count - 1)], transform.position, transform.rotation);
        Destroy(gameObject);
    }
}