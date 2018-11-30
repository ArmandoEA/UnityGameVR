using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastShoot : MonoBehaviour {

    public int gunDamage = 1;
    public float fireRate = 0.25f;
    public float weaponRange = 50f;
    public float hitForece = 100f;
    public Transform gunEnd;
    public FPController fpc;
    public Material[] material;

    private Camera fpsCam;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.05f); 
    private AudioSource gunAudio;
    private LineRenderer laserLine;
    private float nextFire;

	void Start () {
        laserLine = GetComponent<LineRenderer>();
        laserLine.sharedMaterial = material[0];
        gunAudio = GetComponent<AudioSource>();
        fpsCam = GetComponentInParent<Camera>();
	}
	
	void Update () {
        laserLine.SetPosition(0, gunEnd.position);
        laserLine.SetPosition(1, fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0)) + (fpsCam.transform.forward * weaponRange));
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            StartCoroutine(ShotEffect());

            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            laserLine.SetPosition(0, gunEnd.position);

            if(Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
            {
                laserLine.SetPosition(1, hit.point);

                ShootableBox health = hit.collider.GetComponent<ShootableBox>();
                if(health != null)
                {
                    health.Damage(gunDamage);
                    fpc.ModCount();
                }
                /*if(hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * hitForece);
                }*/
            }
            else
            {
                laserLine.SetPosition(1, rayOrigin + (fpsCam.transform.forward * weaponRange));
            }
        }
	}

    private IEnumerator ShotEffect()
    {
        gunAudio.Play();

        //laserLine.enabled = true;
        laserLine.sharedMaterial = material[1];
        yield return shotDuration;
        laserLine.sharedMaterial = material[0];
        //laserLine.enabled = false;
    }
}
