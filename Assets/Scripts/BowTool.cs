using UnityEngine;

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
    private AudioSource audioSource;

    protected override void Start()
    {
        base.Start();

        audioSource = GetComponent<AudioSource>();
        cooldown = bowHotbarCooldown;
    }

    void Update()
    {
        if (!equipped) return;

        if (Input.GetButtonDown("Fire1"))
            Use();

        if (Input.GetButtonDown("Fire2"))
            UseMultiShot();
    }

    public override void Equip()
    {
        equipped = true;
        gameObject.SetActive(true);
    }

    public override void Unequip()
    {
        equipped = false;
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

        if (hotbar != null)
            hotbar.StartCoroutine(hotbar.cooldownSlider(hotbarSlotIndex, cooldown));

        ShootArrow(1);
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

        // visible shared cooldown stays same as normal shot
        if (hotbar != null)
            hotbar.StartCoroutine(hotbar.cooldownSlider(hotbarSlotIndex, cooldown));

        ShootArrow(StatsManager.Instance.bowArrowsPerShot);
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
            baseDirection += Vector3.down * 0.1f;
            baseDirection.Normalize();

            Vector3 shotDirection =
                Quaternion.AngleAxis(angle, arrowSpawn.up) * baseDirection;

            Vector3 spawnPosition =
                arrowSpawn.position +
                arrowSpawn.forward * forwardSpawnOffset +
                arrowSpawn.right * (centerOffset * sidewaysSpawnOffset);

            SpawnArrow(spawnPosition, shotDirection);
        }

        if (audioSource != null && shootSound != null)
            audioSource.PlayOneShot(shootSound);
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
        if (arrowProjectile != null && StatsManager.Instance != null)
        {
            arrowProjectile.damage = StatsManager.Instance.bowDamage;
            arrowProjectile.remainingPierces = StatsManager.Instance.bowPierceCount;
        }

        Collider arrowCol = arrowInstance.GetComponent<Collider>();
        Collider bowCol = GetComponent<Collider>();

        if (arrowCol != null && bowCol != null)
        {
            Physics.IgnoreCollision(arrowCol, bowCol);
        }
    }
}