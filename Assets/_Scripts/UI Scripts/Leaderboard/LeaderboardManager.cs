using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{

  private LeaderboardConnect leaderboardConnectionManager;
  private TextMeshProUGUI text;
  private float timer;

  [SerializeField] private GameObject leaderboardPlacePrefab;

  void Start() {
    leaderboardConnectionManager = GameObject.FindGameObjectWithTag("ConnectionManager").GetComponent<LeaderboardConnect>();
    //text = GetComponent<TextMeshProUGUI>();

    //StartCoroutine(UpdateLeaderboard());
    TriggerReloadLeaderboard();
  }

  void Update() {
    timer += Time.deltaTime;
    if (timer > 10) {
      timer = 0;
      StartCoroutine(ReloadLeaderboard());
    }
  }

  public void TriggerReloadLeaderboard() {
    StartCoroutine(ReloadLeaderboard());
  }

  private IEnumerator ReloadLeaderboard() {
    List<Score> scores = leaderboardConnectionManager.RetrieveScores();
    while (scores.Count == 0) {
      yield return new WaitForSeconds(0.05f);
    }
    scores.Sort((s1, s2) => s1.score.CompareTo(s2.score));
    // Need to add places to the leaderboard
    int place = 1;
    foreach (Score sc in scores) {
      GameObject tmp = Instantiate(leaderboardPlacePrefab, Vector3.zero, Quaternion.identity, transform);
      tmp.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = place.ToString();
      tmp.transform.GetChild(0).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = sc.name;
      tmp.transform.GetChild(0).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = (sc.length / 60).ToString() + ":" + ((sc.length % 60 < 10) ? "0" : "") + (sc.length % 60).ToString();
      tmp.transform.GetChild(0).GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = sc.score.ToString();
      place++;
    }
  }

  //Fetches leaderboard information from the MySQL database and posts it to a UI element.
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