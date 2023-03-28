using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewSwitcher : MonoBehaviour
{
  // On a trigger, activate an effect (tbd, fov increase animation for now) + sounds and change the
  // _currentPlayerCamera to the camera of the currently targeted switchable object.
  //
  // Visually show the currently targeted object through an outline.
  // (Someone please figure out how to do those)
  //
  // Determine the currently targeted switchable object through a raycast,
  // and determine whether it is switchable through a "switchable" tag.
  //
  // A component on the target, SwitchableObject, should take care of the tagging.
  // SwitchableObject, once activated through this script after switching the cameras,
  // should turn on and off scripts, activate player inputs, etc.
  //
  // This should probably also switch off the player input for the current target.
  
  [Header("Camera")]
  [Header("This needs to be assigned before runtime to an 'inital' switchable object.")]
  [SerializeField] private Camera _currentPlayerCamera;
  [SerializeField] private float _startFov;
  [Header("Switching Effect")]
  [SerializeField] private AnimationCurve _effectCurve;  
  
  private void Start() {
    foreach (Camera camera in Camera.allCameras) {
      camera.enabled = false;
    }
    _currentPlayerCamera.enabled = true;
  }

  private void Update() {
    
  }
}
