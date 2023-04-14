using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayControls : MonoBehaviour
{

  //Uses the old input system - migrate
  void Start()
  {
        
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.P)) {
      TogglePathOverlay();
    }
  }

  private void TogglePathOverlay() {

  }

  public void ForceOffOverlays() {
    GetComponent<PathRenderer>().StopOverlay();
  }
}
