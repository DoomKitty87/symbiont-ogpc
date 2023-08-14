using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class CustomTimer : MonoBehaviour
{
  [Header("Text References")]
  [SerializeField] private TextMeshProUGUI _timerText;
  
  [Header("UI")]
  [SerializeField] private Slider _timerSlider;
  [SerializeField] private Image _timerImage;
  
  public enum TimerType { Timer, Stopwatch }
  
  [Header("Settings")]
  [SerializeField] private TimerType _timerType;
  [SerializeField] private bool _ignoreTimescale;

  [Header("Timer Specific Settings")]
  [SerializeField] private float _timerDuration;
  [SerializeField] private bool _countUp;

  [Header("Events")]
  public UnityEvent _OnTimerEnd;

  public void StartTimer() {
    if (_timerType == TimerType.Timer) {
      if (_timerSlider != null) _timerSlider.maxValue = _timerDuration; _timerSlider.value = _timerDuration; 
      if (_timerImage != null) _timerImage.fillAmount = 1;
    }

    switch (_timerType) {
      case TimerType.Timer:
        if (_countUp) StartCoroutine(CountUpTimer());
        else StartCoroutine(CountdownTimer());
        break;
      case TimerType.Stopwatch:
        StartCoroutine(Stopwatch());
        break;
    }
  }

  private IEnumerator CountdownTimer() {
    float timeLeft = _timerDuration;
    while (timeLeft > 0) {
      timeLeft -= _ignoreTimescale ? Time.unscaledDeltaTime : Time.deltaTime;
      if (_timerText != null) {
        _timerText.text = string.Format("{0}.{1}", Mathf.Clamp(Mathf.FloorToInt(timeLeft % 60), 0, 61), Mathf.Clamp(Mathf.FloorToInt((timeLeft * 100) % 100), 0, 101).ToString("00"));
      }
      if (_timerSlider != null) {
        _timerSlider.value = timeLeft;
      } 
      if (_timerImage != null) {
        _timerImage.fillAmount = timeLeft / _timerDuration;
      }
      yield return null;
    }
    if (_timerText != null) {
      _timerText.text = string.Format("{0}.{1}", Mathf.Clamp(Mathf.FloorToInt(timeLeft % 60), 0, 61), Mathf.Clamp(Mathf.FloorToInt((timeLeft * 100) % 100), 0, 101).ToString("00"));
    }
    _OnTimerEnd?.Invoke();
  }

  private IEnumerator CountUpTimer() {
    float timePassed = 0;
    while (timePassed < _timerDuration) {
      timePassed += _ignoreTimescale ? Time.unscaledDeltaTime : Time.deltaTime;
      if (_timerText != null) {
        _timerText.text = string.Format("{0}.{1}", Mathf.Clamp(Mathf.FloorToInt(timePassed % 60), 0, 61), Mathf.Clamp(Mathf.FloorToInt((timePassed * 100) % 100), 0, 101).ToString("00"));
      }
      if (_timerSlider != null) {
        _timerSlider.value = timePassed;
      }
      if (_timerImage != null) {
        _timerImage.fillAmount = timePassed / _timerDuration;
      } 
      yield return null;
    }
    if (_timerText != null) {
      _timerText.text = string.Format("{0}.{1}", Mathf.Clamp(Mathf.FloorToInt(timePassed % 60), 0, 61), Mathf.Clamp(Mathf.FloorToInt((timePassed * 100) % 100), 0, 101).ToString("00"));
    }
    _OnTimerEnd?.Invoke();
  }

  private IEnumerator Stopwatch() {
    float timePassed = 0;
    while (true) {
      timePassed += Time.deltaTime;
      if (_timerText != null) {
        _timerText.text = string.Format("{0}:{1}.{2}", Mathf.FloorToInt(timePassed / 60), Mathf.Clamp(Mathf.FloorToInt(timePassed % 60), 0, 61), Mathf.Clamp(Mathf.FloorToInt((timePassed * 100) % 100), 0, 101).ToString("00"));
      }
      yield return null;
    }
  }
}

