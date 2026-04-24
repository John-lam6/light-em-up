using UnityEngine;
using System.Collections;

public class BowTool : RangedTool
{
    // REFERENCES
    public GameObject arrowPrefab;
    public Transform arrowSpawn;

    // AUDIO
    public AudioClip shootSound;

    // HOTBAR
    [Header("Hotbar")]
    [SerializeField] private int hotbarSlotIndex = 1;

    // SHARED VISIBLE COOLDOWN
    [Header("Bow Cooldown")]
    [SerializeField] private float bowHotbarCooldown = 0.75f;

    // SPAWN TUNING
    [Header("Arrow Spawn Tuning")]
    [SerializeField] private float forwardSpawnOffset = 0.6f;
    [SerializeField] private float sidewaysSpawnOffset = 0.08f;
    

    private float last_multishot_time = -999f;
    public AudioSource audioSource;
    private Animator anim;

    public Sprite multishotIcon;

    protected override void Start()
    {
        base.Start();
        cooldown = bowHotbarCooldown;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        if (!equipped)
        {
            HideAbilityUI();
            return;
        }

        if (Input.GetMouseButton(0))
            Use();

        if (Input.GetMouseButton(1))
            if(StatsManager.Instance != null && StatsManager.Instance.bowMultishotUnlocked)
            {
                UseMultiShot();
            }

        UpdateAbilityUI();
    }

    public override void Equip()
    {
        equipped = true;
        gameObject.SetActive(true);
    }

    public override void Unequip()
    {
        equipped = false;

        if (hotbar != null)
            hotbar.HideAbilityCooldown();

        gameObject.SetActive(false);
    }

    // NORMAL SHOT
    public override void Use()
    {
        if (!equipped) return;

        if (hotbar != null && hotbar.IsOnCooldown(hotbarSlotIndex)) return;

        if (arrowPrefab == null)
        {
            Debug.LogError("BowTool ERROR: arrowPrefab is not assigned!");
            return;
        }

        if (arrowSpawn == null)
        {
            Debug.LogError("BowTool ERROR: arrowSpawn is not assigned!");
            return;
        }

        if (anim != null)
        {
            anim.Play("Shoot", 0, 0f);
        }

        if (hotbar != null)
            hotbar.StartCoroutine(hotbar.cooldownSlider(hotbarSlotIndex, cooldown));

        ShootArrow(1);

        // single shot sound
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound, 0.8f);
        }
    }

    // MULTISHOT
    void UseMultiShot()
    {
        if (!equipped) return;

        if (hotbar != null && hotbar.IsOnCooldown(hotbarSlotIndex)) return;

        if (arrowPrefab == null)
        {
            Debug.LogError("BowTool ERROR: arrowPrefab is not assigned!");
            return;
        }

        if (arrowSpawn == null)
        {
            Debug.LogError("BowTool ERROR: arrowSpawn is not assigned!");
            return;
        }

        if (StatsManager.Instance == null) return;

        if (Time.time < last_multishot_time + StatsManager.Instance.bowMultishotCooldown)
            return;

        last_multishot_time = Time.time;

        if (anim != null)
        {
            anim.Play("Shoot", 0, 0f);
        }

        // visible shared cooldown stays same as normal shot
        if (hotbar != null)
            hotbar.StartCoroutine(hotbar.cooldownSlider(hotbarSlotIndex, cooldown));

        ShootArrow(StatsManager.Instance.bowArrowsPerShot);

        // multishot delayed sound
        StartCoroutine(MultishotSound());
    }

    void ShootArrow(int arrowsToShoot)
    {
        int totalArrowsToShoot = Mathf.Max(1, arrowsToShoot);
        Debug.Log("Shooting " + totalArrowsToShoot + " arrows");

        float angleBetweenArrows = 0f;
        if (StatsManager.Instance != null)
            angleBetweenArrows = StatsManager.Instance.bowAngleBetweenArrows;

        for (int i = 0; i < totalArrowsToShoot; i++)
        {
            float centerOffset = i - (totalArrowsToShoot - 1) / 2f;
            float angle = centerOffset * angleBetweenArrows;

            Vector3 baseDirection = arrowSpawn.forward;
            //baseDirection += Vector3.down * 0.1f;
            baseDirection.Normalize();

            Vector3 shotDirection = Quaternion.AngleAxis(angle, arrowSpawn.up) * baseDirection;

            Vector3 spawnPosition =
                arrowSpawn.position +
                arrowSpawn.forward * forwardSpawnOffset +
                arrowSpawn.right * (centerOffset * sidewaysSpawnOffset);

            SpawnArrow(spawnPosition, shotDirection);
        }
    }

    void SpawnArrow(Vector3 spawnPosition, Vector3 shotDirection)
    {
        GameObject arrowInstance = Instantiate(
            arrowPrefab,
            spawnPosition,
            Quaternion.LookRotation(shotDirection)
        );

        Rigidbody rb = arrowInstance.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = shotDirection.normalized * projectile_speed;
        }

        ArrowProjectile arrowProjectile = arrowInstance.GetComponent<ArrowProjectile>();
        if (arrowProjectile != null)
        {
            float bowDamage = 10f;
            int bowPierce = 0;

            if (StatsManager.Instance != null)
            {
                bowDamage = StatsManager.Instance.bowDamage;
                bowPierce = StatsManager.Instance.bowPierceCount;
            }

            arrowProjectile.damage = bowDamage;
            arrowProjectile.remainingPierces = bowPierce;

            Debug.Log("Arrow spawned with damage: " + bowDamage + " and pierce: " + bowPierce);
        }

        Collider arrowCol = arrowInstance.GetComponent<Collider>();
        Collider bowCol = GetComponent<Collider>();

        if (arrowCol != null && bowCol != null)
        {
            Physics.IgnoreCollision(arrowCol, bowCol);
        }
    }

    // multishot sound coroutine
    IEnumerator MultishotSound()
    {
        if (audioSource == null || shootSound == null) yield break;

        int count = StatsManager.Instance != null ? StatsManager.Instance.bowArrowsPerShot : 1;

        for (int i = 0; i < count; i++)
        {
            audioSource.PlayOneShot(shootSound, 0.8f);

            if (i < count - 1)
                yield return new WaitForSeconds(0.06f);
        }
    }

    void UpdateAbilityUI()
    {
        if (hotbar == null || StatsManager.Instance == null) return;

        bool show = equipped && StatsManager.Instance.bowArrowsPerShot > 1;

        if (!show)
        {
            hotbar.HideAbilityCooldown();
            return;
        }

        float cooldown = StatsManager.Instance.bowMultishotCooldown;
        float timeRemaining = Mathf.Max(0f, (last_multishot_time + cooldown) - Time.time);

        hotbar.ShowAbilityCooldown(multishotIcon, timeRemaining, cooldown);
    }


    void HideAbilityUI()
    {
        if (hotbar != null)
            hotbar.HideAbilityCooldown();
    }
}