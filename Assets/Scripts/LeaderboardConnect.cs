using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System;

public class LeaderboardConnect : MonoBehaviour
{

  private const string highscoreURL = "https://csprojectdatabase.000webhostapp.com/scores.php";

  //Returns the leaderboard scores for external scripts.
  public List<Score> RetrieveScores() {
    List<Score> scores = new List<Score>();
    StartCoroutine(DoRetrieveScores(scores));
    return scores;
  }

  void Awake() {
    if (GameObject.FindGameObjectsWithTag("ConnectionManager").Length > 1) Destroy(this.gameObject);
    DontDestroyOnLoad(this.gameObject);
  }

  public bool PostScores(int score) {
    if (!GetComponent<LoginConnect>().IsLoggedIn()) return false;
    string authPassword = GetComponent<LoginConnect>().GetActiveAuthPass();
    string name = GetComponent<LoginConnect>().GetActiveAccountName();
    StartCoroutine(DoPostScores(name, authPassword, score));
    return true;
  }

  //Fetches the leaderboard data from the web server, and formats it into Score objects.
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

  //Executes a leaderboard update for a player score.
  public IEnumerator DoPostScores(string name, string password, int score) {
    WWWForm form = new WWWForm();
    form.AddField("post_leaderboard", "true");
    form.AddField("name", name);
    form.AddField("password", password);
    form.AddField("score", score);
    print(name);
    print(password);
    print(score);
    using (UnityWebRequest www = UnityWebRequest.Post(highscoreURL, form)) {
      yield return www.SendWebRequest();
      if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError) {
        Debug.Log(www.error);
        yield return false;
      }
      else {
        Debug.Log("Successfully posted score!");
        Debug.Log(www.result);
        Debug.Log(www.downloadHandler.text);
        yield return true;
      }
    }
  }
}

//Data structure to store score data.
public struct Score {
  public string name;
  public int score;
}