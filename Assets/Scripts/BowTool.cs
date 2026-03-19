using UnityEngine;

public class BowTool : RangedTool
{
    // REFERENCES
    public GameObject arrowPrefab;     // Prefab for the arrow we spawn
    public Transform arrowSpawn;       // Where the arrow comes out of (tip of bow)

    // AUDIO
    public AudioClip shootSound;       // Sound when shooting
    public AudioClip noAmmoSound;      // Sound when out of arrows

    // AMMO
    [Header("Ammo")]
    public int currentArrows = 20;     // How many arrows we currently have

    // HOTBAR
    [Header("Hotbar")]
    [SerializeField] private int hotbarSlotIndex = 1; // Which slot this bow is in

    // BASE STATS
    // These are the default values before any upgrades
    [Header("Base Bow Stats")]
    [SerializeField] private int baseArrowsPerShot = 1;
    [SerializeField] private int basePierceCount = 0;
    [SerializeField] private float baseArrowDamage = 10f;
    [SerializeField] private float baseAngleBetweenArrows = 0f;

    // CURRENT STATS
    // These change during gameplay with upgrades
    [Header("Current Bow Stats")]
    [SerializeField] private int arrowsPerShot;
    [SerializeField] private int pierceCount;
    [SerializeField] private float arrowDamage;
    [SerializeField] private float angleBetweenArrows;

    // UPGRADE VALUES
    // How much each upgrade increases stats
    [Header("Upgrade Amounts")]
    [SerializeField] private int arrowsPerShotUpgradeAmount = 1;
    [SerializeField] private float angleBetweenArrowsUpgradeAmount = 2f;
    [SerializeField] private int pierceUpgradeAmount = 1;
    [SerializeField] private float damageUpgradeAmount = 5f;

    // SPAWN TUNING
    // Small offsets so arrows don't spawn inside each other
    [Header("Arrow Spawn Tuning")]
    [SerializeField] private float forwardSpawnOffset = 0.6f;
    [SerializeField] private float sidewaysSpawnOffset = 0.08f;

    private AudioSource audioSource;   // Used to play sounds

    // START
    protected override void Start()
    {
        base.Start();

        // Set cooldown between shots
        cooldown = 0.75f;
        last_use_time = -cooldown;

        audioSource = GetComponent<AudioSource>();

        // Reset all stats to base values at start
        ResetAllUpgrades();
    }

    // UPDATE
    void Update()
    {
        if (!equipped) return; // Only work if bow is equipped

        // Left click → shoot
        if (Input.GetButtonDown("Fire1"))
            Use();

        // Debug/testing upgrade keys
        if (Input.GetKeyDown(KeyCode.U))
            IncreaseMultiShotUpgrade();

        if (Input.GetKeyDown(KeyCode.I))
            IncreasePierceCount(pierceUpgradeAmount);

        if (Input.GetKeyDown(KeyCode.O))
            IncreaseDamage(damageUpgradeAmount);

        if (Input.GetKeyDown(KeyCode.P))
            ResetAllUpgrades();
    }

    // EQUIP
    public override void Equip()
    {
        equipped = true;
        gameObject.SetActive(true);
    }

    // UNEQUIP
    public override void Unequip()
    {
        equipped = false;
        gameObject.SetActive(false);
    }

    // USE (SHOOT)
    public override void Use()
    {
        if (!equipped) return;
        if (!CanUse()) return;

        // Check if out of ammo
        if (currentArrows <= 0)
        {
            if (audioSource != null && noAmmoSound != null)
                audioSource.PlayOneShot(noAmmoSound);
            return;
        }

        // Safety checks so game doesn't crash
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

        // Update cooldown + ammo
        last_use_time = Time.time;
        currentArrows--;

        // Start cooldown UI on hotbar
        if (hotbar != null)
            hotbar.StartCoroutine(hotbar.cooldownSlider(hotbarSlotIndex, cooldown));

        ShootArrow();
    }

