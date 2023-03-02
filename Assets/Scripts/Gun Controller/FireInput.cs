using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class FireInput : MonoBehaviour
{
  // NOTE: This is seperated out from FireRaycast because of the different inputs possible with just the
  // input axis. Eventually, FireRaycast will be able to use these to change the input style of different
  // weapons.

  // Maybe FireRaycast should have this as a dependancy and use C# Events instead of UnityEvents later on;
  // only if we see a major performance impact though.

  [Header("Input Axis")]
  [SerializeField][Tooltip("Defaults to 'Fire1' if axis is empty or doesn't exist.")] private string _fireInputAxis;
  [Tooltip("Fires on the first frame FireInputAxis is 1.")] public UnityEvent _OnFireInputDown;
  [Tooltip("Fires on every frame FireInputAxis is 1, except for the first frame.")] public UnityEvent _OnFireInputHeld;
  [Tooltip("Fires on the frame after the last frame FireInputAxis is 1.")] public UnityEvent _OnFireInputUp;
  private bool _hadInputLastFrame;

  private void Start() {
    _hadInputLastFrame = false;
    if (!IsAxisSetup("Fire1")) Debug.LogError("FireInput: Fire1 is not setup! FireInputAxis has no default, and will be unassigned if the axis name is invalid.");
    if (IsAxisSetup(_fireInputAxis)) return;
    else _fireInputAxis = "Fire1";
  }

  private bool IsAxisSetup(string axisName) {
    try {
      Input.GetAxis(axisName);
      return true;
    }
    catch {
      return false;
    }
  }
  
  private void Update() {
    if (!_hadInputLastFrame) {
      if (Input.GetAxisRaw(_fireInputAxis) == 1) {
        _hadInputLastFrame = true;
        _OnFireInputDown?.Invoke();
      }
      else {
        _hadInputLastFrame = false;
      }
    }
    else if (_hadInputLastFrame) {
      if (Input.GetAxisRaw(_fireInputAxis) == 1) {
        _hadInputLastFrame = true;
        _OnFireInputHeld?.Invoke();
      }
      else {
        _hadInputLastFrame = false;
        _OnFireInputUp?.Invoke();
      }
    }
  }
}
