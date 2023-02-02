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
    private GameObject impactFX;
    private GameObject explodeFX;
    private GameObject muzzleFlashFX;
    private GameObject fragmentFX;
    private UnityEngine.ParticleSystem.MainModule main;
    private UnityEngine.ParticleSystem.ShapeModule sh;

    void Start() {
      laserEffect = GetComponent<LineRenderer>();
      laser = laserEffect.gameObject.GetComponent<Renderer>();
      cam = GetComponent<Camera>();
      Cursor.lockState = CursorLockMode.Locked;
      gun = gunMuzzle.parent;
      impactFX = gunMuzzle.GetChild(0).gameObject;
      fragmentFX = gunMuzzle.GetChild(1).gameObject;
      explodeFX = gunMuzzle.GetChild(2).gameObject;
      muzzleFlashFX = gunMuzzle.GetChild(3).gameObject;
      main = explodeFX.GetComponent<ParticleSystem>().main;
      sh = explodeFX.GetComponent<ParticleSystem>().shape;
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
      Vector3 origin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, cam.nearClipPlane));
      RaycastHit hit;
      laserEffect.SetPosition(0, gunMuzzle.position);
      ShootFX();
      if (Physics.Raycast(origin, cam.transform.forward, out hit)) {
        laserEffect.SetPosition(1, hit.point);
        impactFX.transform.position = hit.point;
        impactFX.GetComponent<ParticleSystem>().Play();
        if (hit.collider.gameObject.CompareTag("Target")) {
          HitTarget(hit);
        }
      }
      else {
        laserEffect.SetPosition(1, origin + (cam.transform.forward * 30));
      }
    }

    public void HitTarget(RaycastHit hit) {
      StartCoroutine(ExplodeTarget(hit.collider.gameObject));
      return;
    }

    private IEnumerator ExplodeTarget(GameObject target) {
      float timer = 0f;
      float explodeTime = 0.12f;
      MeshRenderer targetMesh = target.GetComponent<MeshRenderer>();
      Vector3 initScale = target.transform.localScale;
      while (timer < explodeTime) {
        target.transform.localScale = Vector3.Lerp(initScale, new Vector3(0, 0, 0), Mathf.SmoothStep(0f, 1f, timer / explodeTime));
        timer += Time.deltaTime;
        yield return null;
      }
      sh.mesh = target.GetComponent<MeshFilter>().mesh;
      sh.scale = target.transform.localScale;
      main.startSizeMultiplier = 0.25f;
      explodeFX.transform.position = target.transform.position;
      fragmentFX.transform.position = explodeFX.transform.position;
      explodeFX.GetComponent<ParticleSystem>().Play();
      fragmentFX.GetComponent<ParticleSystem>().Play();
      //explodeFX.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
      Destroy(target);
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
      laserEffect.startWidth = 0f;
      laserEffect.endWidth = laserEffect.startWidth;
      while (timer < durIn) {
        laserEffect.startWidth = Mathf.Lerp(0f, 0.25f, timer / durIn);
        laserEffect.endWidth = laserEffect.startWidth;
        laser.material.color = Color.Lerp(colout, colin, timer / durIn);
        timer += Time.deltaTime;
        yield return null;
      }
      timer = 0f;
      laser.material.color = colin;
      laserEffect.startWidth = 0.25f;
      laserEffect.endWidth = laserEffect.startWidth;
      while (timer < durOut) {
        laserEffect.startWidth = Mathf.Lerp(0.25f, 0f, timer / durOut);
        laserEffect.endWidth = laserEffect.startWidth;
        laser.material.color = Color.Lerp(colin, colout, timer / durOut);
        timer += Time.deltaTime;
        yield return null;
      }
      laserEffect.startWidth = 0f;
      laserEffect.endWidth = laserEffect.startWidth;
      laser.material.color = colout;
      laserEffect.enabled = false;
    }

    private void ShootFX() {
      StopCoroutine("LaserFX");
      StopCoroutine("Recoil");
      StartCoroutine(LaserFX());
      StartCoroutine(Recoil());
      muzzleFlashFX.GetComponent<ParticleSystem>().Play();
    }
}
