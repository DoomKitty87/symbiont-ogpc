using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayControls : MonoBehaviour
{

  //Uses the old input system - migrate

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.JoystickButton3)) {
      TogglePathOverlay();
    }
  }

  private void TogglePathOverlay() {
    if (GetComponent<PathRenderer>().active) {
      GetComponent<PathRenderer>().StopOverlay();
    }
    else {
      GetComponent<PathRenderer>().StartOverlay();
    }
  }

  public void ForceOffOverlays() {
    GetComponent<PathRenderer>().StopOverlay();
  }
}
