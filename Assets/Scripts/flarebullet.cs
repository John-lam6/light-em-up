using System.Collections;
using UnityEngine;

public class FlareBullet : MonoBehaviour
{
    private Light flarelight;
    private AudioSource flaresound;
    private ParticleSystemRenderer smokeParSystem;
    private Rigidbody rb;

    private bool burning = true;
    private bool landed = false;

    private float smooth = 2.4f;

    public float flareTimer = 7f;
    public float radius = 10f;
    public float landedLightIntensity = 1f;
    public AudioClip flareBurningSound;

    public Vector3 targetPoint;
    public float moveSpeed = 20f;

    void Start()
    {
        flarelight = GetComponent<Light>();
        flaresound = GetComponent<AudioSource>();
        smokeParSystem = GetComponent<ParticleSystemRenderer>();
        rb = GetComponent<Rigidbody>();

        if (flarelight != null)
        {
            flarelight.range = 0f;
            flarelight.intensity = 0f;
            flarelight.color = new Color(1f, 0.35f, 0f); // deeper orange
        }

        if (flaresound != null && flareBurningSound != null)
        {
            flaresound.PlayOneShot(flareBurningSound);
        }

        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        Destroy(gameObject, flareTimer + 2f);
    }

    void Update()
    {
        if (!landed)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);

            Vector3 direction = targetPoint - transform.position;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }

            if (Vector3.Distance(transform.position, targetPoint) < 0.05f)
            {
                Land();
            }
        }

        if (landed && burning)
        {
            if (flarelight != null)
            {
                flarelight.intensity = Mathf.Lerp(flarelight.intensity, landedLightIntensity, Time.deltaTime * smooth);
                flarelight.range = Mathf.Lerp(flarelight.range, radius, Time.deltaTime * smooth);
            }
        }
        else
        {
            if (flarelight != null)
            {
                flarelight.intensity = Mathf.Lerp(flarelight.intensity, 0f, Time.deltaTime * smooth);
                flarelight.range = Mathf.Lerp(flarelight.range, 0f, Time.deltaTime * smooth);
            }

            if (flaresound != null)
            {
                flaresound.volume = Mathf.Lerp(flaresound.volume, 0f, Time.deltaTime * smooth);
            }

            if (smokeParSystem != null)
            {
                smokeParSystem.maxParticleSize = Mathf.Lerp(smokeParSystem.maxParticleSize, 0f, Time.deltaTime * 5f);
            }
        }
    }

    void Land()
    {
        landed = true;
        transform.position = targetPoint;
        StartCoroutine(FlareLife());
    }

    IEnumerator FlareLife()
    {
        burning = true;
        yield return new WaitForSeconds(flareTimer);
        burning = false;
    }
}