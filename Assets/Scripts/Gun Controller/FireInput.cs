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

  // TODO: Update this to the new input system if neccessary (not at the moment)

  [Header("Fire Axis")]
  [SerializeField][Tooltip("Defaults to 'Fire1' if axis is empty or doesn't exist.")] private string _fireInputAxis;
  [Tooltip("Fires on the first frame FireInputAxis is 1.")] public UnityEvent _OnFireInputDown;
  [Tooltip("Fires on every frame FireInputAxis is 1, except for the first frame.")] public UnityEvent _OnFireInputHeld;
  [Tooltip("Fires on the frame after the last frame FireInputAxis is 1.")] public UnityEvent _OnFireInputUp;

  private bool _hadFireInputLastFrame;

  [Header("Reload Axis")]
  [SerializeField] private string _reloadInputAxis;
  public UnityEvent _OnReloadInputDown;
  private bool _hadReloadInputLastFrame;
  

  private void Start() {
    _hadFireInputLastFrame = false;
    _hadReloadInputLastFrame = false;
    if (!IsAxisSetup("Fire1")) Debug.LogError("FireInput: Fire1 is not setup! FireInputAxis has no default, and will be unassigned if the axis name is invalid.");
    if (IsAxisSetup(_fireInputAxis)) return;
    else _fireInputAxis = "Fire1";
    if (IsAxisSetup(_reloadInputAxis)) {
      return;
    }
    else {
      Debug.LogError("FireInpput: ReloadInputAxis is unassigned!");
    }
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
    CheckFireInput();
    CheckReloadInput();
  }

  private void CheckFireInput() {
    if (!_hadFireInputLastFrame) {
      if (Input.GetAxisRaw(_fireInputAxis) == 1) {
        _hadFireInputLastFrame = true;
        _OnFireInputDown?.Invoke();
      }
      else {
        _hadFireInputLastFrame = false;
      }
    }
    else if (_hadFireInputLastFrame) {
      if (Input.GetAxisRaw(_fireInputAxis) == 1) {
        _hadFireInputLastFrame = true;
        _OnFireInputHeld?.Invoke();
      }
      else {
        _hadFireInputLastFrame = false;
        _OnFireInputUp?.Invoke();
      }
    }
  }

  private void CheckReloadInput()
  {
    if (!_hadReloadInputLastFrame) {
      if (Input.GetAxisRaw(_reloadInputAxis) == 1) {
        _hadReloadInputLastFrame = true;
        _OnReloadInputDown?.Invoke();
      }
      else {
        _hadReloadInputLastFrame = false;
      }
    }
    else if (_hadReloadInputLastFrame) {
      if (Input.GetAxisRaw(_reloadInputAxis) == 1) {
        _hadReloadInputLastFrame = true;
      }
      else {
        _hadReloadInputLastFrame = false;
      }
    }
  }
}
