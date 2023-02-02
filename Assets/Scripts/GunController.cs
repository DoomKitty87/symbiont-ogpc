using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunController : MonoBehaviour
{

    [SerializeField]
    private Transform gunMuzzle;
    [SerializeField]
    private float fireRate;
    [SerializeField]
    private int maxRounds;
    [SerializeField]
    private GameObject magazine;
    [SerializeField]
    private GameObject maginfo;

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
    private int rounds;
    private bool reloading;
    private TextMeshProUGUI magtext;
    private GameObject reloadtext;

    void Start() {
      rounds = maxRounds;
      reloadtext = maginfo.transform.GetChild(1).gameObject;
      magtext = maginfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
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
      if (rounds == 0) {
        Reload();
        return false;
      }
      else if (Time.time > canFireTime && reloading == false) return true;
      else return false;
    }

    public void Reload() {
        if (rounds < maxRounds) {
            rounds = maxRounds;
            reloadtext.SetActive(true);
            reloading = true;
            StartCoroutine(ReloadAnim());
        }
    }

    private void Shoot() {
      if (!CanShoot()) return;
      rounds -= 1;
      magtext.text = rounds.ToString();
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

    private IEnumerator ReloadAnim() {
        float timer = 0f;
        float inTime = 0.35f;
        float outTime = 0.3f;
        float popUpTime = 0.1f;
        float popDownTime = 0.125f;
        Vector3 init = magazine.transform.localPosition;
        while (timer < outTime) {
            if (timer / outTime <= 0.15f) gun.localRotation = Quaternion.Lerp(Quaternion.Euler(0, -90f, 0), Quaternion.Euler(-20f, -90f, 0), timer / outTime / 0.15f);
            else gun.localRotation = Quaternion.Lerp(Quaternion.Euler(-20, -90f, 0), Quaternion.Euler(0, -90f, 0), (timer / outTime - 0.15f) / 0.85f);
            magazine.transform.localPosition = Vector3.Lerp(init, new Vector3(init.x, init.y - 1.5f, init.z), Mathf.SmoothStep(0f, 1f, timer / outTime));
            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0f;
        float popUpTimer = 0f;
        float popDownTimer = 0f;
        while (timer < inTime) {
            magazine.transform.localPosition = Vector3.Lerp(new Vector3(init.x, init.y - 1.5f, init.z), init, Mathf.SmoothStep(0f, 1f, timer / inTime));
            gun.localRotation = Quaternion.Lerp(Quaternion.Euler(0, -90f, 0), Quaternion.Euler(0, -90f, 5f), popUpTimer / popUpTime);
            if (timer / inTime >= 0.65f) popUpTimer += Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
        while (popUpTimer < popUpTime) {
            gun.localRotation = Quaternion.Lerp(Quaternion.Euler(0, -90f, 0), Quaternion.Euler(0, -90f, 5f), popUpTimer / popUpTime);
            popUpTimer += Time.deltaTime;
            yield return null;
        }
        gun.localRotation = Quaternion.Euler(0, -90f, 5);
        while (popDownTimer < popDownTime) {
            gun.localRotation = Quaternion.Lerp(Quaternion.Euler(0, -90f, 5f), Quaternion.Euler(0, -90f, 0), popDownTimer / popDownTime);
            popDownTimer += Time.deltaTime;
            yield return null;
        }
        gun.localRotation = Quaternion.Euler(0, -90f, 0);
        magazine.transform.localPosition = init;
        magtext.text = rounds.ToString();
        reloadtext.SetActive(false);
        reloading = false;
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
