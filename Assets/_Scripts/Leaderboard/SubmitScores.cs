using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class SubmitScores : MonoBehaviour
{
  [Header("This requires a GameObject with tag 'Handler' containing ScoreTracker")]
  private CinemachineDollyCart dolly;
  private LeaderboardConnect leaderboardConnectionManager;
  private LoginConnect accountConnectionManager;
  private bool triggered;

  void Start() {
    //dolly = GameObject.FindGameObjectWithTag("DollyFollower").GetComponent<CinemachineDollyCart>();
    leaderboardConnectionManager = GameObject.FindGameObjectWithTag("ConnectionManager").GetComponent<LeaderboardConnect>();
    accountConnectionManager = GameObject.FindGameObjectWithTag("ConnectionManager").GetComponent<LoginConnect>();
  }

  void Update() {
    /*
    if (dolly.m_Position >= dolly.m_Path.PathLength && triggered == false) {
      triggered = true;
      EndLevel();
    }
    */
  }

  public void FinishedRun() {
    Cursor.lockState = CursorLockMode.None;
    StartCoroutine(SubmitScore());
  }

  //Checks for high score and submits it to leaderboard.
  private IEnumerator SubmitScore() {
    if (!accountConnectionManager.IsLoggedIn()) {
      SceneManager.LoadScene("MainMenu");
      yield break;
    }
    print("made it past first break statement");
    //int score = IMPLEMENT
    int length = 0;
    int score = (int)GameObject.FindGameObjectWithTag("Handler").GetComponent<ScoreTracker>().GetPoints();
    List<Score> scores = leaderboardConnectionManager.RetrieveScores();
    while (scores.Count == 0) {
      yield return new WaitForSeconds(0.05f);
    }
    foreach (Score i in scores) {
      if (i.name == accountConnectionManager.GetActiveAccountName()) {
        if (score < i.score) {
          SceneManager.LoadScene("MainMenu");
          yield break;
        }
      }
    }
    print("made it past second break statement");
    bool response = leaderboardConnectionManager.PostScores(score, length);
    while (!response) {
      yield return new WaitForSeconds(0.05f);
    }
    SceneManager.LoadScene("MainMenu");
  }
}
