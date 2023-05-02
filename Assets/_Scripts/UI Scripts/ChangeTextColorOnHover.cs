using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ChangeTextColorOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
    ChangeTextColor(_hoverColor, _duration);
    OnHover?.Invoke();
  }
  
  public void OnPointerExit(PointerEventData pointerEventData) {
    ChangeTextColor(_baseColor, _duration);
    OnHoverExit?.Invoke();
  }

  private void ChangeTextColor(Color targetColor, float duration) {
    StartCoroutine(ChangeTextColorCoroutine(targetColor, duration));
  }
  private IEnumerator ChangeTextColorCoroutine(Color targetColor, float duration) {
    float time = 0;
    Color startColor = _text.color;
    while (time < duration)
    {
      time += Time.deltaTime;
      _text.color = Color.Lerp(startColor, targetColor, _easingCurve.Evaluate(time / duration));
      yield return null;
    }
    _text.color = targetColor;
  }
}
