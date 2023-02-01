using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    [SerializeField]
    private Transform gunMuzzle;
    [SerializeField]
    private float fireRate;

    private LineRenderer laserEffect;
    private Camera cam;
    private float canFireTime;
    private Transform gun;
    private Renderer laser;

    void Start() {
      laserEffect = GetComponent<LineRenderer>();
      laser = laserEffect.gameObject.GetComponent<Renderer>();
      cam = GetComponent<Camera>();
      Cursor.lockState = CursorLockMode.Locked;
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
      ShootFX();   
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
        laserEffect.SetPosition(1, origin + (cam.transform.forward * 30));
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

    private IEnumerator LaserFX() {
      float timer = 0f;
      float durIn = 0.08f;
      float durOut = 0.1f;
      Color colin = Color.white;
      Color colout = Color.clear;
      laserEffect.enabled = true;
      laser.material.color = colout;
      while (timer < durIn) {
        laser.material.color = Color.Lerp(colout, colin, timer / durIn);
        timer += Time.deltaTime;
        yield return null;
      }
      timer = 0f;
      laser.material.color = colin;
      while (timer < durOut) {
        laser.material.color = Color.Lerp(colin, colout, timer / durOut);
        timer += Time.deltaTime;
        yield return null;
      }
      laser.material.color = colout;
      laserEffect.enabled = false;
    }

    private void ShootFX() {
      StartCoroutine(LaserFX());
      StartCoroutine(Recoil());
      gunMuzzle.gameObject.GetComponent<ParticleSystem>().Play();
    }
}