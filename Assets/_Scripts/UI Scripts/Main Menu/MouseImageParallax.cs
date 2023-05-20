using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseImageParallax : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private RectTransform _imageTransform;
  [Header("Settings")]
  [Tooltip("Distance the Rect Transform moves when mouse is on an edge of the screen")][SerializeField] private float _parallaxDistance;
  [SerializeField] private float _parallaxSpeed;

  private void Start() {
    if (_imageTransform == null) {
      Debug.LogError("MouseImageParallax: Image Transform is null!");
    }
  }

  private void Update() {
    EaseToPosition(GetTargetPosition(Input.mousePosition));
  }

  private void EaseToPosition(Vector2 targetPos) {
    _imageTransform.anchoredPosition = Vector2.Lerp(_imageTransform.anchoredPosition, targetPos, Time.deltaTime * _parallaxSpeed);
  }

  private Vector2 GetTargetPosition(Vector2 mousePos) {
    return new Vector2(
      Mathf.Clamp((mousePos.x / Screen.width) * _parallaxDistance, -Mathf.Abs(_parallaxDistance), Mathf.Abs(_parallaxDistance)),
      Mathf.Clamp((mousePos.y / Screen.height) * _parallaxDistance, -Mathf.Abs(_parallaxDistance), Mathf.Abs(_parallaxDistance))
    );
  }
} 
