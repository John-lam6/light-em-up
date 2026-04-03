using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    public float damage = 10f;          // How much damage the arrow does
    public int remainingPierces = 0;    // How many targets the arrow can pass through

    private Rigidbody rb;
    private bool hasStuck = false;      
    private Collider arrowCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        arrowCollider = GetComponent<Collider>();
    }

    void Update()
    {
        // Rotate arrow to follow velocity
        if (!hasStuck && rb != null && rb.velocity.sqrMagnitude > 0.01f)
        {
            transform.forward = rb.velocity.normalized;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasStuck) return;

        // DAMAGE
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Try to get enemy health script
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();

            if (enemy != null)
            {
                enemy.StartCoroutine(enemy.DamageAgent((int)damage));
                Debug.Log("Hit enemy for " + damage + " damage");
            }
        }

        // PIERCE
        if (remainingPierces > 0)
        {
            remainingPierces--;

            Collider hitCollider = collision.collider;
            if (arrowCollider != null && hitCollider != null)
            {
                Physics.IgnoreCollision(arrowCollider, hitCollider);
            }

            return;
        }

        StickToTarget(collision);
    }

    void StickToTarget(Collision collision)
    {
        hasStuck = true;

        ContactPoint contact = collision.contacts[0];

        transform.position = contact.point;
        transform.rotation = Quaternion.LookRotation(-contact.normal);

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        if (arrowCollider != null)
        {
            arrowCollider.enabled = false;
        }

        // Stick to enemy so it moves with them
        transform.SetParent(collision.transform, true);

        //Destroy(gameObject, 1f);
        Destroy(gameObject);
    }
}