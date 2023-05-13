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
  [SerializeField] private int _startPositionIndex;
  [SerializeField] private AnimationCurve _easeCurve;
  [SerializeField] private float _duration;

  public UnityEvent OnEaseToPositionComplete;

  private bool _moving = false;

  private void Start() {
    if (_rectTransformToMove == null) {
      Debug.LogError("EaseElementToPosition: RectTransform to move is null!");
    }
    if (_positions.Count == 0) {
      Debug.LogError("EaseElementToPosition: No positions to move to!");
    }
    _startPositionIndex = 0;
    EaseToPosition(_startPositionIndex);   
  }

  public void EaseToPosition(int positionIndex) {
    if (positionIndex < 0 || positionIndex >= _positions.Count) {
      Debug.LogError("EaseElementToPosition: Position index is out of range!");
      return;
    }
    StartCoroutine(EaseElementToPositionCoroutine(_positions[positionIndex], _duration));
    _startPositionIndex = positionIndex;
  }
  public void EaseToNextPosition() {
    if (_moving) return;
    int nextPositionIndex = _startPositionIndex + 1;
    if (nextPositionIndex >= _positions.Count) {
      nextPositionIndex = 0;
    }
    StartCoroutine(EaseElementToPositionCoroutine(_positions[nextPositionIndex], _duration));
    _startPositionIndex = nextPositionIndex;
  }
  private IEnumerator EaseElementToPositionCoroutine(RectTransform targetPosition, float duration) {
    _moving = true;
    float time = 0;
    while (time < duration) {
      _rectTransformToMove.anchoredPosition = Vector2.Lerp(_rectTransformToMove.anchoredPosition, targetPosition.anchoredPosition, _easeCurve.Evaluate(time / duration));
      yield return null;
      time += Time.deltaTime;
    }
    _rectTransformToMove.anchoredPosition = targetPosition.anchoredPosition;
    OnEaseToPositionComplete?.Invoke();
    _moving = false;
  }
}
