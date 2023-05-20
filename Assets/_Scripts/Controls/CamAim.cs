using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamAim : MonoBehaviour
{

  [Header("Sensitivity")]
  public float _horizontalSens = 1f;
  public float _verticalSens = 1f;
  public float _smoothSpeed = 0.8f;
  [Header("Constraints")]
  public bool _constrainX;
  public float _minX = -90f;
  public float _maxX = 90f;
  public bool _constrainY;
  public float _minY = -180f;
  public float _maxY = 180f;
  [HideInInspector] public float _deltaX = 0;
  [HideInInspector] public float _deltaY = 0;

  [HideInInspector] public float _rotX;
  [HideInInspector] public float _rotY;

  private Vector3 _rot;

  void Start() {
    Cursor.lockState = CursorLockMode.Locked;
    transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
  }

  void OnEnable() {
    _rotY = Mathf.Lerp(_minX, _maxX, 0.5f);
  }

  void Update() {
    float oldY = _rotY;
    float oldX = _rotX;
    _rotY += Input.GetAxis("Mouse X") * _horizontalSens * ReturnCorrectValue("CONTROLS_INVERT_X");
    _rotX += Input.GetAxis("Mouse Y") * _verticalSens * ReturnCorrectValue("CONTROLS_INVERT_Y");
    
    if (_constrainX) {
      _rotX = Mathf.Clamp(_rotX, _minX, _maxX);
    }
    if (_constrainY) {
      _rotY = Mathf.Clamp(_rotY, _minY, _maxY);
    }

    _deltaY = _rotY - oldY;
    _deltaX = _rotX - oldX;

    // Negative bc up is negative for some reason
    _rot = Vector3.Slerp(_rot, new Vector3(_rotX, _rotY, transform.localRotation.z), _smoothSpeed * Time.deltaTime);
    transform.localRotation = Quaternion.Euler(_rot);
  }

  private int ReturnCorrectValue(string playerPref) {
    if (PlayerPrefs.GetInt(playerPref) == 1) return -1;
    else return 1;
  }
}