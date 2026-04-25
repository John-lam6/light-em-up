using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {
    private TutorialManager tutorialManager;
    private Collider doorCollider;
    private Door door;
    void Start()
    {
        tutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();

        door = GameObject.Find("Door").GetComponent<Door>();
        doorCollider = GameObject.Find("Double Door Frame").GetComponent<Collider>();

        doorCollider.enabled = false;
    }
    void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player"))
        {
            if(tutorialManager)
            {
                tutorialManager.BeginExitPhase();
            }

            doorCollider.enabled = true;
            door.turnOnLight();
            Destroy(gameObject);
        }
    }
}
