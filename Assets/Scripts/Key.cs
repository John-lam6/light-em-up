using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {
    [SerializeField] private AudioClip pickupSound;
    private AudioSource audioSource;
    private TutorialManager tutorialManager;
    private Collider doorCollider;
    private Door door;
    void Start()
    {
        audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();

        GameObject tutorialManagerObj = GameObject.Find("TutorialManager");
        if(tutorialManagerObj)
        {
            tutorialManager = tutorialManagerObj.GetComponent<TutorialManager>();    
        }

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
            audioSource.PlayOneShot(pickupSound);
            Destroy(gameObject);
        }
    }
}
