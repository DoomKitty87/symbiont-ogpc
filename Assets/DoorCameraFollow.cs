using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCameraFollow : MonoBehaviour
{
    [HideInInspector] public Transform door;
    [HideInInspector] public Transform otherDoor;

    private Transform playerCamera;

    private void Awake() {
        playerCamera = Camera.main.transform;
    }

    private void Update() {
        Vector3 playeroOffsetFromDoor = playerCamera.position - otherDoor.position;
        transform.position = door.position + playeroOffsetFromDoor;

        float angularDifferenceBetweenTwoDoors = Quaternion.Angle(door.rotation, otherDoor.rotation);
        Quaternion doorRotationDifference = Quaternion.AngleAxis(angularDifferenceBetweenTwoDoors, Vector3.up);
        Vector3 newCameraDirection = doorRotationDifference * playerCamera.forward;
        transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
	}
}
