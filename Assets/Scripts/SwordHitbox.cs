using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    private float damage;
    private bool canHit = false;

    public void Activate()
    {
        damage = StatsManager.Instance.swordDamage;
        canHit = true;
    }

    public void Deactivate()
    {
        canHit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canHit) return;

        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Hit enemy for " + damage);
            EnemyController enemy = other.GetComponent<EnemyController>();
            enemy.StartCoroutine(enemy.DamageAgent((int)damage));
        }
    }
}
