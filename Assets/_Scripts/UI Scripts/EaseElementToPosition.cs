using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class EaseElementToPosition : MonoBehaviour
{
  [SerializeField] private Vector3 _targetPosition;
  [SerializeField] private float _duration;

  public UnityEvent OnEaseToPositionComplete;
  public void EaseToPosition(Vector3 targetPosition, float duration)
  {
    // Just so we can see the values in the inspector
    _targetPosition = targetPosition;
    _duration = duration;

    StartCoroutine(EaseElementToPositionCoroutine(targetPosition, duration));
  }
  private IEnumerator EaseElementToPositionCoroutine(Vector3 targetPosition, float duration)
  {
    float time = 0;
    Vector3 startPosition = transform.position;
    while (time < duration)
    {
      time += Time.deltaTime;
      transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
      yield return null;
    }
    transform.position = targetPosition;
    OnEaseToPositionComplete?.Invoke();
  }
}