    // SHOOT LOGIC
    void ShootArrow()
    {
        int totalArrowsToShoot = Mathf.Max(1, arrowsPerShot);
        Debug.Log("Shooting " + totalArrowsToShoot + " arrows");

        for (int i = 0; i < totalArrowsToShoot; i++)
        {
            // Center arrows properly (so spread is symmetric)
            float centerOffset = i - (totalArrowsToShoot - 1) / 2f;

            // Calculate angle for spread
            float angle = centerOffset * angleBetweenArrows;

            // Base direction = forward from spawn point
            Vector3 baseDirection = arrowSpawn.forward;

            // Slight downward tilt so arrows don't fly perfectly straight
            baseDirection += Vector3.down * 0.1f;

            baseDirection.Normalize();

            // Apply spread rotation
            Vector3 shotDirection =
                Quaternion.AngleAxis(angle, arrowSpawn.up) * baseDirection;

            // Offset spawn position slightly so arrows don't overlap
            Vector3 spawnPosition =
                arrowSpawn.position +
                arrowSpawn.forward * forwardSpawnOffset +
                arrowSpawn.right * (centerOffset * sidewaysSpawnOffset);

            SpawnArrow(spawnPosition, shotDirection);
        }

        // Play shooting sound
        if (audioSource != null && shootSound != null)
            audioSource.PlayOneShot(shootSound);
    }

    // SPAWN ARROW
    void SpawnArrow(Vector3 spawnPosition, Vector3 shotDirection)
    {
        // Create arrow instance
        GameObject arrowInstance = Instantiate(
            arrowPrefab,
            spawnPosition,
            Quaternion.LookRotation(shotDirection)
        );

        // Apply velocity to arrow
        Rigidbody rb = arrowInstance.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = shotDirection.normalized * projectile_speed;
        }

        // Set arrow stats (damage + pierce)
        ArrowProjectile arrowProjectile = arrowInstance.GetComponent<ArrowProjectile>();
        if (arrowProjectile != null)
        {
            arrowProjectile.damage = arrowDamage;
            arrowProjectile.remainingPierces = pierceCount;
        }

        // Prevent arrow from colliding with player/bow immediately
        Collider arrowCol = arrowInstance.GetComponent<Collider>();
        Collider bowCol = GetComponent<Collider>();

        if (arrowCol != null && bowCol != null)
        {
            Physics.IgnoreCollision(arrowCol, bowCol);
        }
    }

    // UPGRADES
    
    // Increase number of arrows shot + spread
    public void IncreaseMultiShotUpgrade()
    {
        arrowsPerShot += arrowsPerShotUpgradeAmount;
        arrowsPerShot = Mathf.Max(1, arrowsPerShot);

        if (arrowsPerShot > 1)
            angleBetweenArrows += angleBetweenArrowsUpgradeAmount;

        Debug.Log("Bow upgraded: arrowsPerShot = " + arrowsPerShot +
                  ", angleBetweenArrows = " + angleBetweenArrows);
    }

    // Increase how many enemies arrow can pass through
    public void IncreasePierceCount(int amount)
    {
        pierceCount += amount;
        pierceCount = Mathf.Max(0, pierceCount);
    }

    // Increase arrow damage
    public void IncreaseDamage(float amount)
    {
        arrowDamage += amount;
        arrowDamage = Mathf.Max(0f, arrowDamage);
    }

    // Reset everything back to base stats
    public void ResetAllUpgrades()
    {
        arrowsPerShot = baseArrowsPerShot;
        pierceCount = basePierceCount;
        arrowDamage = baseArrowDamage;
        angleBetweenArrows = baseAngleBetweenArrows;

        Debug.Log("Bow reset: arrowsPerShot = " + arrowsPerShot +
                  ", angleBetweenArrows = " + angleBetweenArrows);
    }
}