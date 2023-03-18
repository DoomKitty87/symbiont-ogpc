using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotTargeting : MonoBehaviour
{

  public bool controlled;
  public GameObject playerInhabiting;

  private void Start() {
    playerInhabiting = GameObject.FindGameObjectWithTag("Inhabited");
    InvokeRepeating("TargetingLoop", 0, 0.1f);
  }

  private void TargetingLoop() {
    RaycastHit hit;
    if (Physics.Raycast(transform.position, playerInhabiting.transform.position - transform.position, out hit)) {
      if (hit.collider.gameObject.CompareTag("Inhabited")) {

      }
    }
  }
}