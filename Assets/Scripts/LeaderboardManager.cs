using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{

  private LeaderboardConnect leaderboardConnectionManager;
  private TextMeshProUGUI text;

  void Start() {
    leaderboardConnectionManager = GameObject.FindGameObjectWithTag("ConnectionManager").GetComponent<LeaderboardConnect>();
    text = GetComponent<TextMeshProUGUI>();

    StartCoroutine(UpdateLeaderboard());
  }

  private IEnumerator UpdateLeaderboard() {
    List<Score> scores = leaderboardConnectionManager.RetrieveScores();
    while (scores.Count == 0) {
      yield return new WaitForSeconds(0.05f);
    }
    string newText = "";
    foreach (Score i in scores) {
      newText += (i.name + ": " + i.score + "\n");
    }
    text.text = newText;
  }
}