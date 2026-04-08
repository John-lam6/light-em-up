using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorBridge : MonoBehaviour {
    private EnemyMeleeAttack meleeAttack;
    private EnemyRangedAttack rangedAttack;
    
    void Start() {
        meleeAttack = GetComponentInParent<EnemyMeleeAttack>();
        rangedAttack = GetComponentInParent<EnemyRangedAttack>();
    }
    
    public void OnAttackHit()
    {
        if (meleeAttack != null) meleeAttack.OnAttackHit();
        else if (rangedAttack != null) rangedAttack.OnAttackHit();
    }
}
