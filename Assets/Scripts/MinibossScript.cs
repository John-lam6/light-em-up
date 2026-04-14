using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinibossScript : MonoBehaviour
{
    public AudioSource audiosource;
    public AudioClip clip;
    
    public Transform transform;
    private EnemyController enemyscript;

    public GameObject key;
    private bool dead = false;
    
    void Start()
    {
        transform = GetComponent<Transform>();
        enemyscript = GetComponent<EnemyController>();
        audiosource.volume = 0.35f;
        audiosource.PlayOneShot(clip);
    }

    void Update() {
        if (!dead && enemyscript.GetHealth() <= 0) {
            dead = true;
            SpawnKey();
        }
    }

    private void SpawnKey() {
        Instantiate (key, transform.position, Quaternion.Euler(0,0,0));
    }
}
