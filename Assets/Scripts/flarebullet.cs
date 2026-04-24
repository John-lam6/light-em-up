using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareBullet : MonoBehaviour
{
    public Light flarelight;
    private AudioSource flaresound;
    private Rigidbody rb;

    private bool burning = true;
    private bool landed = false;

    private float smooth = 2.4f;
    private Vector3 adjustedTargetPoint;

    public float flareTimer = 7f;
    public float radius = 10f;
    public float landedLightIntensity = 1f;
    public AudioClip flareBurningSound;
    public AudioClip blueFlareBurningSound;

    public Vector3 targetPoint;
    public float moveSpeed = 20f;
    public float landedHeightOffset = 0.3f;

    [Header("Audio Settings")]
    [SerializeField] private float flareVolume = 0.3f;
    [SerializeField] private float flareSpatialBlend = 1f;
    [SerializeField] private float flareMinDistance = 2f;

    [HideInInspector] public bool isBlueFlare = false;

    private Dictionary<EnemyController, float> slowedEnemies = new Dictionary<EnemyController, float>();

    void Start()
    {
        flaresound = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        adjustedTargetPoint = targetPoint + Vector3.up * landedHeightOffset;

        if (StatsManager.Instance != null)
        {
            radius += StatsManager.Instance.flareRadiusBonus;
        }
        Debug.Log("Flare parent is: " + transform.parent);

        if (flaresound != null)
        {
            flaresound.volume = 1f;
            flaresound.spatialBlend = 1f;
            flaresound.minDistance = radius * 0.2f;
            flaresound.maxDistance = radius * 2.8f;
            flaresound.rolloffMode = AudioRolloffMode.Linear;
            flaresound.dopplerLevel = 0f;
        }

        if (isBlueFlare)
        {
            flarelight.color = Color.blue;
        }
        else
        {
            flarelight.color = new Color(1f, 0.35f, 0f);
        }

        if (isBlueFlare)
        {
            flaresound.PlayOneShot(blueFlareBurningSound, 3f);
        }
        else
        {
            flaresound.PlayOneShot(flareBurningSound, 1f);
        }

        rb.useGravity = false;
        rb.isKinematic = true;

        Destroy(gameObject, flareTimer + 2f);
    }

    void Update()
    {
        if (!landed)
        {
            transform.position = Vector3.MoveTowards(transform.position, adjustedTargetPoint, moveSpeed * Time.deltaTime);

            Vector3 direction = adjustedTargetPoint - transform.position;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }

            if (Vector3.Distance(transform.position, adjustedTargetPoint) < 0.05f)
            {
                Land();
            }
        }

        if (landed && burning)
        {
            flarelight.intensity = Mathf.Lerp(flarelight.intensity, landedLightIntensity, Time.deltaTime * smooth);
            flarelight.range = Mathf.Lerp(flarelight.range, radius, Time.deltaTime * smooth);

            if (isBlueFlare)
            {
                HandleBlueFlareEffect();
            }
        }
        else if (!landed && burning)
        {
            flarelight.intensity = landedLightIntensity;
            flarelight.range = radius;
        }
        else
        {
            flarelight.intensity = Mathf.Lerp(flarelight.intensity, 0f, Time.deltaTime * smooth);
            flarelight.range = Mathf.Lerp(flarelight.range, 0f, Time.deltaTime * smooth);

            flaresound.volume = Mathf.Lerp(flaresound.volume, 0f, Time.deltaTime * smooth);
        }
    }

    void Land()
    {
        landed = true;
        transform.position = adjustedTargetPoint;
        StartCoroutine(FlareLife());
    }

    IEnumerator FlareLife()
    {
        burning = true;
        yield return new WaitForSeconds(flareTimer);
        burning = false;
        RestoreAllEnemies();
    }

    void HandleBlueFlareEffect()
    {
        if (StatsManager.Instance == null) return;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        HashSet<EnemyController> enemiesInRange = new HashSet<EnemyController>();

        foreach (Collider hit in hitColliders)
        {
            EnemyController enemy = hit.GetComponentInParent<EnemyController>();

            if (enemy != null && !enemy.isDead())
            {
                enemiesInRange.Add(enemy);

                if (enemy.agent != null && enemy.agent.enabled)
                {
                    if (!slowedEnemies.ContainsKey(enemy))
                    {
                        slowedEnemies.Add(enemy, enemy.agent.speed);
                        enemy.agent.speed *= StatsManager.Instance.blueFlareSlowMultiplier;
                    }
                }
            }
        }

        List<EnemyController> enemiesToRestore = new List<EnemyController>();

        foreach (var slowedEnemy in slowedEnemies)
        {
            if (!enemiesInRange.Contains(slowedEnemy.Key))
            {
                if (slowedEnemy.Key != null && slowedEnemy.Key.agent != null && slowedEnemy.Key.agent.enabled)
                {
                    slowedEnemy.Key.agent.speed = slowedEnemy.Value;
                }

                enemiesToRestore.Add(slowedEnemy.Key);
            }
        }

        foreach (EnemyController enemy in enemiesToRestore)
        {
            slowedEnemies.Remove(enemy);
        }
    }

    void RestoreAllEnemies()
    {
        foreach (var slowedEnemy in slowedEnemies)
        {
            if (slowedEnemy.Key != null && slowedEnemy.Key.agent != null && slowedEnemy.Key.agent.enabled)
            {
                slowedEnemy.Key.agent.speed = slowedEnemy.Value;
            }
        }

        slowedEnemies.Clear();
    }

    void OnDestroy()
    {
        RestoreAllEnemies();
    }
}