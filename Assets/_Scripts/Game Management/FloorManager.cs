using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class FloorTheme
{

  public GameObject[] floorTypeStartRooms;
  public GameObject[] floorTypeRooms;
  public GameObject[] floorTypeEndRooms;
  public GameObject[] floorTypeWeapons;
  public GameObject floorTypeEnemy;
}

public class FloorManager : MonoBehaviour
{

  [SerializeField] private FloorTheme[] _floorThemes;
  [SerializeField] private float _healthBase, _speedBase, _visionRangeBase, _visionArcBase, _awarenessBase, _lookSpeedBase;
  [SerializeField] private float _diffScaleSpeed;

  private float _healthScale, _speedScale, _visionRangeScale, _visionArcScale, _awarenessScale, _lookSpeedScale;

  private int _chosenFloorType;

  [HideInInspector] public List<GameObject> _robotsSeen = new List<GameObject>();
  [HideInInspector] public List<string[]> _robotData = new List<string[]>();

  //Put this script on a DDOL GameObject (needs persistence)

  private void Awake() {
    _chosenFloorType = 0;
  }

  private void Start() {
    if (GameObject.FindGameObjectsWithTag("Persistent").Length > 1 || SceneManager.GetActiveScene().name != "Game") Destroy(gameObject);
    DontDestroyOnLoad(gameObject);
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

  public GameObject[] GetCurrentFloorWeapon() {
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
    return new float[] {visionRange, visionArc, awareness, lookSpeed};
  }

  public float GetRandEnemySpeed() {
    return Random.Range(_speedScale - (_speedScale / 5), _speedScale + (_speedScale / 5));
  }

  public float GetRandEnemyHealth() {
    return Random.Range(_healthScale - (_healthScale / 10), _healthScale + (_healthScale / 10));
  }

  private IEnumerator MoveFloors() {
    _healthScale *= _diffScaleSpeed;
    _speedScale *= _diffScaleSpeed;
    _visionRangeScale *= _diffScaleSpeed;
    _visionArcScale *= _diffScaleSpeed;
    _awarenessScale *= _diffScaleSpeed;
    _lookSpeedScale *= _diffScaleSpeed;
    _chosenFloorType = Random.Range(0, _floorThemes.Length);
    _robotsSeen.Clear();
    _robotData.Clear();
    //Animate switch with post processing here or something
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    yield return null;
  }
}