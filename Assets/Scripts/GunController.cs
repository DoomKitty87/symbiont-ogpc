using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    [SerializeField]
    private Transform gunMuzzle;
    [SerializeField]
    private float fxPeriod;
    [SerializeField]
    private float fireRate;

    private LineRenderer laserEffect;
    private Camera cam;
    private float canFireTime;
    private WaitForSeconds shotDelay;
    private Transform gun;

    void Start() {
      laserEffect = GetComponent<LineRenderer>();
      cam = GetComponent<Camera>();
      Cursor.lockState = CursorLockMode.Locked;
      shotDelay = new WaitForSeconds(fxPeriod);
      gun = gunMuzzle.parent;
      //Cursor.visible = false;
    }

    void Update()
    {
      if (Input.GetMouseButtonDown(0)) {
        Shoot();
      }
    }

    private bool CanShoot() {
      if (Time.time > canFireTime) return true;
      else return false;
    }

    private void Shoot() {
      if (!CanShoot()) return;
      canFireTime = Time.time + fireRate;
      StartCoroutine(ShootFX());
      StartCoroutine(Recoil());
      Vector3 origin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, cam.nearClipPlane));
      RaycastHit hit;
      laserEffect.SetPosition(0, gunMuzzle.position);
      if (Physics.Raycast(origin, cam.transform.forward, out hit)) {
        laserEffect.SetPosition(1, hit.point);
        if (hit.collider.gameObject.CompareTag("Target")) {
          HitTarget(hit);
        }
      }
      else {
        laserEffect.SetPosition(1, origin + (cam.transform.forward * 10));
      }
    }

    public void HitTarget(RaycastHit hit) {
      return;
    }

    private IEnumerator Recoil() {
        float timer = 0f;
        float rcUp = 0.09f;
        float rcDown = 0.125f;
        Vector3 initial = gun.localPosition;
        while (timer < rcUp) {
            gun.localRotation = Quaternion.Lerp(Quaternion.Euler(0, -90f, 0f), Quaternion.Euler(0, -90f, 40f), timer / rcUp);
            gun.localPosition = Vector3.Lerp(initial, new Vector3(initial.x, initial.y, initial.z - 0.3f), timer / rcUp);
            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0f;
        gun.localPosition = new Vector3(initial.x, initial.y, initial.z - 0.3f);
        gun.localRotation = Quaternion.Euler(0f, -90f, 70f);
        Vector3 initialnew = gun.localPosition;
        while (timer < rcDown) {
            gun.localRotation = Quaternion.Lerp(Quaternion.Euler(0, -90f, 40f), Quaternion.Euler(0, -90f, 0f), timer / rcDown);
            gun.localPosition = Vector3.Lerp(initialnew, new Vector3(initial.x, initial.y, initial.z), timer / rcDown);
            timer += Time.deltaTime;
            yield return null;
        }
        gun.localPosition = new Vector3(initial.x, initial.y, initial.z);
        gun.localRotation = Quaternion.Euler(0f, -90f, 0f);
    }

    private IEnumerator ShootFX() {
      laserEffect.enabled = true;
      yield return shotDelay;
      laserEffect.enabled = false;
    }
}
/*
using UnityEngine;
using System.Collections;

public class RaycastShootComplete : MonoBehaviour {

    public int gunDamage = 1;                                            // Set the number of hitpoints that this gun will take away from shot objects with a health script
    public float fireRate = 0.25f;                                        // Number in seconds which controls how often the player can fire
    public float weaponRange = 50f;                                        // Distance in Unity units over which the player can fire
    public float hitForce = 100f;                                        // Amount of force which will be added to objects with a rigidbody shot by the player
    public Transform gunEnd;                                            // Holds a reference to the gun end object, marking the muzzle location of the gun

    private Camera fpsCam;                                                // Holds a reference to the first person camera
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);    // WaitForSeconds object used by our ShotEffect coroutine, determines time laser line will remain visible
    private AudioSource gunAudio;                                        // Reference to the audio source which will play our shooting sound effect
    private LineRenderer laserLine;                                        // Reference to the LineRenderer component which will display our laserline
    private float nextFire;                                                // Float to store the time the player will be allowed to fire again, after firing


    void Start ()
    {
        // Get and store a reference to our LineRenderer component
        laserLine = GetComponent<LineRenderer>();

        // Get and store a reference to our AudioSource component
        gunAudio = GetComponent<AudioSource>();

        // Get and store a reference to our Camera by searching this GameObject and its parents
        fpsCam = GetComponentInParent<Camera>();
    }


    void Update ()
    {
        // Check if the player has pressed the fire button and if enough time has elapsed since they last fired
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            // Update the time when our player can fire next
            nextFire = Time.time + fireRate;

            // Start our ShotEffect coroutine to turn our laser line on and off
            StartCoroutine (ShotEffect());

            // Create a vector at the center of our camera's viewport
            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));

            // Declare a raycast hit to store information about what our raycast has hit
            RaycastHit hit;

            // Set the start position for our visual effect for our laser to the position of gunEnd
            laserLine.SetPosition (0, gunEnd.position);

            // Check if our raycast has hit anything
            if (Physics.Raycast (rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
            {
                // Set the end position for our laser line
                laserLine.SetPosition (1, hit.point);

                // Get a reference to a health script attached to the collider we hit
                ShootableBox health = hit.collider.GetComponent<ShootableBox>();

                // If there was a health script attached
                if (health != null)
                {
                    // Call the damage function of that script, passing in our gunDamage variable
                    health.Damage (gunDamage);
                }

                // Check if the object we hit has a rigidbody attached
                if (hit.rigidbody != null)
                {
                    // Add force to the rigidbody we hit, in the direction from which it was hit
                    hit.rigidbody.AddForce (-hit.normal * hitForce);
                }
            }
            else
            {
                // If we did not hit anything, set the end of the line to a position directly in front of the camera at the distance of weaponRange
                laserLine.SetPosition (1, rayOrigin + (fpsCam.transform.forward * weaponRange));
            }
        }
    }


    private IEnumerator ShotEffect()
    {
        // Play the shooting sound effect
        gunAudio.Play ();

        // Turn on our line renderer
        laserLine.enabled = true;

        //Wait for .07 seconds
        yield return shotDuration;

        // Deactivate our line renderer after waiting
        laserLine.enabled = false;
    }
}
*/
