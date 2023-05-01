using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloorManager : MonoBehaviour
{

  [SerializeField] private GameObject[][] _floorTypeRooms;
  [SerializeField] private GameObject[][] _floorTypeStartRooms;
  [SerializeField] private GameObject[][] _floorTypeEndRooms;
  [SerializeField] private GameObject[][] _floorTypeWeapons;
  [SerializeField] private GameObject[] _floorTypeEnemies;

  private int _chosenFloorType;

  //Put this script on a DDOL GameObject (needs persistence)
  public void ClearedFloor() {
    StartCoroutine(MoveFloors());
  }

  public GameObject[] GetCurrentFloorRooms() {
    return _floorTypeRooms[_chosenFloorType];
  }

  public GameObject[] GetCurrentFloorStartRooms() {
    return _floorTypeStartRooms[_chosenFloorType];
  }

  public GameObject[] GetCurrentFloorEndRooms() {
    return _floorTypeEndRooms[_chosenFloorType];
  }

  public GameObject[] GetCurrentFloorWeapon() {
    return _floorTypeWeapons[_chosenFloorType];
  }

  public GameObject GetCurrentFloorEnemies() {
    return _floorTypeEnemies[_chosenFloorType];
  }

  private IEnumerator MoveFloors() {
    _chosenFloorType = Random.Range(0, _floorTypeEnemies.Length);
    //Animate switch with post processing here or something
    SceneManager.LoadScene(SceneManager.GetActiveScene());
  }
}