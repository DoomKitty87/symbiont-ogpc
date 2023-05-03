using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeElementInOut : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private CanvasGroup _canvasGroup;
  [Header("Settings")]
  [SerializeField] private AnimationCurve _easingCurve;
  [SerializeField] private float _inDuration;
  [SerializeField] private float _outDuration;

  public void FadeIn(bool resetAlpha) {
    if (resetAlpha) {
      StartCoroutine(FadeElementInOutCoroutine(0, 1, _inDuration));
    } else {
      StartCoroutine(FadeElementInOutCoroutine(_canvasGroup.alpha, 1, _inDuration));
    }
    _canvasGroup.interactable = true;
  }
  public void FadeOut(bool resetAlpha) {
    if (resetAlpha) {
      StartCoroutine(FadeElementInOutCoroutine(1, 0, _outDuration));
    } else {
      StartCoroutine(FadeElementInOutCoroutine(_canvasGroup.alpha, 0, _outDuration));
    }
    _canvasGroup.interactable = false;
  }
  private IEnumerator FadeElementInOutCoroutine(float startAlpha, float targetAlpha, float duration) {
    float time = 0;
    while (time < duration) {
      time += Time.deltaTime;
      _canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, _easingCurve.Evaluate(time / duration));
      yield return null;
    }
    _canvasGroup.alpha = targetAlpha;
  }
}
