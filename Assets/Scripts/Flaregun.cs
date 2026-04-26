using UnityEngine;
using System.Collections;

public class Flaregun : ToolBase
{
    [Header("References")]
    public Rigidbody flareBullet;
    public Transform barrelEnd;
    public HotbarManager hotbar;

    [Header("Audio")]
    public AudioClip flareShotSound;

    [Header("Stats")]
    public float bulletSpeed = 20f;
    public float maxAimDistance = 200f;

    [Header("Aiming")]
    public Camera playerCamera;
    public LayerMask aimLayers = ~0;

    [Header("Hotbar")]
    [SerializeField] private int hotbarSlotIndex = 2;

    [Header("Shared Visible Cooldown")]
    [SerializeField] private float flareHotbarCooldown = 5f;
    
    [Header("Ability Slot UI")]
    public Sprite blueFlareIcon;

    private Animation anim;
    private AudioSource audioSource;
    private bool equipped = false;

    private float last_blue_flare_time = -999f;

    void Start()
    {
        cooldown = flareHotbarCooldown;
        last_use_time = -cooldown;

        anim = GetComponent<Animation>();
        audioSource = GetComponent<AudioSource>();

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        if (hotbar == null)
        {
            hotbar = FindObjectOfType<HotbarManager>();
        }
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
        {
            Use();
        }

        if (Input.GetMouseButton(1))
        {
            if(StatsManager.Instance != null && StatsManager.Instance.blueFlareUnlocked)
            {
                UseBlueFlare();
            }
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

    public override void Use()
    {
        if (!equipped) return;

        if (hotbar != null && hotbar.IsOnCooldown(hotbarSlotIndex)) return;

        Shoot(false);

        if (hotbar != null)
        {
            hotbar.StartCoroutine(hotbar.cooldownSlider(hotbarSlotIndex, cooldown));
        }
    }

    void UseBlueFlare()
    {
        if (!equipped) return;

        if (hotbar != null && hotbar.IsOnCooldown(hotbarSlotIndex)) return;

        if (StatsManager.Instance == null) return;
        if (!StatsManager.Instance.blueFlareUnlocked) return;

        if (Time.time < last_blue_flare_time + StatsManager.Instance.blueFlareCooldown)
            return;

        last_blue_flare_time = Time.time;

        Shoot(true);

        if (hotbar != null)
        {
            hotbar.StartCoroutine(hotbar.cooldownSlider(hotbarSlotIndex, cooldown));
        }
    }

    void Shoot(bool isBlueFlare)
    {
        if (barrelEnd == null)
        {
            Debug.LogError("Flaregun ERROR: barrelEnd is not assigned!");
            return;
        }

        if (flareBullet == null)
        {
            Debug.LogError("Flaregun ERROR: flareBullet prefab is missing!");
            return;
        }

        if (anim != null)
            anim.CrossFade("Shoot");

        if (audioSource != null && flareShotSound != null)
            audioSource.PlayOneShot(flareShotSound, 0.25f);

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit, maxAimDistance, aimLayers))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(maxAimDistance);
        }

        Rigidbody bulletInstance = Instantiate(flareBullet, barrelEnd.position, Quaternion.identity);

        Vector3 shootDirection = (targetPoint - barrelEnd.position).normalized;

        bulletInstance.rotation = Quaternion.LookRotation(shootDirection);
        bulletInstance.velocity = shootDirection * bulletSpeed;

        FlareBullet flareScript = bulletInstance.GetComponent<FlareBullet>();
        if (flareScript != null)
        {
            flareScript.targetPoint = targetPoint;
            flareScript.isBlueFlare = isBlueFlare;
        }
    }

    void UpdateAbilityUI()
    {
        if (hotbar == null || StatsManager.Instance == null) return;

        bool show = equipped && StatsManager.Instance.blueFlareUnlocked;

        if (!show)
        {
            hotbar.HideAbilityCooldown();
            return;
        }

        float cooldown = StatsManager.Instance.blueFlareCooldown;
        float timeRemaining = Mathf.Max(0f, (last_blue_flare_time + cooldown) - Time.time);

        hotbar.ShowAbilityCooldown(blueFlareIcon, timeRemaining, cooldown);
    }

    void HideAbilityUI()
    {
        if (hotbar != null)
            hotbar.HideAbilityCooldown();
    }
}