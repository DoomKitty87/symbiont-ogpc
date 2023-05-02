using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ChangeTextColorOnHover : MonoBehaviour, IPointerEnterHandler
{
  [Header("References")]
  [SerializeField] private TextMeshProUGUI _text; 
  [Header("Settings")]
  [SerializeField] private Color _baseColor;
  [SerializeField] private Color _hoverColor;
  [Header("Easing")]
  [SerializeField] private AnimationCurve _easingCurve;
  [SerializeField] private float _duration;

  public UnityEvent OnHover;
  public UnityEvent OnHoverExit;

  private void Awake() {
    _text = GetComponent<TextMeshProUGUI>();
  }
  public void OnPointerEnter(PointerEventData pointerEventData) {
    ChangeToHoverColor();
    StartCoroutine(CheckForNotMouseOver());
  }
  private IEnumerator CheckForNotMouseOver() {
    while (true) {
      // All other ways to test if the pointer was over the UI Image had problems when the mouse moved too fast
      if (!RectTransformUtility.RectangleContainsScreenPoint(_text.rectTransform, Input.mousePosition)) {
        ChangeToBaseColor();
        break;
      }
      yield return null;
    }
  }
  public void ChangeToBaseColor() {
    ChangeTextColor(_hoverColor, _baseColor, _duration);
    OnHoverExit?.Invoke();
  }
  public void ChangeToHoverColor() {
    ChangeTextColor(_baseColor, _hoverColor, _duration);
    OnHover?.Invoke();
  }

  private void ChangeTextColor(Color startColor, Color targetColor, float duration) {
    StopCoroutine(ChangeTextColorCoroutine(startColor, targetColor, duration));
    StartCoroutine(ChangeTextColorCoroutine(startColor, targetColor, duration));
  }
  private IEnumerator ChangeTextColorCoroutine(Color startColor, Color targetColor, float duration) {
    float time = 0;
    while (time < duration)
    {
      time += Time.deltaTime;
      _text.color = Color.Lerp(startColor, targetColor, _easingCurve.Evaluate(time / duration));
      yield return null;
    }
    _text.color = targetColor;
  }
}
