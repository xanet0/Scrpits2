using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float speedFire = 15f;
    public float impactForce = 30f;
    public float ammoGive = 270f;
    public float ammoRifle = 30f;
    private float nextTimeToFire = 0f;

    public Camera cam;
    public ParticleSystem fire;
    public GameObject impactEffect;
    public AudioSource sound;
    public AudioSource soundEnd;
    public AudioSource ammoEnd;
    public TextMeshProUGUI ammoText;

    private void Start()
    {
        ammoText.text = ammoRifle + "";
    }
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / speedFire;
            Shoot();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            fire.Stop();
            sound.Stop();
            if (ammoRifle != 0)
            {
                soundEnd.Play();
            }
            
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            if (ammoRifle > 0)
            {
                ammoRifle = ammoRifle - 1;
                ammoText.text = "" + ammoRifle;
                Target target = hit.transform.GetComponent<Target>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                }

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                }

                if (Input.GetButtonDown("Fire1"))
                {
                    sound.Play();
                }


                GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGo, 0.5f);
            }
            if (ammoRifle == 0)
            {
                Reload();
            }


        }
    }
    void Reload()
    {
        fire.Stop();
        sound.Stop();
        ammoEnd.Play();
    }
}
