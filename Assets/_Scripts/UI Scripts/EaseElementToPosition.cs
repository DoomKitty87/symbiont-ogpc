using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class EaseElementToPosition : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private RectTransform _rectTransformToMove;
  [Header("Settings")]
  [SerializeField] private List<RectTransform> _positions = new();
  [SerializeField] private AnimationCurve _easeCurve;
  [SerializeField] private float _duration;

  public UnityEvent OnEaseToPositionComplete;

  public void EaseToPosition(int positionIndex)
  {
    StartCoroutine(EaseElementToPositionCoroutine(_positions[positionIndex], _duration));
  }
  private IEnumerator EaseElementToPositionCoroutine(RectTransform targetPosition, float duration)
  {
    float time = 0;
    while (time < duration)
    {
      time += Time.deltaTime;
      _rectTransformToMove.anchoredPosition = Vector2.Lerp(_rectTransformToMove.anchoredPosition, targetPosition.anchoredPosition, _easeCurve.Evaluate(time / duration));
      yield return null;
    }
    _rectTransformToMove.anchoredPosition = targetPosition.anchoredPosition;
    OnEaseToPositionComplete?.Invoke();
  }
}
