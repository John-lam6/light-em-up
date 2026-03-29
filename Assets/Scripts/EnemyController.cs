using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;
    public Transform target;
    public Color damageColor;
    
    private float nextPathUpdateTime = 0f;
    private float updateRate = 0.2f;
    
    public TMP_Text healthText;
    public Slider healthSlider;
    
    public int maxHealth = 100;
    public int currentHealth;
    public float sliderEaseTime = 0.05f;
    
    public float deathDelay = 1.2f;
    private bool dead = false;
    private CapsuleCollider capsule;
    private bool canMove = false;
    
    [HideInInspector]
    public NavMeshAgent agent;
    [HideInInspector]
    public Rigidbody rb;
    [HideInInspector]
    public Material mat;
    [HideInInspector]
    public Color m_Color;
    
    
    // Start is called before the first frame update
    void Start()
    {
        capsule = GetComponent<CapsuleCollider>();
        m_Animator = GetComponentInChildren<Animator>();
        target = FindObjectByTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        mat = GetComponentInChildren<Renderer>().material;
        m_Color = mat.color;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //healthText.text = currentHealth + " / " + maxHealth;
        if (Time.time >= nextPathUpdateTime) {
            nextPathUpdateTime = Time.time + updateRate;
            if (!dead && !agent.isStopped) agent.SetDestination(target.position);
        }
    }
    
    public GameObject FindObjectByTag(string tag)
    {
        GameObject obj = GameObject.FindWithTag(tag);
        return obj;
    }
    
    public bool isDead() {
        return dead;
    }
    
    public IEnumerator DamageAgent(int damage) {
        if (dead) yield break;
        
        if (agent.enabled && !dead && canMove) {
            agent.isStopped = true;
            mat.DOColor(damageColor, 0.1f);
            yield return new WaitForSeconds(0.1f);

            //rb.velocity = Vector3.zero;
            //rb.angularVelocity = Vector3.zero;

            if (currentHealth - damage > 0) agent.ResetPath();
            mat.DOColor(m_Color, 0.1f);
        }
        
        if (currentHealth - damage < 0) currentHealth = 0;
        else currentHealth -= damage;
        
        healthSlider.DOValue((float)currentHealth / maxHealth, sliderEaseTime).SetEase(Ease.Linear);
    
        yield return new WaitForSeconds(sliderEaseTime);
        
        if (currentHealth <= 0 && !dead) {
            agent.isStopped = true;
            agent.enabled = false;
            dead = true;
            m_Animator.SetBool("isDead", dead);
            capsule.enabled = false;
            //rb.velocity = Vector3.zero;
            //rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            //gameManager.addScore(score);
            yield return new WaitForSeconds(deathDelay);
            Destroy(gameObject);
        }
        else if (!dead) {
            agent.isStopped = false;
        }
    }

    public void setCanMove(bool canMove) {
        this.canMove = canMove;
    }
}
