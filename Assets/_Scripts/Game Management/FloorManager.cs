using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Linq;
using TMPro;

[System.Serializable]
public class FloorTheme
{

  public GameObject[] floorTypeStartRooms;
  public GameObject[] floorTypeRooms;
  public GameObject[] floorTypeEndRooms;
  public WeaponItem[] floorTypeWeapons;
  public GameObject floorTypeEnemy;
}

public class FloorManager : MonoBehaviour
{

  [SerializeField] private FloorTheme[] _floorThemes;
  [SerializeField] private float _healthBase, _speedBase, _visionRangeBase, _visionArcBase, _awarenessBase, _lookSpeedBase;
  [SerializeField] private float _healthCap, _speedCap, _visionRangeCap, _visionArcCap, _awarenessCap, _lookSpeedCap;
  [SerializeField] private float _diffScaleSpeed;
  [SerializeField] private GameObject _loseScreenPrefab;
  [SerializeField] private AudioClip _loseAudio;
  [SerializeField] private string[] _flavorTexts;

  private float _healthScale, _speedScale, _visionRangeScale, _visionArcScale, _awarenessScale, _lookSpeedScale;

  private int _chosenFloorType;
  private float _diffScaleOverall = 1f; 
  private int _currHueShift = 0;

  [HideInInspector] public List<GameObject> _robotsSeen = new List<GameObject>();
  [HideInInspector] public List<string[]> _robotData = new List<string[]>();


  //Put this script on a DDOL GameObject (needs persistence)

  private void Awake() {
    _healthScale = _healthBase;
    _speedScale = _speedBase;
    _visionRangeScale = _visionRangeBase;
    _visionArcScale = _visionArcBase;
    _awarenessScale = _awarenessBase;
    _lookSpeedScale = _lookSpeedBase;
    _chosenFloorType = 0;
  }

  private void Start() {
    if (GameObject.FindGameObjectsWithTag("Persistent").Length > 1 || !(SceneManager.GetActiveScene().name == "Game" || SceneManager.GetActiveScene().name == "Tutorial")) Destroy(gameObject);
    //if (GameObject.FindGameObjectsWithTag("Persistent").Length > 1) Destroy(gameObject);
    DontDestroyOnLoad(gameObject);
    SceneManager.activeSceneChanged += InNewFloor;
  }

  public void ClearedFloor() {
    GetComponent<PlayerTracker>().ClearedFloor();
    StartCoroutine(MoveFloors());
  }

  public GameObject[] GetCurrentFloorRooms() {
    return _floorThemes[_chosenFloorType].floorTypeRooms;
  }

  public GameObject[] GetCurrentFloorStartRooms() {
    return _floorThemes[_chosenFloorType].floorTypeStartRooms;
  }

  public GameObject[] GetCurrentFloorEndRooms() {
    return _floorThemes[_chosenFloorType].floorTypeEndRooms;
  }

  public WeaponItem[] GetCurrentFloorWeapon() {
    return _floorThemes[_chosenFloorType].floorTypeWeapons;
  }

  public GameObject GetCurrentFloorEnemies() {
    return _floorThemes[_chosenFloorType].floorTypeEnemy;
  }

  public float[] GetRandEnemyAIStats() {
    float visionRange = Random.Range(_visionRangeScale - (_visionRangeScale / 10), _visionRangeScale + (_visionRangeScale / 10));
    float visionArc = Random.Range(_visionArcScale - (_visionArcScale / 12), _visionArcScale + (_visionArcScale / 12));
    float awareness = Random.Range(_awarenessScale - (_awarenessScale / 4), _awarenessScale + (_awarenessScale / 4));
    float lookSpeed = Random.Range(_lookSpeedScale - (_lookSpeedScale / 6), _lookSpeedScale + (_lookSpeedScale / 6));
    if (visionRange > _visionRangeCap) visionRange = _visionRangeCap;
    if (visionArc > _visionArcCap) visionArc = _visionArcCap;
    if (awareness > _awarenessCap) awareness = _awarenessCap;
    if (lookSpeed > _lookSpeedCap) lookSpeed = _lookSpeedCap;
    return new float[] {visionRange, visionArc, awareness, lookSpeed};
  }

  public float GetRandEnemySpeed() {
    float speed = Random.Range(_speedScale - (_speedScale / 5), _speedScale + (_speedScale / 5));
    if (speed > _speedCap) speed = _speedCap;
    return speed;
  }

