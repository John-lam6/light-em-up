using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrowProjectile : MonoBehaviour {
    private Vector3 direction;
    private int damage;
    private float speed;
    public float lifetime = 30f;
    
    private Rigidbody rb;

    void Awake() {
        rb = GetComponent<Rigidbody>();
    }
    
    public void Init(Vector3 dir, int dmg, float spd=20f) {
        direction = dir.normalized;
        damage = dmg;
        speed = spd;
        transform.rotation = Quaternion.LookRotation(dir);

        rb.velocity = direction * speed;
        
        Destroy (gameObject, lifetime); // if it doesn't hit anything in lifetime seconds then it gets destroyed
        
    }

    void OnTriggerEnter(Collider other) {
        // hit player
        if (other.CompareTag ("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null) health.StartCoroutine(health.Damage(damage));

            Destroy(gameObject);
        }

        // hit obstacle
        if (other.CompareTag("Obstacle")) {
            GetComponent<Rigidbody>().isKinematic = true;
            Destroy(gameObject);
        }
    }
}
