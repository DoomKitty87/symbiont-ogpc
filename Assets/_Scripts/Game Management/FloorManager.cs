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
  [SerializeField] private string[] _flavorTexts;

  private float _healthScale, _speedScale, _visionRangeScale, _visionArcScale, _awarenessScale, _lookSpeedScale;

  private int _chosenFloorType;
  private float _diffScaleOverall = 1f; 

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
    // Makes it beyond difficult to test in any other scene, so I'm disabling it for now
    if (GameObject.FindGameObjectsWithTag("Persistent").Length > 1 || SceneManager.GetActiveScene().name != "Game") Destroy(gameObject);
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

  public void LoseState() {
    GameObject.FindWithTag("Handler").GetComponent<PauseHandler>()._pauseState = PauseHandler.PauseState.Dead;
    StartCoroutine(SubmitHighScore());
    StartCoroutine(DeathEffects());
    BringUpOverviewScreen();
  }

  private IEnumerator DeathEffects() {
    float elapsedTime = 0;
    float duration = 1f;
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
    GameObject tmp = Instantiate(_loseScreenPrefab, Vector3.zero, Quaternion.identity);
    StartCoroutine(FadeOutDeathScreen(tmp));
    for (int i = 0; i < runStats.Length; i++) {
      tmp.transform.GetChild(1).GetChild(0).GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().text = runStats[i].ToString();
    }
    tmp.transform.GetChild(1).GetChild(2).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = _flavorTexts[Random.Range(0, _flavorTexts.Length)];
  }
  
  private IEnumerator FadeOutDeathScreen(GameObject tmp) {
    float alpha = 0;
    Time.timeScale = 0f;
    CanvasGroup tmpCanvasGroup = tmp.GetComponent<CanvasGroup>();

    // Fade in BackgroundDim
    while (alpha < 1) {
      tmpCanvasGroup.alpha = Mathf.Lerp(0, 1, alpha);
      alpha += Time.unscaledDeltaTime * 0.7f; // Takes one seconds to fade in
      yield return null;
	  }
    tmpCanvasGroup.alpha = 1;

    Cursor.visible = true;
    Cursor.lockState = CursorLockMode.None;

		//Fade in text
		alpha = 0;
		tmpCanvasGroup = tmp.transform.GetChild(1).GetComponent<CanvasGroup>();
		while (alpha < 1) {
			tmpCanvasGroup.alpha = Mathf.Lerp(0, 1, alpha);
			alpha += Time.unscaledDeltaTime; // Takes one seconds to fade in
			yield return null;
		}
		tmpCanvasGroup.alpha = 1;
	}


	private IEnumerator SubmitHighScore() {
    int[] runStats = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PlayerTracker>().GetRunStats();
    List<Score> scores = GameObject.FindGameObjectWithTag("ConnectionManager").GetComponent<LeaderboardConnect>().RetrieveScores();
    while (scores.Count == 0) {
      yield return new WaitForSeconds(0.05f);
    }
    bool submitted = false;
    foreach (Score s in scores) {
      if (s.name == GameObject.FindGameObjectWithTag("ConnectionManager").GetComponent<LoginConnect>().GetActiveAccountName()) {
        if (runStats[0] > s.score) {
          submitted = true;
          GameObject.FindGameObjectWithTag("ConnectionManager").GetComponent<LeaderboardConnect>().PostScores(runStats[0], runStats[1]);
        }
        else submitted = true;
      }
    }
    if (!submitted) GameObject.FindGameObjectWithTag("ConnectionManager").GetComponent<LeaderboardConnect>().PostScores(runStats[0], runStats[1]);
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
    colorAdjustments.hueShift.Override(Random.Range(-360, 361));
  }
}