using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayHealth : MonoBehaviour
{
  [Header("UI References")]
  [SerializeField][Tooltip("This is optional.")] private TextMeshProUGUI _textElement;
  [SerializeField] private string _textElementPrefix;
  [SerializeField][Range(0, 10)] private int _healthTextDecimalPlaces;
  [SerializeField][Tooltip("This is optional.")] private Slider _healthSlider;

  [Header("Tweening Values")]
  [SerializeField] private AnimationCurve _easeCurve;
  [SerializeField] private float _easeDuration;

  // Debounce
  private bool _tweeningHealth;

  private void Start() {
    if (_textElement == null & _healthSlider == null) {
      Debug.LogWarning("DisplayHealth: Both UI Elements are null. Are you missing a reference?");
    }
  }
  public void OnHealthInitialize(float maxHealth) {
    if (_textElement != null) {
      _textElement.text = _textElementPrefix + maxHealth.ToString();
    }
    if (_healthSlider != null) {
      _healthSlider.value = 1;
    }
  }
  public void OnHealthChanged(float initHealth, float currentHealth, float maxHealth) {
    if (this.enabled == false) return;
    if (_textElement != null) {
      StartCoroutine(TweenTextValue(_textElement, initHealth, currentHealth, _easeDuration, _healthTextDecimalPlaces));
    }
    if (_healthSlider != null) {
      StartCoroutine(TweenSlider(_healthSlider, currentHealth / maxHealth, _easeDuration));
    }
  }
  private IEnumerator TweenSlider(Slider slider, float targetValue, float duration) {
    float timeElapsed = 0;
    float initSliderValue = slider.value;
    while (timeElapsed < duration) {
      timeElapsed += Time.deltaTime;
      float t = timeElapsed / duration;
      t = _easeCurve.Evaluate(t);
      slider.value = Mathf.Lerp(initSliderValue, targetValue, t);
      yield return null;
    }
    slider.value = targetValue;
  }
  private IEnumerator TweenTextValue(TextMeshProUGUI text, float startValue, float targetValue, float duration, int decimalPlaces) {
    float timeElapsed = 0;
    // float initSliderValue = slider.value;
    while (timeElapsed < duration) {
      timeElapsed += Time.deltaTime;
      float t = timeElapsed / duration;
      t = _easeCurve.Evaluate(t);
      text.text = _textElementPrefix + Math.Round(Mathf.Lerp(startValue, targetValue, t), decimalPlaces).ToString();
      yield return null;
    }
    text.text = _textElementPrefix + targetValue.ToString();
  }
}



