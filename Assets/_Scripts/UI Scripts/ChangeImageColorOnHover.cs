using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ChangeImageColorOnHover : MonoBehaviour, IPointerEnterHandler
{
  [Header("References")]
  [SerializeField] private Image _image; 
  [Header("Settings")]
  [SerializeField] private Color _baseColor;
  [SerializeField] private Color _hoverColor;
  [Header("Easing")]
  [SerializeField] private AnimationCurve _easingCurve;
  [SerializeField] private float _duration;

  public UnityEvent OnHover;
  public UnityEvent OnHoverExit;

  private void Awake() {
    _image = GetComponent<UnityEngine.UI.Image>();
  }
  public void OnPointerEnter(PointerEventData pointerEventData) {
    ChangeToHoverColor();
    StartCoroutine(CheckForNotMouseOver());
  }
  private IEnumerator CheckForNotMouseOver() {
    while (true) {
      // All other ways to test if the pointer was over the UI Image had problems when the mouse moved too fast
      if (!RectTransformUtility.RectangleContainsScreenPoint(_image.rectTransform, Input.mousePosition)) {
        ChangeToBaseColor();
        break;
      }
      yield return null;
    }
  }
  public void ChangeToBaseColor() {
    ChangeImageColor(_hoverColor, _baseColor, _duration);
    OnHoverExit?.Invoke();
  }
  public void ChangeToHoverColor() {
    ChangeImageColor(_baseColor, _hoverColor, _duration);
    OnHover?.Invoke();
  }

  private void ChangeImageColor(Color startColor, Color targetColor, float duration) {
    StopCoroutine(ChangeImageColorCoroutine(startColor, targetColor, duration));
    StartCoroutine(ChangeImageColorCoroutine(startColor, targetColor, duration));
  }
  private IEnumerator ChangeImageColorCoroutine(Color startColor, Color targetColor, float duration) {
    float time = 0;
    while (time < duration)
    {
      time += Time.unscaledDeltaTime;
      _image.color = Color.Lerp(startColor, targetColor, _easingCurve.Evaluate(time / duration));
      yield return null;
    }
    _image.color = targetColor;
  }
}
