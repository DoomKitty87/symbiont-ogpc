using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(CanvasGroup))]
public class FadeElementInOut : MonoBehaviour
{
  [Header("References")]
  public CanvasGroup _canvasGroup;
  [Header("Settings")]
  [SerializeField] private AnimationCurve _easingCurve;
  [SerializeField] private float _inDuration;
  [SerializeField] private float _outDuration;
  [Header("Events")]
  public UnityEvent _OnFadeComplete;

  void OnValidate() {
    if (_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();
  }

  public void FadeIn(bool resetAlpha) {
    if (resetAlpha) {
      _canvasGroup.interactable = false;
      _canvasGroup.blocksRaycasts = false;
      StartCoroutine(FadeElementInOutCoroutine(0, 1, _inDuration));
    } else {
      StartCoroutine(FadeElementInOutCoroutine(_canvasGroup.alpha, 1, _inDuration));
    }
    _canvasGroup.interactable = true;
    _canvasGroup.blocksRaycasts = true;
  }
  public void FadeOut(bool resetAlpha) {
    if (resetAlpha) {
      _canvasGroup.interactable = true;
      _canvasGroup.blocksRaycasts = true;
      StartCoroutine(FadeElementInOutCoroutine(1, 0, _outDuration));
    } else {
      StartCoroutine(FadeElementInOutCoroutine(_canvasGroup.alpha, 0, _outDuration));
    }
    _canvasGroup.interactable = false;
    _canvasGroup.blocksRaycasts = false;
  }
  private IEnumerator FadeElementInOutCoroutine(float startAlpha, float targetAlpha, float duration) {
    float time = 0;
    while (time < duration) {
      time += Time.unscaledDeltaTime;
      _canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, _easingCurve.Evaluate(time / duration));
      yield return null;
    }
    _canvasGroup.alpha = targetAlpha;
    _OnFadeComplete?.Invoke();
  }
}
