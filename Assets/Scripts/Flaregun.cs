using UnityEngine;
using System.Collections;

public class Flaregun : ToolBase
{
    [Header("References")]
    public Rigidbody flareBullet;
    public Transform barrelEnd;
    public GameObject muzzleParticles;
    public HotbarManager hotbar;

    [Header("Audio")]
    public AudioClip flareShotSound;
    public AudioClip noAmmoSound;
    public AudioClip reloadSound;

    [Header("Stats")]
    public float bulletSpeed = 20f;
    public float maxAimDistance = 200f;

    [Header("Ammo")]
    public int maxSpareRounds = 5;
    public int spareRounds = 3;
    public int currentRound = 3;

    [Header("Aiming")]
    public Camera playerCamera;
    public LayerMask aimLayers = ~0;

    [Header("Hotbar")]
    [SerializeField] private int hotbarSlotIndex = 2;

    private Animation anim;
    private AudioSource audioSource;
    private bool equipped = false;

    void Start()
    {
        cooldown = 5f;
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
        if (!equipped) return;

        if (Input.GetButtonDown("Fire1") && (anim == null || !anim.isPlaying))
        {
            Use();
        }

        if (Input.GetKeyDown(KeyCode.R) && (anim == null || !anim.isPlaying))
        {
            Reload();
        }
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

    public override void Use()
    {
        if (!equipped)
            return;

        if (!CanUse())
            return;

        if (currentRound > 0)
        {
            last_use_time = Time.time;

            if (hotbar != null)
            {
                hotbar.StartCoroutine(hotbar.cooldownSlider(hotbarSlotIndex, cooldown));
            }

            Shoot();
        }
        else
        {
            if (anim != null)
                anim.Play("noAmmo");

            if (audioSource != null && noAmmoSound != null)
                audioSource.PlayOneShot(noAmmoSound);
        }
    }

    void Shoot()
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

        currentRound = Mathf.Max(0, currentRound - 1);

        if (anim != null)
            anim.CrossFade("Shoot");

        if (audioSource != null && flareShotSound != null)
            audioSource.PlayOneShot(flareShotSound);

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
        }

        if (muzzleParticles != null)
        {
            Instantiate(muzzleParticles, barrelEnd.position, barrelEnd.rotation);
        }
    }

    void Reload()
    {
        if (!equipped)
            return;

        if (spareRounds >= 1 && currentRound == 0)
        {
            if (audioSource != null && reloadSound != null)
                audioSource.PlayOneShot(reloadSound);

            spareRounds--;
            currentRound++;

            if (anim != null)
                anim.CrossFade("Reload");
        }
    }
}