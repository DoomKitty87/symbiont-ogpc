using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAim : MonoBehaviour
{
 
  [SerializeField] private float horizontalSens = 1f;
  [SerializeField] private float verticalSens = 1f;

  private float rotX;
  private float rotY;
  private Transform cam;

  void Start() {
    Cursor.lockState = CursorLockMode.Locked;
    cam = GameObject.FindGameObjectWithTag("VCam").transform;
  }

  void Update() {
    rotY += Input.GetAxis("Mouse X") * horizontalSens;
    rotX += Input.GetAxis("Mouse Y") * verticalSens;

    cam.eulerAngles = new Vector3(-rotX, rotY, 0);
  }
}