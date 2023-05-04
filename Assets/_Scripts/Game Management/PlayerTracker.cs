using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTracker : MonoBehaviour
{

  private int roomsCleared;
  private int floorsCleared;
  private int kills;

  private void Start() {
    if (GameObject.FindGameObjectsWithTag("Persistent") > 1 || SceneManager.GetActiveScene().name != "Game") {
      Destroy(gameObject);
    }
    DontDestroyOnLoad(gameObject);
  }

  public void ClearedRoom() {
    roomsCleared++;
  }

  public void ClearedFloor() {
    floorsCleared++;
  }

  public void Kills() {
    kills++;
  }
}