using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class X5_57Controller : MonoBehaviour
{

  public BotData botData = new BotData();
  public GameObject player;
  public float currentHealthTankAmount;
  public float currentChargeAmount; 
  public float currentShieldHealthAmount;

  private GameObject focusedTarget;
  private GameObject persistentData;
  private GameObject beingDestroyed;
  private string currentMode = "Attack";
  private ParticleSystem.MainModule main;
  private ParticleSystem.ShapeModule sh;
  private float timer;
  private float timeSinceShot;
  private bool turning;

  [SerializeField] private GameObject explodeFX;
  [SerializeField] private GameObject fragmentFX;
  [SerializeField] private GameObject muzzleFlashFX;
  [SerializeField] private GameObject impactFX;
  [SerializeField] private GameObject explosionAudioPrefab;
  [SerializeField] private GameObject laserBeamPrefab;
  [SerializeField][ColorUsageAttribute(true, true)] private Color laserColor;

  void Awake() {
    persistentData = GameObject.FindGameObjectWithTag("Data");
    // botData = persistentData.GetComponent<PersistentData>().selectedBotStats;
    currentHealthTankAmount = botData.healTankMaxCapacity;
    currentChargeAmount = botData.maxCharge;
    currentShieldHealthAmount = botData.shieldMaxHealth;
    main = explodeFX.GetComponent<ParticleSystem>().main;
    sh = explodeFX.GetComponent<ParticleSystem>().shape;
  }
  void Update() {
    switch (currentMode) {
      case "Attack":
        AttackMode();
        break;
      case "Heal":
        HealMode();
        break;
      case "ConservePower":
        ConservePowerMode();
        break;
      case "ProtectOffensive":
        ProtectModeOffensive();
        break;
      case "ProtectDefensive":
        ProtectModeDefensive();
        break;
      case "Testing":
        TestingMode();
        break;
    }
  }

  void ChangeMode(string selectedMode) {
    currentMode = selectedMode;
  }

  void AttackMode() {
    // This mode has the bot shoot at the closest targets/turrets/etc. 
    if (!focusedTarget) {
      float distance;
      float bestDistance = Mathf.Infinity;
      GameObject bestTarget;
      Collider[] targets = Physics.OverlapSphere(transform.position, botData.maxRange);
      if (!targets.Any()) {
        StartCoroutine(MoveToObject(player, 2, 2, 2));
        return;
      } 
      else {
        bestTarget = targets[0].gameObject;
        foreach (Collider col in targets) {
          distance = (transform.position - col.transform.position).magnitude;
          if (col.gameObject.tag == "Target" && distance < bestDistance && col.gameObject != beingDestroyed) {
            bestTarget = col.gameObject;
            bestDistance = distance;
          }
        }
        focusedTarget = bestTarget;
      }
    }
    if (!turning) {
      RaycastHit hit;
      if (!Physics.Raycast(transform.position, Quaternion.Euler(-90, 90, 0) * transform.up, out hit)) {
        StartCoroutine(TurnToObject(focusedTarget));
        return;
      }
      if (hit.collider.gameObject == focusedTarget) {
        if (timeSinceShot >= botData.attackRate) {
          HitTarget(focusedTarget);
          muzzleFlashFX.GetComponent<ParticleSystem>().Play();
          impactFX.transform.position = hit.point;
          impactFX.GetComponent<ParticleSystem>().Play();
          StartCoroutine(LaserFX(new Vector3[] {transform.position, hit.point}));
          timeSinceShot = 0;
        }
      }
      else if (transform.rotation == Quaternion.LookRotation(focusedTarget.transform.position - transform.position) * Quaternion.Euler(-90, 90, 0)) {
        focusedTarget = null;
        return;
      }
      else StartCoroutine(TurnToObject(focusedTarget));
    }
    timeSinceShot += Time.deltaTime;
  }

  void HealMode() {
    // this mode gives the player health periodically.
    StartCoroutine(MoveToObject(player, 2, 2, 2));
    timer += Time.deltaTime;
    if (timer >= botData.healPeriod) {
      HealPlayer();
      timer = 0;
    }
  }

  void ConservePowerMode() {
    // this mode has the bot get as far away as possible in order to stay alive.
  }

  void ProtectModeOffensive() {
    // this mode has the bot shoot down missiles coming the player's direction.
  }

  void ProtectModeDefensive() {
    // this mode has the bot block the player with a shield.
  }

  void TestingMode() {
    StartCoroutine(TurnToObject(player));
    StartCoroutine(MoveToObject(player, 2, 2, 2));
  }

  private IEnumerator TurnToObject(GameObject focus) {
    turning = true;
    float elapsedTime = 0f;
    float waitTime = 1f;      
    Vector3 relativePos = focus.transform.position - transform.position;
    Quaternion toRotation = Quaternion.LookRotation(relativePos) * Quaternion.Euler(new Vector3(-90, 90, 0));
    Quaternion initRotation = transform.rotation;
    while (elapsedTime < waitTime) {
      transform.rotation = Quaternion.Lerp(initRotation, toRotation, elapsedTime / waitTime);
      elapsedTime += Time.deltaTime;
      yield return null;
    }
    transform.rotation = toRotation;
    turning = false;
  }

  private IEnumerator MoveToObject(GameObject focus, int proxX, int proxY, int proxZ) {
    float elapsedTime = 0f;
    float waitTime = 1f;      
    Vector3 targetLocation = new Vector3(focus.transform.position.x + proxX, player.transform.position.y + proxY, player.transform.position.z + proxZ);
    Vector3 initLoc = transform.position;
    while (elapsedTime < waitTime) {
      transform.position = Vector3.Lerp(initLoc, targetLocation, elapsedTime / waitTime);
      elapsedTime += Time.deltaTime;
      yield return null;
    }
  }

  public void HitTarget(GameObject hit) {
    hit.GetComponent<HealthManager>().Damage(botData.attackPower);
    if (hit.GetComponent<HealthManager>()._currentHealth >= 0) {
      return;
    }
    player.GetComponent<ScoreTracker>().DestroyedTarget(hit);
    StartCoroutine(ExplodeTarget(hit));
    focusedTarget = null;
    beingDestroyed = hit;
    StartCoroutine(MoveToObject(player, 2, 2, 2));
    StartCoroutine(TurnToObject(player));
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
    GameObject tmp = Instantiate(explosionAudioPrefab, target.transform.position, Quaternion.identity);
    Destroy(target);
    tmp.GetComponent<AudioSource>().Play();
    yield return new WaitForSeconds(tmp.GetComponent<AudioSource>().clip.length);
    Destroy(tmp);
  }

  private IEnumerator LaserFX(Vector3[] points) {
    float fxtimer = 0f;
    float durIn = 0.08f;
    float durOut = 0.1f;
    Color colin = Color.white;
    Color colout = Color.clear;
    LineRenderer laserEffect = Instantiate(laserBeamPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform).GetComponent<LineRenderer>();
    Renderer laser = laserEffect.gameObject.GetComponent<Renderer>();
    laserEffect.SetPosition(0, points[0]);
    laserEffect.SetPosition(1, points[1]);
    laserEffect.material.SetColor("_EmissionColor", laserColor);
    laserEffect.enabled = true;
    laser.material.color = colout;
    laserEffect.startWidth = 0f;
    laserEffect.endWidth = laserEffect.startWidth;
    while (fxtimer < durIn) {
      laserEffect.startWidth = Mathf.Lerp(0f, 0.25f, fxtimer / durIn);
      laserEffect.endWidth = laserEffect.startWidth;
      laser.material.color = Color.Lerp(colout, colin, fxtimer / durIn);
      fxtimer += Time.deltaTime;
      yield return null;
    }
    fxtimer = 0f;
    laser.material.color = colin;
    laserEffect.startWidth = 0.25f;
    laserEffect.endWidth = laserEffect.startWidth;
    while (fxtimer < durOut) {
      laserEffect.startWidth = Mathf.Lerp(0.25f, 0f, fxtimer / durOut);
      laserEffect.endWidth = laserEffect.startWidth;
      laser.material.color = Color.Lerp(colin, colout, fxtimer / durOut);
      fxtimer += Time.deltaTime;
      yield return null;
    }
    laserEffect.startWidth = 0f;
    laserEffect.endWidth = laserEffect.startWidth;
    laser.material.color = colout;
    laserEffect.enabled = false;
    Destroy(laserEffect.gameObject);
  }

  private void HealPlayer() {
    if (Vector3.Distance(transform.position, player.transform.position) < botData.healRange) {
      player.GetComponent<HealthManager>()._currentHealth += botData.healPower;
      currentHealthTankAmount -= botData.healPower;
      print(currentHealthTankAmount);
    } else {
      StartCoroutine(MoveToObject(player, 2, 2, 2));
    }
  }
}