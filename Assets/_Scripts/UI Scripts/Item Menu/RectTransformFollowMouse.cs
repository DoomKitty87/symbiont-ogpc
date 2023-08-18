using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class RectTransformFollowMouse : MonoBehaviour
{
  [SerializeField] private RectTransform _rectTransform;

  private void OnValidate() {
    _rectTransform = gameObject.GetComponent<RectTransform>();
  }

  private void Update() {
    Rect rect = _rectTransform.rect;
    _rectTransform.transform.position = (Vector2)Input.mousePosition + new Vector2(rect.width / 2,rect.height / 2);
  }
}
