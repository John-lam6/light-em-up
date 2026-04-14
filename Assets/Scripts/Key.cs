using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {
    public GameObject door;
    public GameObject doubledoor;
    public Collider doorCollider;

    private Door doorscript;
    void Start()
    {
        doorscript = door.GetComponent<Door>();
        doorCollider = doubledoor.GetComponent<Collider>();

        doorCollider.enabled = false;
    }
    void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player"))
        {
            doorCollider.enabled = true;
            doorscript.turnOnLight();
            Destroy(gameObject);
        }
    }
}
