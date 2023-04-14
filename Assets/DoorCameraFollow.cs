using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCameraFollow : MonoBehaviour
{
    public Transform playerCamera;
    public Transform door;
    public Transform otherDoor;

    private void Update() {
        Vector3 playeroOffsetFromDoor = playerCamera.position - otherDoor.position;
        transform.position = door.position + playeroOffsetFromDoor;

        float angularDifferenceBetweenTwoDoors = Quaternion.Angle(door.rotation, otherDoor.rotation);
        Quaternion doorRotationDifference = Quaternion.AngleAxis(angularDifferenceBetweenTwoDoors, Vector3.up);
        Vector3 newCameraDirection = doorRotationDifference * playerCamera.forward;
        transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
	}
}
