using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class SubmitScores : MonoBehaviour
{

  private CinemachineTrackedDolly dolly;
  private LeaderboardConnect leaderboardConnectionManager;
  private LoginConnect accountConnectionManager;
  private bool triggered;

  void Start() {
    dolly = GameObject.FindGameObjectWithTag("VCam").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>();
    leaderboardConnectionManager = GameObject.FindGameObjectWithTag("ConnectionManager").GetComponent<LeaderboardConnect>();
    accountConnectionManager = GameObject.FindGameObjectWithTag("ConnectionManager").GetComponent<LoginConnect>();
  }

  void Update() {
    if (dolly.m_PathPosition >= dolly.m_Path.PathLength && triggered == false) {
      triggered = true;
      StartCoroutine(SubmitScore("testing"));
    }
  }

  //Checks for high score and submits it to leaderboard.
  private IEnumerator SubmitScore(string submissionName) {
    if (accountConnectionManager.GetActiveAccountName == "") {
      SceneManager.LoadScene("MainMenu");
      yield break;
    }
    int score = (int)GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PointTracker>().GetPoints();
    List<Score> scores = leaderboardConnectionManager.RetrieveScores();
    while (scores.Count == 0) {
      yield return new WaitForSeconds(0.05f);
    }
    foreach (Score i in scores) {
      if (i.name == submissionName) {
        if (score < i.score) {
          SceneManager.LoadScene("MainMenu");
          yield break;
        }
      }
    }
    bool response = leaderboardConnectionManager.PostScores(submissionName, score);
    while (!response) {
      yield return new WaitForSeconds(0.05f);
    }
    SceneManager.LoadScene("MainMenu");
  }

}