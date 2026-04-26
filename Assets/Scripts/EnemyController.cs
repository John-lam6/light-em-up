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
    public int xpValue;
    public float permCurrencyChance;
    public TMP_Text currencyText;
    private float nextPathUpdateTime = 0f;
    private float updateRate = 0.2f;
    
    public int maxHealth = 100;
    public int currentHealth;
    public float sliderEaseTime = 0.05f;
    
    public float deathDelay = 1.2f;
    private bool dead = false;
    private CapsuleCollider capsule;
    private bool canMove = false;

    [Header("Audio")]
    public AudioClip enemyDamageSound;
    private AudioSource audioSource;

    [Header("Enemy Damage Sound Settings")]
    [SerializeField] private float damageSoundVolume = 1f;
    [SerializeField] private float damageSoundSpatialBlend = 1f;
    [SerializeField] private float damageSoundMinDistance = 2f;
    [SerializeField] private float damageSoundMaxDistance = 15f;
    [SerializeField] private AudioRolloffMode damageSoundRolloff = AudioRolloffMode.Linear;

    
    [HideInInspector]
    public NavMeshAgent agent;
    [HideInInspector]
    public Rigidbody rb;
    [HideInInspector]
    public Renderer[] renderers;
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
        renderers = GetComponentsInChildren<Renderer>();
        m_Color = renderers[0].material.color;
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
        {
            audioSource.volume = 0.65f;
            audioSource.spatialBlend = 0.8f;
            audioSource.minDistance = 1f;
            audioSource.maxDistance = 7;
            audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
            audioSource.dopplerLevel = 0f;
        }

        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
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

    public int GetHealth() {
        return currentHealth;
    }
    
    public IEnumerator DamageAgent(int damage) {
        if (dead) yield break;
        
        if (agent.enabled && !dead && canMove) {
            agent.isStopped = true;
            
            
            foreach (Renderer r in renderers)
            {
                r.material.EnableKeyword("_EMISSION");
                r.material.SetColor("_EmissionColor", Color.black);
                r.material.DOColor(damageColor * 0.3f, "_EmissionColor", 0.1f);
            }

            yield return new WaitForSeconds(0.1f);

            foreach (Renderer r in renderers)
            {
                r.material.DOColor(Color.black, "_EmissionColor", 0.1f);
            }
            
            /*
            foreach (Renderer r in renderers)
                r.material.DOColor(damageColor, 0.1f);
            
            yield return new WaitForSeconds(0.1f);
            */
            //rb.velocity = Vector3.zero;
            //rb.angularVelocity = Vector3.zero;

            if (currentHealth - damage > 0) agent.ResetPath();
            
            
            //foreach (Renderer r in renderers)
                //r.material.DOColor(m_Color, 0.1f);
        }
        
        if (audioSource != null && enemyDamageSound != null)
        {
            audioSource.PlayOneShot(enemyDamageSound);
        }

        if (currentHealth - damage < 0) currentHealth = 0;
        else currentHealth -= damage;


    
        yield return new WaitForSeconds(sliderEaseTime);
        
        if (currentHealth <= 0 && !dead) {
            StatsManager.Instance.xp += xpValue;
            if (permCurrencyChance > 1f)
            {
                PlayerPrefs.SetInt("PermCurrency", PlayerPrefs.GetInt("PermCurrency", 0) + (int)permCurrencyChance);
                CurrencyManager.Instance.UpdateDisplay((int)permCurrencyChance);    
            } else
            {
                float roll = Random.value; // gives a number between 0.0 and 1.0
                if (roll < permCurrencyChance)
                {
                    PlayerPrefs.SetInt("PermCurrency", PlayerPrefs.GetInt("PermCurrency", 0) + 1);
                    CurrencyManager.Instance.UpdateDisplay(1);
                }
            }
            
            //Debug.Log("Gained " + xpValue + " XP " + StatsManager.Instance.xp + " / " + StatsManager.Instance.xpNeeded);
            agent.isStopped = true;
            agent.enabled = false;
            dead = true;
            m_Animator.SetBool("isDead", dead);
            capsule.enabled = false;
            rb.isKinematic = true;

            while (!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("death") && !m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Z_FallingBack") && !m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Z_FallingBack 0")) {
                yield return null;
            }
            
            AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
            float deathAnimLength = stateInfo.length;
            yield return new WaitForSeconds(deathAnimLength);
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