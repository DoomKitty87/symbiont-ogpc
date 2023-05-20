using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[RequireComponent(typeof(FadeElementInOut))]
public class LoseScreenManager : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private FadeElementInOut _fadeElementInOut;
  [Header("Text References")]
  [SerializeField] private TextMeshProUGUI _flavorText;
  [SerializeField] private TextMeshProUGUI _timeElapsedText;
  [SerializeField] private TextMeshProUGUI _roomsClearedText;
  [SerializeField] private TextMeshProUGUI _floorsClearedText;
  [SerializeField] private TextMeshProUGUI _adversariesDisabledText;
  [SerializeField] private TextMeshProUGUI _damageDoneText;
  [SerializeField] private TextMeshProUGUI _enemiesSwitchedToText;
  [SerializeField] private TextMeshProUGUI _totalScore;
  [SerializeField] private TextMeshProUGUI _scoresSubmittedText;
  public void Initalize(int[] stats, string flavorText) {
    _flavorText.text = flavorText;
    _totalScore.text = stats[0].ToString();
    _timeElapsedText.text = ((int)(stats[1] / 60)).ToString() + ":" + (stats[1] % 60 < 10 ? "0" : "") + (stats[1] % 60).ToString();
    _roomsClearedText.text = stats[2].ToString();
    _floorsClearedText.text = stats[3].ToString();
    _adversariesDisabledText.text = stats[4].ToString();
    _damageDoneText.text = stats[5].ToString();
    _enemiesSwitchedToText.text = stats[6].ToString();
    if (GameObject.FindGameObjectWithTag("ConnectionManager") != null) {
      if (GameObject.FindGameObjectWithTag("ConnectionManager").GetComponent<LoginConnect>().IsLoggedIn()) {
        _scoresSubmittedText.text = "Score submitted to leaderboard as " + GameObject.FindGameObjectWithTag("ConnectionManager").GetComponent<LoginConnect>().GetActiveAccountName() + ".";
      }
      else {
        _scoresSubmittedText.text = "Not logged in- scores not submitted to leaderboard.";
      }
    } else  _scoresSubmittedText.text = "Not logged in- scores not submitted to leaderboard.";
    _fadeElementInOut.FadeIn(true);
  }
}
