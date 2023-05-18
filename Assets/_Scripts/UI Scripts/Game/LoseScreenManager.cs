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
  [SerializeField] private TextMeshProUGUI _adversariesDisabledText;
  [SerializeField] private TextMeshProUGUI _damageDoneText;
  [SerializeField] private TextMeshProUGUI _enemiesSwitchedToText;
  [SerializeField] private TextMeshProUGUI _totalScore;
  public void Initalize(int[] stats, string flavorText) {
    _flavorText.text = flavorText;
    _totalScore.text = stats[0].ToString();
    _timeElapsedText.text = stats[1].ToString();
    _roomsClearedText.text = stats[2].ToString();
    _adversariesDisabledText.text = stats[3].ToString();
    _damageDoneText.text = stats[4].ToString();
    _enemiesSwitchedToText.text = stats[5].ToString();
    _fadeElementInOut.FadeIn(true);
  }
}
