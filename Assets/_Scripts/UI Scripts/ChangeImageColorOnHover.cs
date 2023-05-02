using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ChangeImageColorOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
    ChangeImageColor(_hoverColor, _duration);
    OnHover?.Invoke();
  }
  
  public void OnPointerExit(PointerEventData pointerEventData) {
    ChangeImageColor(_baseColor, _duration);
    OnHoverExit?.Invoke();
  }

  private void ChangeImageColor(Color targetColor, float duration) {
    StartCoroutine(ChangeImageColorCoroutine(targetColor, duration));
  }
  private IEnumerator ChangeImageColorCoroutine(Color targetColor, float duration) {
    float time = 0;
    Color startColor = _image.color;
    while (time < duration)
    {
      time += Time.deltaTime;
      _image.color = Color.Lerp(startColor, targetColor, _easingCurve.Evaluate(time / duration));
      yield return null;
    }
    _image.color = targetColor;
  }
}
