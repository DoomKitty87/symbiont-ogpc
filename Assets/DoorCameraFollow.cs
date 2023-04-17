using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCameraFollow : MonoBehaviour
{
  public Transform door;
  public Transform otherDoor;

  public Transform playerCamera;

  private void Awake() {
    playerCamera = Camera.main.transform;
    door = transform.parent;
  }

  private void Update() {
    if (otherDoor) {
      Vector3 playerOffsetFromDoor = playerCamera.position - otherDoor.position;
      transform.position = door.position + playerOffsetFromDoor;

      float angularDifferenceBetweenTwoDoors = Quaternion.Angle(door.rotation, otherDoor.rotation);
      Quaternion doorRotationDifference = Quaternion.AngleAxis(angularDifferenceBetweenTwoDoors, Vector3.up);
      Vector3 newCameraDirection = doorRotationDifference * -playerCamera.forward;
      transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
    }
  }
}
