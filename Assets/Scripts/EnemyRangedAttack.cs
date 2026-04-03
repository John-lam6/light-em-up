using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public float arrowSpeed = 20f;

    public float attackRange = 40f;
    public float attackCooldown = 2.5f;
    public LayerMask obstacleMask;
    public LayerMask playerMask;

    public int damage = 1;

    private bool isAttacking = false;
    private bool canMove = true;
    private float lastAttackTime = -999f;
    
    public GameObject player;
    public EnemyController enemyController;

    void Start() {
        m_Animator = GetComponentInChildren<Animator>();
        enemyController = GetComponent<EnemyController>();
        player = GameObject.FindWithTag("Player");
    }

    void Update() {
        if (enemyController.isDead()) return;

        float targetDistance = GetFlatDistance();
        bool inRangeWithLOS = canMove && HasLineOfSight() && targetDistance <= attackRange;
        
        // stop moving when in range
        if (!enemyController.isDead() && inRangeWithLOS && !enemyController.agent.isStopped) {
            enemyController.agent.isStopped = true;
            m_Animator.SetBool("isWalking", false);
        }
        else if (!inRangeWithLOS && !enemyController.isDead() && enemyController.agent.isStopped) {
            enemyController.agent.isStopped = false;
            m_Animator.SetBool("isWalking", true);
        }
        
        // adjust the rotation still
        if (enemyController.agent.isStopped) {
            Vector3 lookDir = player.transform.position - transform.position;
            lookDir.y = 0;
            if (lookDir != Vector3.zero) {
                Quaternion targetRotation = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
        
        if (canMove && !enemyController.isDead() && !isAttacking && Time.time >= lastAttackTime + attackCooldown && inRangeWithLOS) {
            StartCoroutine(Attack());
        }
    }

    private bool HasLineOfSight() {
        Vector3 origin = arrowSpawnPoint.position;
        Vector3 direction = (player.transform.position - origin).normalized;
        float distance = Vector3.Distance (origin, player.transform.position);

        if (Physics.Raycast(origin, direction, distance, obstacleMask)) return false;
        return true;
    }

    private IEnumerator Attack() {
        isAttacking = true;
        lastAttackTime = Time.time;
        m_Animator.SetBool("isAttacking", isAttacking);
        
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
        m_Animator.SetBool("isAttacking", isAttacking);
    }

    public void OnAttackHit() {
        if (enemyController.isDead()) return;
        
        // if player is still within range at hit time
        if (!HasLineOfSight()) return;
        if (GetFlatDistance() > attackRange) return;
        
        GameObject arrow = Instantiate (arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
        EnemyArrowProjectile proj =  arrow.GetComponent<EnemyArrowProjectile> ();
        if (proj != null) {
            Vector3 direction = (player.transform.position - arrowSpawnPoint.position).normalized;
            proj.Init(direction, damage, arrowSpeed);
        }
    }
    
    private float GetFlatDistance()
    {
        Vector3 toTarget = player.transform.position - transform.position;
        toTarget.y = 0;
        return toTarget.magnitude;
    }

    public void setCanMove(bool canMove) {
        this.canMove = canMove;
    }
}
