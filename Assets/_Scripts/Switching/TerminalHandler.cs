using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalHandler : MonoBehaviour
{
  
  // private GameObject room;

  // public void Start() {
  //   while (!room.CompareTag("Room")) {
  //     room = room.transform.parent.gameObject;
  //   }
  // }

  // public void Update() {
  //   if (room.transform.GetChild(0).childCount > 2) {
  //     gameObject.tag = "DisabledSwitchable";
  //   }
  //   else gameObject.tag = "SwitchableObject";
  // }

  public void TriggerFloorSwitch() {
    GameObject.FindGameObjectWithTag("ItemMenu").GetComponent<ItemMenuOnFloorEnd>().ClearedFloor(); 
  }
}
