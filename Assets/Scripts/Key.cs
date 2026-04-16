using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {
    private Collider doorCollider;
    private Door door;
    void Start()
    {
        door = GameObject.Find("Door").GetComponent<Door>();
        doorCollider = GameObject.Find("Double Door Frame").GetComponent<Collider>();

        doorCollider.enabled = false;
    }
    void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player"))
        {
            doorCollider.enabled = true;
            door.turnOnLight();
            Destroy(gameObject);
        }
    }
}
