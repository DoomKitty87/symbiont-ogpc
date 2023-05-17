using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScript : MonoBehaviour
{

  [SerializeField] private RectTransform _creditsContent;
  [SerializeField] private float _creditsScrollSpeed;
  [SerializeField] private float _creditsScrollDistance;

  private Vector3 _startPosition;

  private void Start() {
    _startPosition = _creditsContent.transform.localPosition;
  }

  public void StartCredits() {
    StopCoroutine(StartCreditsCoroutine());
    StartCoroutine(StartCreditsCoroutine());
  }

  private IEnumerator StartCreditsCoroutine() {
    _creditsContent.transform.localPosition = _startPosition;
    while (_startPosition.y - _creditsContent.transform.localPosition.y < _creditsScrollDistance) {
      _creditsContent.transform.localPosition += new Vector3(0, _creditsScrollSpeed * Time.deltaTime, 0);
      yield return null;
    }
  }
}