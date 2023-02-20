using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using static GunData;

public class GunController : MonoBehaviour
{
  [SerializeField] private GameObject gameHandler;

  [SerializeField] private GameObject HUDCanvas;
  [SerializeField] private GameObject shell;
  [SerializeField] private GameObject ammoInfo;
  [SerializeField] private GameObject laserBeamPrefab;
  [SerializeField] private GameObject explosionAudioPrefab;
  [SerializeField][ColorUsageAttribute(true, true)] private Color[] colors;


  private Camera cam;
  private float canFireTime;
  private float holdTimer;
  private float vertRecoilTracking;
  private float changeTimer;
  private Transform gun;
  private GameObject reloadtext;
  private GameObject leftleg, rightleg;
  private GameObject impactFX;
  private GameObject explodeFX;
  private GameObject muzzleFlashFX;
  private GameObject fragmentFX;
  private GameObject magazine;
  private ParticleSystem.MainModule main;
  private ParticleSystem.ShapeModule sh;
  private CinemachineVirtualCamera vcam;
  private CinemachinePOV pov;
  private int rounds;
  private bool reloading;
  private bool shells = false;
  private TextMeshProUGUI magtext;
  private TextMeshProUGUI ammoText;
  private Transform crosshair;
  private Vector3 gunInitPos;
  private Quaternion gunInitRot;
  private Transform gunMuzzle;
  private Transform attachmentHolder;
  private Vector3 beamInit;
  private AudioSource shootSound;
  private AmmoScript ammoScript;
  private GunData pistol = new GunData("Pistol");
  private GunData assaultRifle = new GunData("Assault Rifle");
  private GunData heavyRifle = new GunData("Heavy Rifle");
  private GunData activeGun;
  private GunData activePrimary;
  private GunData activeSecondary;
  private List<Attachment> activeAttachments;
  private PersistentData dataContainer;

  private void Start() {
    GameObject.FindGameObjectWithTag("Data").GetComponent<PersistentData>().selectedSecondary = new GunData("Pistol");
    dataContainer = GameObject.FindGameObjectWithTag("Data").GetComponent<PersistentData>();
    RefactorGunData(dataContainer.selectedPrimary.id, dataContainer.selectedSecondary.id);
    gun = transform.GetChild(activePrimary.id);
    activeGun = activePrimary;
    activeAttachments = dataContainer.selectedAttachments;
    ReloadGunAssets();
    ProcessAttachments();
    ammoText = ammoInfo.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    vcam = GameObject.FindGameObjectWithTag("VCam").GetComponent<CinemachineVirtualCamera>();
    pov = vcam.GetCinemachineComponent<CinemachinePOV>();
    crosshair = HUDCanvas.transform.GetChild(0);
    leftleg = crosshair.GetChild(1).gameObject;
    rightleg = crosshair.GetChild(2).gameObject;
    rounds = activeGun.magSize;
    cam = GetComponent<Camera>();
    Cursor.lockState = CursorLockMode.Locked;
    beamInit = transform.GetChild(1).GetChild(3).localScale;
    ammoScript = ammoInfo.GetComponent<AmmoScript>();
    ReloadGunValues();
  }

  private void Update() {
    if (vertRecoilTracking > 0 && vertRecoilTracking - activeGun.recoilRecovery * Time.deltaTime >= 0) pov.m_VerticalAxis.Value += 20f * activeGun.recoilRecovery * Time.deltaTime;
    else if (vertRecoilTracking > 0) pov.m_VerticalAxis.Value += 20f * ((activeGun.recoilRecovery * Time.deltaTime) - Mathf.Abs((vertRecoilTracking - activeGun.recoilRecovery * Time.deltaTime)));
    vertRecoilTracking = Mathf.Clamp(vertRecoilTracking - activeGun.recoilRecovery * Time.deltaTime, 0, 1);
    if (Input.GetMouseButtonDown(0)) {
      if (activeGun.id == 0 | activeGun.id == 2) Shoot();
      holdTimer = 0;
    }
    else if (Input.GetMouseButton(0)) {
      if (activeGun.id == 1) Shoot();
      holdTimer += Time.deltaTime;
    }
    else if (Input.GetMouseButtonUp(0)) {
      holdTimer = 0;
    }

    if (Input.GetKeyDown(KeyCode.R)) {
      Reload();
    }

    if (Input.GetKeyDown(KeyCode.Alpha1) && changeTimer <= 0) {
      ChangeGun(activePrimary);
      changeTimer = 1;
    }
    else if (Input.GetKeyDown(KeyCode.Alpha2) && changeTimer <= 0) {
      ChangeGun(activeSecondary);
      changeTimer = 1;
    }

    changeTimer -= Time.deltaTime;
  }

