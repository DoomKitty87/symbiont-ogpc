using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class GunController : MonoBehaviour
{

  [SerializeField] private float fireRate;
  [SerializeField] private int maxRounds;
  [SerializeField] private GameObject HUDCanvas;
  [SerializeField] private GameObject shell;
  [SerializeField] private GameObject ammoInfo;
  [SerializeField] private GameObject laserBeamPrefab;
  [SerializeField][ColorUsageAttribute(true, true)] private Color[] colors;

  private string[] gunNames = {"Pistol", "Assault Rifle", "Heavy Rifle"};

  //Values are (1) Name, (2) Mag size, (3) Fire rate, (4) Shot recoil (up), (5) Recoil Recovery, (6) Shot spread, (7) Shot recoil (back), (8) Reload time, (9) Uses upwards recoil animation, (10) Shot color
  private float[][] gunSpecs = {new float[] {0, 10, 0.5f, 0.2f, 0.4f, 0.5f, 0.3f, 1f, 0, 2}, new float[] {1, 30, 0.125f, 0.05f, 0.5f, 0.8f, 0.09f, 1.5f, 0, 0}, new float[]{2, 24, 0.5f, 0.05f, 0.4f, 0.4f, 0.25f, 2f, 0, 1}};

  private string activeGun = "Pistol";
  private Camera cam;
  private float canFireTime;
  private float holdTimer;
  private float reloadTime;
  private float shotRecoilUp;
  private float shotRecoilBack;
  private float recoilRecovery;
  private float aimSpread;
  private float upRecoilAnim;
  private float vertRecoilTracking;
  private Transform gun;
  private GameObject reloadtext;
  private GameObject leftleg, rightleg;
  private GameObject impactFX;
  private GameObject explodeFX;
  private GameObject muzzleFlashFX;
  private GameObject fragmentFX;
  private GameObject magazine;
  private UnityEngine.ParticleSystem.MainModule main;
  private UnityEngine.ParticleSystem.ShapeModule sh;
  private CinemachineVirtualCamera vcam;
  private CinemachinePOV pov;
  private int rounds;
  private int beamColor;
  private bool reloading;
  private bool shells = false;
  private TextMeshProUGUI magtext;
  private TextMeshProUGUI ammoText;
  private Transform crosshair;
  private Vector3 gunInitPos;
  private Quaternion gunInitRot;
  private Transform gunMuzzle;
  private Vector3 beamInit;
  private AmmoScript ammoScript;

  void Start() {
    ammoText = ammoInfo.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    vcam = GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;
    pov = vcam.GetCinemachineComponent<CinemachinePOV>();
    crosshair = HUDCanvas.transform.GetChild(0);
    leftleg = crosshair.GetChild(1).gameObject;
    rightleg = crosshair.GetChild(2).gameObject;
    rounds = maxRounds;
    cam = GetComponent<Camera>();
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
    gun = transform.GetChild(0);
    beamInit = transform.GetChild(1).GetChild(3).localScale;
    ReloadGunAssets(gunSpecs[0]);
    ammoScript = ammoInfo.GetComponent<AmmoScript>();
	ammoScript.maxAmmo = maxRounds;
  }

  void Update()
  {
    if (vertRecoilTracking > 0 && vertRecoilTracking - recoilRecovery * Time.deltaTime >= 0) pov.m_VerticalAxis.Value += 20f * recoilRecovery * Time.deltaTime;
    else if (vertRecoilTracking > 0) pov.m_VerticalAxis.Value += 20f * ((recoilRecovery * Time.deltaTime) - Mathf.Abs((vertRecoilTracking - recoilRecovery * Time.deltaTime)));
    vertRecoilTracking = Mathf.Clamp(vertRecoilTracking - recoilRecovery * Time.deltaTime, 0, 1);
    if (Input.GetMouseButtonDown(0)) {
      if (activeGun == "Pistol") Shoot();
      holdTimer = 0;
    }
    else if (Input.GetMouseButton(0)) {
      if (activeGun == "Assault Rifle" | activeGun == "Heavy Rifle") Shoot();
      holdTimer += Time.deltaTime;
    }
    if (Input.GetMouseButtonDown(1)) {
      ChangeGun(FetchGunInfo(activeGun));
    }
  }

  private float[] FetchGunInfo(string currentGun) {
    switch(currentGun) {
      case "Pistol":
        return(gunSpecs[1]);
      case "Assault Rifle":
        return(gunSpecs[2]);
      case "Heavy Rifle":
        return(gunSpecs[0]);
      default:
        return(new float[8]);
    }
  }

  private void ReloadGunAssets(float[] gunStats) {
    magazine = gun.GetChild(1).gameObject;
    gunMuzzle = gun.GetChild(2);
    gun.gameObject.SetActive(true);
    gunInitPos = gun.transform.localPosition;
    gunInitRot = gun.transform.localRotation;
    impactFX = gunMuzzle.GetChild(0).gameObject;
    fragmentFX = gunMuzzle.GetChild(1).gameObject;
    explodeFX = gunMuzzle.GetChild(2).gameObject;
    main = explodeFX.GetComponent<ParticleSystem>().main;
    sh = explodeFX.GetComponent<ParticleSystem>().shape;
    muzzleFlashFX = gunMuzzle.GetChild(3).gameObject;
    maxRounds = (int)gunStats[1];
    fireRate = gunStats[2];
    shotRecoilUp = gunStats[3];
    recoilRecovery = gunStats[4];
    aimSpread = gunStats[5];
    shotRecoilBack = gunStats[6];
    reloadTime = gunStats[7];
    upRecoilAnim = gunStats[8];
    beamColor = (int)gunStats[9];
    activeGun = gunNames[(int)gunStats[0]];
  }

  public void ChangeGun(float[] gunStats) {
    StopFX();
    ResetGun();
    transform.GetChild(0).gameObject.SetActive(false);
    transform.GetChild(1).gameObject.SetActive(false);
    transform.GetChild(2).gameObject.SetActive(false);
    gun = transform.GetChild((int)gunStats[0]);
    ReloadGunAssets(gunStats);
    rounds = maxRounds;
    ammoText.text = rounds.ToString() + " | " + maxRounds.ToString();
    ammoScript.maxAmmo = maxRounds;
    vertRecoilTracking = 0f;
	ammoScript.currAmmo = rounds;
    }

  private bool CanShoot() {
    if (rounds == 0) {
      Reload();
      return false;
    }
    else if (Time.time > canFireTime && reloading == false) return true;
    else return false;
  }

  private void ResetGun() {
    gun.transform.localPosition = gunInitPos;
    gun.transform.localRotation = gunInitRot;
  }

  public void Reload() {
    if (rounds < maxRounds) {
      rounds = maxRounds;
      reloading = true;
      StartCoroutine(ReloadAnim());
    }
  }

  private void EjectShell() {
    Rigidbody rb = Instantiate(shell, gun.transform.position, Quaternion.identity, cam.gameObject.transform).GetComponent<Rigidbody>();
    rb.gameObject.transform.localPosition = new Vector3(0.55f, -0.1f, 0.66f);
    rb.gameObject.transform.localRotation = gun.localRotation;
    rb.AddForce(cam.gameObject.transform.localRotation * new Vector3(Random.Range(-1, -0.5f), Random.Range(1, 2), 0),ForceMode.Impulse);
  }

  private void Shoot() {
    if (!CanShoot()) return;
    StopFX();
    rounds -= 1;
    ammoText.text = rounds.ToString() + " | " + maxRounds.ToString();
    ammoScript.currAmmo = rounds;
    canFireTime = Time.time + fireRate;
    Vector3 origin = Quaternion.Euler(holdTimer < fireRate ? Random.Range(-aimSpread / 4, aimSpread / 4) : Random.Range(-aimSpread, aimSpread), holdTimer < fireRate ? Random.Range(-aimSpread / 4, aimSpread / 4) : Random.Range(-aimSpread, aimSpread), 0) * cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, cam.nearClipPlane));
    RaycastHit hit;
    Vector3[] points = new Vector3[2];
    points[0] = gunMuzzle.position;
    if (Physics.Raycast(origin, cam.transform.forward, out hit)) {
      points[1] = hit.point;
      impactFX.transform.position = hit.point;
      impactFX.GetComponent<ParticleSystem>().Play();
      if (hit.collider.gameObject.CompareTag("Target")) HitTarget(hit);
    }
    else {
      points[1] = origin + (cam.transform.forward * 50);
    }
    ShootFX(points);
    vertRecoilTracking = Mathf.Clamp(vertRecoilTracking + shotRecoilUp, 0, 1);
    pov.m_VerticalAxis.Value -= 20f * shotRecoilUp;
  }

  public void HitTarget(RaycastHit hit) {
    GetComponent<PointTracker>().DestroyedTarget(hit.collider.gameObject);
    StartCoroutine(ExplodeTarget(hit.collider.gameObject));
    return;
  }

  private IEnumerator ReloadAnim() {
    float timer = 0f;
    float inTime = 0.35f * reloadTime;
    float outTime = 0.3f * reloadTime;
    float popUpTime = 0.1f * reloadTime;
    float popDownTime = 0.125f * reloadTime;
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
    ammoText.text = rounds.ToString() + " | " + maxRounds.ToString();
    ammoScript.currAmmo = rounds;
    reloading = false;
  }

  private IEnumerator CrosshairFX() {
    float timer = 0f;
    float outTime = 0.15f;
    float inTime = 0.2f;
    while (timer < outTime) {
      leftleg.transform.localPosition = Vector3.Lerp(new Vector3(-30f * (leftleg.transform.localScale.x / 0.5f), 0, 0), new Vector3(-30f - (30 * (shotRecoilUp / 0.2f) * (leftleg.transform.localScale.x / 0.5f)), 0, 0), Mathf.SmoothStep(0f, 1f, timer / outTime));
      rightleg.transform.localPosition = leftleg.transform.localPosition * -1f;
      timer += Time.deltaTime;
      yield return null;
    }
    timer = 0;
    while (timer < inTime) {
      leftleg.transform.localPosition = Vector3.Lerp(new Vector3(-30f - (30 * (shotRecoilUp / 0.2f) * (leftleg.transform.localScale.x / 0.5f)), 0, 0), new Vector3(-30f * (leftleg.transform.localScale.x / 0.5f), 0, 0), Mathf.SmoothStep(0f, 1f, timer / inTime));
      rightleg.transform.localPosition = leftleg.transform.localPosition * -1f;
      timer += Time.deltaTime;
      yield return null;
    }
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

  private IEnumerator RecoilBackOnly() {
    float timer = 0f;
    float rcUp = 0.09f;
    float rcDown = 0.125f;
    Vector3 initial = gun.localPosition;
    while (timer < rcUp) {
      gun.localPosition = Vector3.Lerp(initial, new Vector3(initial.x, initial.y, initial.z - (0.8f * shotRecoilBack)), timer / rcUp);
      timer += Time.deltaTime;
      yield return null;
    }
    timer = 0f;
    gun.localPosition = new Vector3(initial.x, initial.y, initial.z - 0.8f * shotRecoilUp);
    while (timer < rcDown) {
      gun.localPosition = Vector3.Lerp(new Vector3(initial.x, initial.y, initial.z - (0.8f * shotRecoilBack)), initial, timer / rcDown);
      timer += Time.deltaTime;
      yield return null;
    }
    gun.localPosition = initial;
  }

  private IEnumerator Recoil() {
    float timer = 0f;
    float rcUp = 0.09f;
    float rcDown = 0.125f;
    Vector3 initial = gun.localPosition;
    while (timer < rcUp) {
      gun.localRotation = Quaternion.Lerp(Quaternion.Euler(0, -90f , 0f), Quaternion.Euler(0, -90f, (100f * shotRecoilUp)), timer / rcUp);
      gun.localPosition = Vector3.Lerp(initial, new Vector3(initial.x, initial.y, initial.z - (0.8f * shotRecoilBack)), timer / rcUp);
      timer += Time.deltaTime;
      yield return null;
    }
    timer = 0f;
    gun.localPosition = new Vector3(initial.x, initial.y, initial.z - 0.8f * shotRecoilUp);
    gun.localRotation = Quaternion.Euler(0f, -90f, (100f * shotRecoilBack));
    while (timer < rcDown) {
      gun.localRotation = Quaternion.Lerp(Quaternion.Euler(0, -90f, (100f * shotRecoilUp)), Quaternion.Euler(0, -90f, 0f), timer / rcDown);
      gun.localPosition = Vector3.Lerp(new Vector3(initial.x, initial.y, initial.z - (0.8f * shotRecoilBack)), initial, timer / rcDown);
      timer += Time.deltaTime;
      yield return null;
    }
    gun.localPosition = initial;
    gun.localRotation = Quaternion.Euler(0f, -90f, 0f);
  }

  private IEnumerator ChamberCharge() {
    float timer = 0f;
    float durIn = 0.05f;
    float durOut = 0.15f;
    GameObject beam = gun.GetChild(3).gameObject;
    Color colin = Color.white;
    Color colout = Color.clear;
    Material mat = beam.GetComponent<MeshRenderer>().material;
    beam.SetActive(true);
    mat.color = colout;
    while (timer < durIn) {
      mat.color = Color.Lerp(colout, colin, timer / durIn);
      beam.transform.localScale = Vector3.Lerp(new Vector3(beamInit.x, 0, 0), beamInit, timer / durIn);
      timer += Time.deltaTime;
      yield return null;
    }
    beam.transform.localScale = beamInit;
    timer = 0f;
    mat.color = colin;
    while (timer < durOut) {
      mat.color = Color.Lerp(colin, colout, timer / durOut);
      beam.transform.localScale = Vector3.Lerp(beamInit, new Vector3(beamInit.x, 0, 0), timer / durOut);
      timer += Time.deltaTime;
      yield return null;
    }
    beam.transform.localScale = beamInit;
    mat.color = colout;
    beam.SetActive(false);
  }

  private IEnumerator ReactorGlow() {
    float timer = 0f;
    float durIn = 0.1f;
    float durOut = 0.15f;
    GameObject reactor = gun.GetChild(0).GetChild(0).gameObject;
    Material mat = reactor.GetComponent<Renderer>().material;
    Color baseCol = mat.GetColor("_EmissionColor");
    Color green = colors[1];
    while (timer < durIn) {
      mat.SetColor("_EmissionColor", Color.Lerp(baseCol, green * 2, timer / durIn));
      timer += Time.deltaTime;
      yield return null;
    }
    timer = 0f;
    while (timer < durOut) {
      mat.SetColor("_EmissionColor", Color.Lerp(green * 2, baseCol, timer / durOut));
      timer += Time.deltaTime;
      yield return null;
    }
    mat.SetColor("_EmissionColor", baseCol);
  }

  private IEnumerator LaserFX(Vector3[] points) {
    float timer = 0f;
    float durIn = 0.08f;
    float durOut = 0.1f;
    Color colin = Color.white;
    Color colout = Color.clear;
    LineRenderer laserEffect = Instantiate(laserBeamPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform).GetComponent<LineRenderer>();
    Renderer laser = laserEffect.gameObject.GetComponent<Renderer>();
    laserEffect.SetPosition(0, points[0]);
    laserEffect.SetPosition(1, points[1]);
    laserEffect.material.SetColor("_EmissionColor", colors[beamColor]);
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
    Destroy(laserEffect.gameObject);
  }

  private void StopFX() {
    ResetGun();
    StopCoroutine("LaserFX");
    StopCoroutine("Recoil");
    StopCoroutine("RecoilBackOnly");
    StopCoroutine("CrosshairFX");
    StopCoroutine("ReactorGlow");
    StopCoroutine("ChamberCharge");
  }

  private void ShootFX(Vector3[] points) {
    StartCoroutine(LaserFX(points));
    if (upRecoilAnim == 1) StartCoroutine(Recoil());
    else StartCoroutine(RecoilBackOnly());
    StartCoroutine(CrosshairFX());
    if (shells) EjectShell();
    if (activeGun == "Heavy Rifle") StartCoroutine(ReactorGlow());
    if (activeGun == "Assault Rifle") StartCoroutine(ChamberCharge());
    muzzleFlashFX.GetComponent<ParticleSystem>().Play();
  }
}
