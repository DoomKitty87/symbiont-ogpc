using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTracker : MonoBehaviour
{

  private int roomsCleared;
  private int floorsCleared;
  private int kills;
  private float runTime;
  private int damage;
  private int switches;
  private int score;

  private void Start() {
    if (GameObject.FindGameObjectsWithTag("Persistent").Length > 1 || SceneManager.GetActiveScene().name != "Game") {
      Destroy(gameObject);
    }
    DontDestroyOnLoad(gameObject);
  }

  public void ResetRun() {
    roomsCleared = 0;
    floorsCleared = 0;
    kills = 0;
    runTime = 0;
    damage = 0;
    switches = 0;
    score = 0;
  }

  private void Update() {
    runTime += Time.deltaTime;
  }

  public int GetPoints() {
    return score;
  }

  public void SpendPoints(int points) {
    score -= points;
  }

  public int[] GetRunStats() {
    return new int[] {score, (int)runTime, roomsCleared, floorsCleared, kills, damage, switches};
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