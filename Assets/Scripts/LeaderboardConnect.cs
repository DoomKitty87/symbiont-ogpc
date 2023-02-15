using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System;

public class LeaderboardConnect : MonoBehaviour
{

  private const string highscoreURL = "https://csprojectdatabase.000webhostapp.com/scores.php";

  public List<Score> RetrieveScores() {
    List<Score> scores = new List<Score>();
    StartCoroutine(DoRetrieveScores(scores));
    return scores;
  }

  void Awake() {
    if (GameObject.FindGameObjectsWithTag("ConnectionManager").Length > 1) Destroy(this.gameObject);
    DontDestroyOnLoad(this.gameObject);
  }

  public void PostScores(string name, int score) {
    StartCoroutine(DoPostScores(name, score));
  }

  private IEnumerator DoRetrieveScores(List<Score> scores) {
    WWWForm form = new WWWForm();
    form.AddField("retrieve_leaderboard", "true");

    using (UnityWebRequest www = UnityWebRequest.Post(highscoreURL, form)) {
      yield return www.SendWebRequest();
      if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError) {
        Debug.Log(www.error);
      }
      else {
        Debug.Log("Successfully retrieved score!");
        string contents = www.downloadHandler.text;
        using (StringReader reader = new StringReader(contents)) {
          string line;
          while ((line = reader.ReadLine()) != null) {
            Score entry = new Score();
            entry.name = line;
            try {
              entry.score = Int32.Parse(reader.ReadLine());
            }
            catch(Exception e) {
              Debug.Log("Invalid score: " + e);
              continue;
            }
            scores.Add(entry);
          }
        }
      }
    }
  }

  private IEnumerator DoPostScores(string name, int score) {
    WWWForm form = new WWWForm();
    form.AddField("post_leaderboard", "true");
    form.AddField("name", name);
    form.AddField("score", score);

    using (UnityWebRequest www = UnityWebRequest.Post(highscoreURL, form)) {
      yield return www.SendWebRequest();
      if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError) {
        Debug.Log(www.error);
      }
      else {
        Debug.Log("Successfully posted score!");
      }
    }
  }
}

public struct Score {
  public string name;
  public int score;
}