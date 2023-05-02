using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class FloorTheme
{

  public GameObject[] floorTypeRooms;
  public GameObject[] floorTypeStartRooms;
  public GameObject[] floorTypeEndRooms;
  public GameObject[] floorTypeWeapons;
  public GameObject floorTypeEnemy;
}

public class FloorManager : MonoBehaviour
{

  [SerializeField] private FloorTheme[] _floorThemes;

  private int _chosenFloorType;

  //Put this script on a DDOL GameObject (needs persistence)

  private void Start() {
    if (GameObject.FindGameObjectsWithTag("Persistent").Length > 0) Destroy(gameObject);
    DontDestroyOnLoad(gameObject);
  }

  public void ClearedFloor() {
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

  private IEnumerator MoveFloors() {
    _chosenFloorType = Random.Range(0, _floorThemes.Length);
    //Animate switch with post processing here or something
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    yield return null;
  }
}