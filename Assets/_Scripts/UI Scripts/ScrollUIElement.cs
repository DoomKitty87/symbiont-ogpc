using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollUIElement : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private RectTransform _rectTransform;
  
  [Header("Settings")]
  [Header("Start Position will be set to current position if left at 0,0")]
  [SerializeField] private Vector2 _startPosition;
  [SerializeField] private Vector2 _scrollVelocity;
  [SerializeField] private float _minX; [SerializeField] private float _maxX;
  [SerializeField] private float _minY; [SerializeField] private float _maxY;
  
  [Header("Debug")]
  [SerializeField] private bool _debug;

  private void Start() {
    if (_startPosition == Vector2.zero) _startPosition = _rectTransform.anchoredPosition;
  }

  private void Update() {
    _rectTransform.anchoredPosition += _scrollVelocity * Time.deltaTime;
    if (_rectTransform.anchoredPosition.x < _minX) _rectTransform.anchoredPosition = new Vector2(_maxX, _rectTransform.anchoredPosition.y);
    if (_rectTransform.anchoredPosition.x > _maxX) _rectTransform.anchoredPosition = new Vector2(_minX, _rectTransform.anchoredPosition.y);
    if (_rectTransform.anchoredPosition.y < _minY) _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, _maxY);
    if (_rectTransform.anchoredPosition.y > _maxY) _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, _minY);
  }

  #if UNITY_EDITOR
  private void OnDrawGizmos() {
    // Draw a yellow sphere at the start position, a green sphere at the current position, and a blue line for the min and max X and a red line for the min and max Y
    Gizmos.color = Color.yellow;
    Gizmos.DrawSphere(_startPosition, 0.1f);
    Gizmos.color = Color.green;
    Gizmos.DrawSphere(_rectTransform.anchoredPosition, 0.1f);
    Gizmos.color = Color.blue;
    Gizmos.DrawLine(new Vector2(_minX, _minY), new Vector2(_maxX, _minY));
    Gizmos.DrawLine(new Vector2(_minX, _maxY), new Vector2(_maxX, _maxY));
    Gizmos.color = Color.red;
    Gizmos.DrawLine(new Vector2(_minX, _minY), new Vector2(_minX, _maxY));
    Gizmos.DrawLine(new Vector2(_maxX, _minY), new Vector2(_maxX, _maxY));
  }
  #endif
}
