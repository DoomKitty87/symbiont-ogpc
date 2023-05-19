using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class ChangeOpacityOnHover : MonoBehaviour, IPointerEnterHandler
{
  [Header("References")]
  [SerializeField] private CanvasGroup _canvasGroup;
  [SerializeField] private Image _mouseOverImage;
  [Header("Settings")]
  [SerializeField] private float _minOpacity;
  [SerializeField] private float _maxOpacity;
  [SerializeField] private AnimationCurve _easingCurve;
  [SerializeField] private float _inDuration;
  [SerializeField] private float _outDuration;

  [Header("Events")]
  public UnityEvent _OnOpacityChangeComplete;

  private void OnValidate() {
    if (_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();
  }

  private void Start() {
    _canvasGroup.alpha = _minOpacity;      
  }

  public void OnPointerEnter(PointerEventData pointerEventData) {
    OpacityIn(false);
    StartCoroutine(CheckForNotMouseOver());
  }
  private IEnumerator CheckForNotMouseOver() {
    while (true) {
      // All other ways to test if the pointer was over the UI Image had problems when the mouse moved too fast
      if (!RectTransformUtility.RectangleContainsScreenPoint(_mouseOverImage.rectTransform, Input.mousePosition)) {
        OpacityOut(false);
        break;
      }
      yield return null;
    }
  }

  private void OpacityIn(bool resetAlpha) {
    if (resetAlpha) {
      StartCoroutine(OpacityInOutCoroutine(_minOpacity, _maxOpacity, _inDuration));
    }
    else {
      StartCoroutine(OpacityInOutCoroutine(_canvasGroup.alpha, _maxOpacity, _inDuration));
    }
  }
  private void OpacityOut(bool resetAlpha) {
    if (resetAlpha) {
      StartCoroutine(OpacityInOutCoroutine(_maxOpacity, _minOpacity, _outDuration));
    }
    else {
      StartCoroutine(OpacityInOutCoroutine(_canvasGroup.alpha, _minOpacity, _outDuration));
    }
  }
  private IEnumerator OpacityInOutCoroutine(float startAlpha, float targetAlpha, float duration) {
    float time = 0;
    while (time < duration) {
      time += Time.unscaledDeltaTime;
      _canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, _easingCurve.Evaluate(time / duration));
      yield return null;
    }
    _canvasGroup.alpha = targetAlpha;
    _OnOpacityChangeComplete?.Invoke();
  }
}