  //Loads gun attachments that are in use
  private void ProcessAttachments() {
    if (activeAttachments.Count == 0) return;
    for (int i = 0; i < activeAttachments.Count; i++) {
      switch(activeAttachments[i].type) {
        case 0:
          activeGun.magSize += (int)activeAttachments[i].value;
          break;
        case 1:
          activeGun.fireRate *= activeAttachments[i].value;
          break;
      }
      attachmentHolder.GetChild(activeAttachments[i].type).gameObject.SetActive(true);
    }
  }

  //Get value of active attachments
  public List<Attachment> GetActiveAttachments() {
    return activeAttachments;
  }

  //Switch statement
  private void RefactorGunData(int id, int secid) {
    switch(id) {
      case 0:
        activePrimary = pistol;
        break;
      case 1:
        activePrimary = assaultRifle;
        break;
      case 2:
        activePrimary = heavyRifle;
        break;
    }
    switch(secid) {
      case 0:
        activeSecondary = pistol;
        break;
      case 1:
        activeSecondary = assaultRifle;
        break;
      case 2:
        activeSecondary = heavyRifle;
        break;
    }
  }

  //Fetches all necessary child GameObjects from active gun
  private void ReloadGunAssets() {
    magazine = gun.GetChild(1).gameObject;
    gunMuzzle = gun.GetChild(2);
    attachmentHolder = gunMuzzle.GetChild(5);
    gun.gameObject.SetActive(true);
    gunInitPos = gun.transform.localPosition;
    gunInitRot = gun.transform.localRotation;
    impactFX = gunMuzzle.GetChild(0).gameObject;
    fragmentFX = gunMuzzle.GetChild(1).gameObject;
    explodeFX = gunMuzzle.GetChild(2).gameObject;
    main = explodeFX.GetComponent<ParticleSystem>().main;
    sh = explodeFX.GetComponent<ParticleSystem>().shape;
    muzzleFlashFX = gunMuzzle.GetChild(3).gameObject;
    shootSound = gunMuzzle.GetChild(4).gameObject.GetComponent<AudioSource>();
  }

  //Reloads values associated with the active gun
  private void ReloadGunValues() {
    rounds = activeGun.magSize;
    ammoText.text = rounds.ToString() + " | " + activeGun.magSize.ToString();
    ammoScript.maxAmmo = activeGun.magSize;
    ammoScript.currAmmo = rounds;
  }

  //Gives activeGun value to other scripts
  public int GetActiveGun() {
    return activeGun.id;
  }

  //Handles changing the active gun
  public void ChangeGun(GunData newGun) {
    activeGun = newGun;
    StopFX();
    ResetGun();
    transform.GetChild(0).gameObject.SetActive(false);
    transform.GetChild(1).gameObject.SetActive(false);
    transform.GetChild(2).gameObject.SetActive(false);
    gun = transform.GetChild(activeGun.id);
    ReloadGunAssets();
    ReloadGunValues();
    vertRecoilTracking = 0f;
  }

  //Checks if player is able to shoot, returns bool
  private bool CanShoot() {
    if (Time.timeScale == 0f) return false;
    if (rounds == 0) {
      Reload();
      return false;
    }
    else if (Time.time > canFireTime && reloading == false && gameHandler.GetComponent<PauseHandler>().isPaused == false) return true;
    else return false;
  }

  private void ResetGun() {
    gun.transform.localPosition = gunInitPos;
    gun.transform.localRotation = gunInitRot;
  }

  public void Reload() {
    if (rounds < activeGun.magSize) {
      rounds = activeGun.magSize;
      reloading = true;
      StartCoroutine(ReloadAnim());
    }
  }

  //Currently unused, shell ejection animation
  private void EjectShell() {
    Rigidbody rb = Instantiate(shell, gun.transform.position, Quaternion.identity, cam.gameObject.transform).GetComponent<Rigidbody>();
    rb.gameObject.transform.localPosition = new Vector3(0.55f, -0.1f, 0.66f);
    rb.gameObject.transform.localRotation = gun.localRotation;
    rb.AddForce(cam.gameObject.transform.localRotation * new Vector3(Random.Range(-1, -0.5f), Random.Range(1, 2), 0),ForceMode.Impulse);
  }

