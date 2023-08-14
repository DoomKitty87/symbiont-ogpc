using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class CanvasFollowMouse : MonoBehaviour
{
  [SerializeField] private Canvas _canvas;

  private void OnValidate() {
    _canvas = gameObject.GetComponent<Canvas>();
  }

  private void Update() {
    _canvas.transform.position = (Vector2)Input.mousePosition + new Vector2(_canvas.GetComponent<RectTransform>().rect.width / 2,_canvas.GetComponent<RectTransform>().rect.height / 2);
  }
}
