using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    [HideInInspector]
    public float damage;

    [HideInInspector]
    public int remainingPierces = 0;

    private Rigidbody rb;
    private bool hasStuck = false;
    private Collider arrowCollider;
    private float launchSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        arrowCollider = GetComponent<Collider>();

        if (rb != null)
        {
            launchSpeed = rb.velocity.magnitude;
        }
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

        // ENEMY HIT
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();

            if (enemy != null)
            {
                enemy.StartCoroutine(enemy.DamageAgent((int)damage));
                Debug.Log("Hit enemy for " + damage + " damage");
            }

            // PIERCE ONLY ON ENEMIES
            if (remainingPierces > 0)
            {
                remainingPierces--;

                Collider hitCollider = collision.collider;
                if (arrowCollider != null && hitCollider != null)
                {
                    Physics.IgnoreCollision(arrowCollider, hitCollider);
                }

                if (rb != null)
                {
                    rb.velocity = transform.forward * launchSpeed;
                    rb.angularVelocity = Vector3.zero;
                }

                return;
            }

            Destroy(gameObject);
            return;
        }

        // ANYTHING THAT IS NOT AN ENEMY
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

        transform.SetParent(collision.transform, true);

        Destroy(gameObject, 2f);
    }
}