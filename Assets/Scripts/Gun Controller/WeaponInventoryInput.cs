using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// This is going to be temporary, since it doesn't seem very scaleable or nice to use.
// Ideally, the input could activate a UI element, which would then call a function in WeaponInventory
// but that's something to worry about later.

// TODO: Update this to the new Input System if necessary.

[System.Serializable]
public class OnInputDownEvent : UnityEvent<int> {}

[System.Serializable]
public class WeaponInventoryInputContainer
{
  public string _inputAxisName;
  public int _inventoryIndexReference;
  public bool _validInput;

  [Tooltip("Fires on the first frame InputAxis is 1. OnInputDown(inventoryIndexReference)")] public OnInputDownEvent _OnInputDown;
  [Tooltip("Fires on every frame InputAxis is 1, except for the first frame.")] public UnityEvent _OnInputHeld;
  [Tooltip("Fires on the frame after the last frame InputAxis is 1.")] public UnityEvent _OnInputUp;

  public bool _hadInputLastFrame;
}

public class WeaponInventoryInput : MonoBehaviour
{
  [SerializeField] private List<WeaponInventoryInputContainer> _inputAxes;

  private void Start() {
    foreach (WeaponInventoryInputContainer inputContainer in _inputAxes) {
      if (InputAxisIsValid(inputContainer._inputAxisName)) {
        inputContainer._validInput = true;
      }
      else {
        inputContainer._validInput = false;
        Debug.LogError("WeaponInventoryInput: " + inputContainer._inputAxisName + " is not a valid input axis!");
      }

      inputContainer._hadInputLastFrame = false;
    }
  }

  private void Update() {
    foreach (WeaponInventoryInputContainer inputContainer in _inputAxes) {
      if (inputContainer._validInput == false) continue;
      if (inputContainer._hadInputLastFrame == false) {
        if (Input.GetAxisRaw(inputContainer._inputAxisName) == 1) {
          inputContainer._hadInputLastFrame = true;
          inputContainer._OnInputDown?.Invoke(inputContainer._inventoryIndexReference);
          continue;
        }
        else {
          inputContainer._hadInputLastFrame = false;
          continue;
        }
      }
      if (inputContainer._hadInputLastFrame) {
        if (Input.GetAxisRaw(inputContainer._inputAxisName) == 1) {
          inputContainer._hadInputLastFrame = true;
          inputContainer._OnInputHeld?.Invoke();
        }
        else {
          inputContainer._hadInputLastFrame = false;
          inputContainer._OnInputUp?.Invoke();
        }
      }
    }
  }

  private bool InputAxisIsValid(string axisName) {
    try {
      Input.GetAxis(axisName);
      return true;
    }
    catch {
      return false;
    }
  }
}
