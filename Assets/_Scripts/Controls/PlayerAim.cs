using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAim : MonoBehaviour
{
  [Header("References")]

  [Tooltip("Will be set to the attached gameObject if unassigned.")][SerializeField] private Transform _objectToAimX;
  [Tooltip("Will be set to the attached gameObject if unassigned.")][SerializeField] private Transform _objectToAimY;
  [Header("(In case we want to seperate the rotations; ie. X rotation is for the camera, " + "\n" + "and Y rotation is for the player.)")]
  [Header("Sensitivity")]
  public float _horizontalSens = 1f;
  public float _verticalSens = 1f;
  [Header("Constraints")]
  public bool _constrainX;
  public float _minX = -90f;
  public float _maxX = 90f;
  public bool _constrainY;
  public float _minY = -180f;
  public float _maxY = 180f;

  private float _rotX;
  private float _rotY;

  void Start() {
    Cursor.lockState = CursorLockMode.Locked;
    if (_objectToAimX == null) _objectToAimX = transform;
    if (_objectToAimY == null) _objectToAimY = transform;
  }

  void Update() {
    _rotY += Input.GetAxis("Mouse X") * _horizontalSens;
    _rotX += Input.GetAxis("Mouse Y") * _verticalSens;

    if (_constrainX) {
      _rotX = Mathf.Clamp(_rotX, _minX, _maxX);
    }
    if (_constrainY) {
      _rotY = Mathf.Clamp(_rotY, _minY, _maxY);
    }

    // Negative bc up is negative for some reason
    _objectToAimX.eulerAngles = new Vector3(-_rotX, _objectToAimX.rotation.eulerAngles.y, _objectToAimX.rotation.eulerAngles.z);
    _objectToAimY.eulerAngles = new Vector3(_objectToAimY.rotation.eulerAngles.x, _rotY, _objectToAimY.rotation.eulerAngles.z);
  }
}