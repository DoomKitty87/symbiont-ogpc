using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingObject : MonoBehaviour
{

  public void SwitchedTo() {
    GameObject.FindGameObjectWithTag("Persistent").GetComponent<FloorManager>().ClearedFloor();
  }
}