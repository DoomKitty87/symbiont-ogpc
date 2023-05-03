using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalHandler : MonoBehaviour
{
  public void TriggerFloorSwitch() {
    GameObject.FindGameObjectWithTag("Persistent").GetComponent<FloorManager>().ClearedFloor();
  }
}