  //Handles firing of gun, starts effects coroutines and positions laser
  private void Shoot() {
    if (!CanShoot()) return;
    StopFX();
    rounds -= 1;
    ammoText.text = rounds.ToString() + " | " + activeGun.magSize.ToString();
    ammoScript.currAmmo = rounds;
    canFireTime = Time.time + activeGun.fireRate;
    Vector3 origin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, cam.nearClipPlane));
    RaycastHit hit;
    Vector3[] points = new Vector3[2];
    points[0] = gunMuzzle.position;
    Vector3 direction = Quaternion.Euler(holdTimer < activeGun.fireRate ? Random.Range(-activeGun.shotSpread / 4, activeGun.shotSpread / 4) : Random.Range(-activeGun.shotSpread, activeGun.shotSpread), holdTimer < activeGun.fireRate ? Random.Range(-activeGun.shotSpread / 4, activeGun.shotSpread / 4) : Random.Range(-activeGun.shotSpread, activeGun.shotSpread), 0) * cam.transform.forward;
    if (Physics.Raycast(origin, direction, out hit)) {
      points[1] = hit.point;
      impactFX.transform.position = hit.point;
      impactFX.GetComponent<ParticleSystem>().Play();
      if (hit.collider.gameObject.CompareTag("Target") | hit.collider.gameObject.CompareTag("Destructible")) {
        if (hit.collider.gameObject.CompareTag("Target")) HitTarget(hit);
        else HitObject(hit);
        if (activeGun.id == 2) {
          int pierced = 1;
          while (pierced <= 1) {
            pierced++;
            if (!Physics.Raycast(hit.point, direction, out hit)) {
              points[1] = hit.point + (direction * 50);
              break;
            }
            points[1] = hit.point;
            impactFX.transform.position = hit.point;
            impactFX.GetComponent<ParticleSystem>().Play();
            if (hit.collider.gameObject.CompareTag("Target")) HitTarget(hit);
            if (hit.collider.gameObject.CompareTag("Destructible")) HitObject(hit);
          }
        }
      }
    }
    else {
      points[1] = origin + (direction * 50);
    }
    ShootFX(points);
    vertRecoilTracking = Mathf.Clamp(vertRecoilTracking + activeGun.upRecoil, 0, 1);
    pov.m_VerticalAxis.Value -= 20f * activeGun.upRecoil;
  }

  public void HitTarget(RaycastHit hit) {
    hit.collider.gameObject.GetComponent<HealthManager>().Damage(activeGun.shotDamage);
    if (hit.collider.gameObject.GetComponent<HealthManager>()._currentHealth >= 0) {
      return;
    }
    GetComponent<PointTracker>().DestroyedTarget(hit.collider.gameObject);
    StartCoroutine(ExplodeTarget(hit.collider.gameObject));
  }

  public void HitObject(RaycastHit hit) {
    hit.collider.gameObject.GetComponent<HealthManager>().Damage(activeGun.shotDamage);
    if (hit.collider.gameObject.GetComponent<HealthManager>()._currentHealth >= 0) {
      return;
    }
    StartCoroutine(ExplodeTarget(hit.collider.gameObject));
  }

  //Animation for reloading the gun
  private IEnumerator ReloadAnim() {
    float timer = 0f;
    float inTime = 0.35f * activeGun.reloadTime;
    float outTime = 0.3f * activeGun.reloadTime;
    float popUpTime = 0.1f * activeGun.reloadTime;
    float popDownTime = 0.125f * activeGun.reloadTime;
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
    ammoText.text = rounds.ToString() + " | " + activeGun.magSize.ToString();
    ammoScript.currAmmo = rounds;
    reloading = false;
  }

  //Animates crosshair during recoil
  private IEnumerator CrosshairFX() {
    float timer = 0f;
    float outTime = 0.15f;
    float inTime = 0.2f;
    while (timer < outTime) {
      leftleg.transform.localPosition = Vector3.Lerp(new Vector3(-30f * (leftleg.transform.localScale.x / 0.5f), 0, 0), new Vector3(-30f - (30 * (activeGun.upRecoil / 0.2f) * (leftleg.transform.localScale.x / 0.5f)), 0, 0), Mathf.SmoothStep(0f, 1f, timer / outTime));
      rightleg.transform.localPosition = leftleg.transform.localPosition * -1f;
      timer += Time.deltaTime;
      yield return null;
    }
    timer = 0;
    while (timer < inTime) {
      leftleg.transform.localPosition = Vector3.Lerp(new Vector3(-30f - (30 * (activeGun.upRecoil / 0.2f) * (leftleg.transform.localScale.x / 0.5f)), 0, 0), new Vector3(-30f * (leftleg.transform.localScale.x / 0.5f), 0, 0), Mathf.SmoothStep(0f, 1f, timer / inTime));
      rightleg.transform.localPosition = leftleg.transform.localPosition * -1f;
      timer += Time.deltaTime;
      yield return null;
    }
  }

  // Destroys target when hit, triggering particle effects
  // NOTE: Gonna move this part to the target to handle -Matthew
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
    GetComponent<ItemDrops>().RollForItem();
    GameObject tmp = Instantiate(explosionAudioPrefab, target.transform.position, Quaternion.identity);
    Destroy(target);
    tmp.GetComponent<AudioSource>().Play();
    yield return new WaitForSeconds(tmp.GetComponent<AudioSource>().clip.length);
    Destroy(tmp);
  }

  //Recoil animation with only backwards recoil animated
  private IEnumerator RecoilBackOnly() {
    float timer = 0f;
    float rcUp = 0.09f;
    float rcDown = 0.125f;
    Vector3 initial = gun.localPosition;
    while (timer < rcUp) {
      gun.localPosition = Vector3.Lerp(initial, new Vector3(initial.x, initial.y, initial.z - (0.8f * activeGun.backRecoil)), timer / rcUp);
      timer += Time.deltaTime;
      yield return null;
    }
    timer = 0f;
    gun.localPosition = new Vector3(initial.x, initial.y, initial.z - 0.8f * activeGun.upRecoil);
    while (timer < rcDown) {
      gun.localPosition = Vector3.Lerp(new Vector3(initial.x, initial.y, initial.z - (0.8f * activeGun.backRecoil)), initial, timer / rcDown);
      timer += Time.deltaTime;
      yield return null;
    }
    gun.localPosition = initial;
  }

  //Full recoil animation
  private IEnumerator Recoil() {
    float timer = 0f;
    float rcUp = 0.09f;
    float rcDown = 0.125f;
    Vector3 initial = gun.localPosition;
    while (timer < rcUp) {
      gun.localRotation = Quaternion.Lerp(Quaternion.Euler(0, -90f , 0f), Quaternion.Euler(0, -90f, (100f * activeGun.upRecoil)), timer / rcUp);
      gun.localPosition = Vector3.Lerp(initial, new Vector3(initial.x, initial.y, initial.z - (0.8f * activeGun.backRecoil)), timer / rcUp);
      timer += Time.deltaTime;
      yield return null;
    }
    timer = 0f;
    gun.localPosition = new Vector3(initial.x, initial.y, initial.z - 0.8f * activeGun.upRecoil);
    gun.localRotation = Quaternion.Euler(0f, -90f, (100f * activeGun.backRecoil));
    while (timer < rcDown) {
      gun.localRotation = Quaternion.Lerp(Quaternion.Euler(0, -90f, (100f * activeGun.upRecoil)), Quaternion.Euler(0, -90f, 0f), timer / rcDown);
      gun.localPosition = Vector3.Lerp(new Vector3(initial.x, initial.y, initial.z - (0.8f * activeGun.backRecoil)), initial, timer / rcDown);
      timer += Time.deltaTime;
      yield return null;
    }
    gun.localPosition = initial;
    gun.localRotation = Quaternion.Euler(0f, -90f, 0f);
  }

  //Charging animation for assault rifle's "laser chamber"
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

  //Glowing effect on firing for heavy rifle's "reactor"
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

  //Effects for laser beam, handles color change
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
    laserEffect.material.SetColor("_EmissionColor", colors[activeGun.shotColor]);
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
    StopCoroutine(nameof(LaserFX));
    StopCoroutine(nameof(Recoil));
    StopCoroutine(nameof(RecoilBackOnly));
    StopCoroutine(nameof(CrosshairFX));
    StopCoroutine(nameof(ReactorGlow));
    StopCoroutine(nameof(ChamberCharge));
  }

  //Triggers all effects for shooting
  private void ShootFX(Vector3[] points) {
    shootSound.Play();
    StartCoroutine(LaserFX(points));
    if (activeGun.upRecoilAnim == true) StartCoroutine(Recoil());
    else StartCoroutine(RecoilBackOnly());
    StartCoroutine(CrosshairFX());
    if (shells) EjectShell();
    if (activeGun == heavyRifle) StartCoroutine(ReactorGlow());
    if (activeGun == assaultRifle) StartCoroutine(ChamberCharge());
    muzzleFlashFX.GetComponent<ParticleSystem>().Play();
  }
}
