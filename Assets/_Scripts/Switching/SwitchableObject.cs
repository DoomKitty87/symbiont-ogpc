using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchableObject : MonoBehaviour
{
  // This script should contain a camera and an audio listener that are disabled and enabled 
  // when switching, 

  [Header("Note: To deem this object switchable, its tag will be overridden.")]
  
  [Header("Selection Raycast")]

  public Transform _raycastOrigin;
  public Vector3 _raycastOriginOffset;
  public float _raycastDistance;
  public LayerMask _layerMask;
  public bool _showRaycast;

  [Header("Object References")]
  [Header("Note: The FOV Effect will only play for the first camera in this list.")] // Could make it so that the list has a selection of whether to use the effect or not, but the KIS rule says no
  public List<Camera> _objectCameras = new();
  public AudioListener _objectAudioListener;

  [Header("Events")]
  public UnityEvent _OnSwitchedTo;
  public UnityEvent _OnSwitchedAway;  
  public UnityEvent _OnSelected;
  public UnityEvent _OnUnselected;

  private void Awake() {
    gameObject.tag = "SwitchableObject";
    ChangeStateOfAllCamerasTo(false);
    _objectAudioListener.enabled = false;
    _OnSwitchedAway?.Invoke();
  }
  public void Selected(bool isSelected) {
    if (isSelected) {
      _OnSelected?.Invoke();
    }
    else {
      _OnUnselected?.Invoke();
    }
  }
  public void SwitchTo() {
    _OnSwitchedTo?.Invoke();
    ChangeStateOfAllCamerasTo(true);
    _objectAudioListener.enabled = true;
  }
  public void SwitchAway() {
    _OnSwitchedAway?.Invoke();
    ChangeStateOfAllCamerasTo(false);
    _objectAudioListener.enabled = false;
  }
  private void ChangeStateOfAllCamerasTo(bool enabled) {
    for (int i = 0; i < _objectCameras.Count; i++) {
      _objectCameras[i].enabled = enabled;
    }
  }
}