  public float GetRandEnemyHealth() {
    float health = Random.Range(_healthScale - (_healthScale / 10), _healthScale + (_healthScale / 10));
    if (health > _healthCap) health = _healthCap;
    health = Mathf.Floor(health);
    return health;
  }

  public WeaponItem[] GetRandWeapons() {
    WeaponItem[] weaponsToReturn = new WeaponItem[1];
    int wpn1 = Random.Range(0, _floorThemes[_chosenFloorType].floorTypeWeapons.Length);
    weaponsToReturn[0] = _floorThemes[_chosenFloorType].floorTypeWeapons[wpn1];
    if (Random.value < _diffScaleOverall - 1) {
      int wpn2 = Random.Range(0, _floorThemes[_chosenFloorType].floorTypeWeapons.Length);
      while (wpn2 == wpn1) wpn2 = Random.Range(0, _floorThemes[_chosenFloorType].floorTypeWeapons.Length);
      weaponsToReturn = weaponsToReturn.Append(_floorThemes[_chosenFloorType].floorTypeWeapons[wpn2]).ToArray();
    }
    return weaponsToReturn;
  }

  public int GetHueShift() {
    return _currHueShift;
  }

  public void LoseState() {
    PauseHandler pauseHandler = GameObject.FindWithTag("Handler").GetComponent<PauseHandler>();
    pauseHandler._disablePauseKeycodes = true;
    pauseHandler.Pause(false, false);
    StartCoroutine(SubmitHighScore());
    StartCoroutine(DeathEffects());
    BringUpOverviewScreen();
  }

  private IEnumerator DeathEffects() {
    float elapsedTime = 0;
    float duration = 1f;
    MusicManager musicManager = GameObject.FindWithTag("Handler").GetComponent<MusicManager>();
    musicManager.Pause();
    musicManager.gameObject.GetComponent<AudioSource>().clip = _loseAudio;
    musicManager.gameObject.GetComponent<AudioSource>().Play();
    VolumeProfile volumeProfile = GameObject.FindGameObjectWithTag("Post Processing").GetComponent<Volume>().profile;
    FilmGrain filmGrain;
    volumeProfile.TryGet(out filmGrain);
    while (elapsedTime < duration) {
      filmGrain.intensity.Override(Mathf.SmoothStep(0, 50, elapsedTime / duration));
      elapsedTime += Time.unscaledDeltaTime;
      yield return null;
    }
  }

  private void BringUpOverviewScreen() {
    if (GameObject.FindWithTag("LoseScreen")) return;

    int[] runStats = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PlayerTracker>().GetRunStats();
    GameObject loseScreenInstance = Instantiate(_loseScreenPrefab, Vector3.zero, Quaternion.identity);
    LoseScreenManager loseScreenManager = loseScreenInstance.GetComponent<LoseScreenManager>();
    loseScreenManager.Initalize(runStats, _flavorTexts[Random.Range(0, _flavorTexts.Length)]);
    GetComponent<PlayerTracker>().ResetRun();
  }

	private IEnumerator SubmitHighScore() {
    int[] runStats = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PlayerTracker>().GetRunStats();
    GameObject connectionManager = GameObject.FindGameObjectWithTag("ConnectionManager");
    if (connectionManager == null) {
      Debug.LogWarning("FloorManager: No connection manager found.");
      yield break;
    }
    LeaderboardConnect leaderboardConnect = connectionManager.GetComponent<LeaderboardConnect>();
    leaderboardConnect.PostScores(runStats[0], runStats[1]);
  }

  private IEnumerator MoveFloors() {
    _healthScale *= _diffScaleSpeed;
    _speedScale *= _diffScaleSpeed;
    _visionRangeScale *= _diffScaleSpeed;
    _visionArcScale *= _diffScaleSpeed;
    _awarenessScale *= _diffScaleSpeed;
    _lookSpeedScale *= _diffScaleSpeed;
    _diffScaleOverall *= _diffScaleSpeed;
    _chosenFloorType = Random.Range(0, _floorThemes.Length);
    _robotsSeen.Clear();
    _robotData.Clear();

    //Animate switch with post processing here or something
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    yield return null;
  }

  private void InNewFloor(Scene current, Scene next) {
    VolumeProfile volumeProfile = GameObject.FindGameObjectWithTag("Post Processing").GetComponent<Volume>().profile;
    ColorAdjustments colorAdjustments;
    volumeProfile.TryGet(out colorAdjustments);
    _currHueShift = Random.Range(-360, 361);
    colorAdjustments.hueShift.Override(_currHueShift);
  }
}