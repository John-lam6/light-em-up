using UnityEngine;
using System.Collections;

public class Flaregun : ToolBase
{
    public Rigidbody flareBullet;
    public Transform barrelEnd;
    public GameObject muzzleParticles;
    public AudioClip flareShotSound;
    public AudioClip noAmmoSound;
    public AudioClip reloadSound;

    public float bulletSpeed = 20f;
    public float maxAimDistance = 200f;

    public int maxSpareRounds = 5;
    public int spareRounds = 3;
    public int currentRound = 3;

    public Camera playerCamera;
    public LayerMask aimLayers = ~0;

    void Start()
    {
        cooldown = 0.8f;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !GetComponent<Animation>().isPlaying)
        {
            Use();
        }

        if (Input.GetKeyDown(KeyCode.R) && !GetComponent<Animation>().isPlaying)
        {
            Reload();
        }
    }

    public override void Equip()
    {
    }

    public override void Unequip()
    {
    }

    public override void Use()
    {
        if (!CanUse())
            return;

        if (currentRound > 0)
        {
            Shoot();
            last_use_time = Time.time;
        }
        else
        {
            GetComponent<Animation>().Play("noAmmo");
            GetComponent<AudioSource>().PlayOneShot(noAmmoSound);
        }
    }

    void Shoot()
    {
        currentRound--;

        if (currentRound < 0)
        {
            currentRound = 0;
        }

        GetComponent<Animation>().CrossFade("Shoot");
        GetComponent<AudioSource>().PlayOneShot(flareShotSound);

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
        if (spareRounds >= 1 && currentRound == 0)
        {
            GetComponent<AudioSource>().PlayOneShot(reloadSound);
            spareRounds--;
            currentRound++;
            GetComponent<Animation>().CrossFade("Reload");
        }
    }
}