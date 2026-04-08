using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;
    public GameObject player;
    public float attackCooldown = 1.5f;
    public float attackRange;
    private bool isAttacking = false;
    public EnemyController enemyController;
    public int damage = 1;
    private float lastAttackTime = -999f;
    private bool canMove = true;
    
    void Start()
    {
        m_Animator = GetComponentInChildren<Animator>();
        enemyController = GetComponent<EnemyController>();
        player = GameObject.FindWithTag("Player");
    }
    
    void Update()
    {
        if (enemyController.isDead()) return;

        float targetDistance = GetFlatDistance();
        
        if (canMove && !enemyController.isDead() && targetDistance <= attackRange && !isAttacking && Time.time >= lastAttackTime + attackCooldown) {
            StartCoroutine(Attack());
        }
    }
    
    private IEnumerator Attack() {
        isAttacking = true;
        lastAttackTime = Time.time;
        m_Animator.SetBool("isAttacking", isAttacking);
        
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
        m_Animator.SetBool("isAttacking", isAttacking);
    }

    private float GetFlatDistance() {
        Vector3 toTarget = player.transform.position - transform.position;
        toTarget.y = 0;
        return toTarget.magnitude;
    }

    public void OnAttackHit() {
        if (enemyController.isDead()) return;
        
        // if player is still within range at hit time
        if (GetFlatDistance() > attackRange) return;
        
        PlayerHealth health = player.GetComponent<PlayerHealth>();
        health.StartCoroutine(health.Damage(damage));
    }

    public void setCanMove(bool canmove) {
        canMove = canmove;
    }
}
