using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCameraFollow : MonoBehaviour
{
  public Transform door;
  public Transform otherDoor;

  private Transform playerCamera;

  private void Awake() {
    playerCamera = Camera.main.transform;
    door = transform.parent;
  }

  private void Update() {
    if (otherDoor) {
      Vector3 playerOffsetFromDoor = otherDoor.position - playerCamera.position;
      transform.localPosition = door.position - playerOffsetFromDoor;
      // ((playerOffsetFromDoor.normalized + new Vector3(door.rotation.eulerAngles.x, door.rotation.eulerAngles.y, door.rotation.eulerAngles.z)).normalized
      // * playerOffsetFromDoor.magnitude);
      // Debug.Log(new Vector3(door.rotation.eulerAngles.x, door.rotation.eulerAngles.y, door.rotation.eulerAngles.z).normalized + playerOffsetFromDoor.normalized);
      // Debug.Log(playerOffsetFromDoor.normalized);
      // ((door.rotation.eulerAngles.normalized + playerOffsetFromDoor.normalized).normalized * playerOffsetFromDoor.magnitude);

      Quaternion fixedRotation = Quaternion.Euler(door.eulerAngles.x- 180, door.eulerAngles.y, door.eulerAngles.z);

      float angularDifferenceBetweenTwoDoors = Quaternion.Angle(fixedRotation, otherDoor.rotation);
      Quaternion doorRotationDifference = Quaternion.AngleAxis(angularDifferenceBetweenTwoDoors, Vector3.up);
      Vector3 newCameraDirection = doorRotationDifference * -playerCamera.forward;
      transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
    }
  }
}
