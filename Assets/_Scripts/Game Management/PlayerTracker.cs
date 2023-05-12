using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTracker : MonoBehaviour
{

  private int roomsCleared;
  private int floorsCleared;
  private int kills;
  private int runTime;
  private int damage;
  private int switches;
  private int score;

  private void Start() {
    if (GameObject.FindGameObjectsWithTag("Persistent").Length > 1 || SceneManager.GetActiveScene().name != "Game") {
      Destroy(gameObject);
    }
    DontDestroyOnLoad(gameObject);
  }

  private void Update() {
    runTime += Time.deltaTime;
  }

  public int[] GetRunStats() {
    int scoreToSubmit = GameObject.FindGameObjectWithTag("Persistent").GetComponent<FloorManager>().GetScore();
    return new int[] {score, runTime, roomsCleared, floorsCleared, kills, damage, switches};
  }

  public void ClearedRoom() {
    roomsCleared++;
    score += 150;
  }

  public void ClearedFloor() {
    floorsCleared++;
    score += 1000;
  }

  public void Kills() {
    kills++;
    score += 100;
  }

  public void Damage(int damageAmt) {
    damage += damageAmt;
  }

  public void Switched() {
    switches++;
  }
}