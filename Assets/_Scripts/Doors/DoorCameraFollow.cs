using UnityEngine;

public class DoorCameraFollow : MonoBehaviour
{
  [HideInInspector] public Transform door;
  [HideInInspector] public Transform otherDoor;
  [HideInInspector] public Transform playerCamera;

  private void Awake() {
    playerCamera = Camera.main.transform;
    door = transform.parent;
  }

  private void Update() {
    if (otherDoor) {
      Vector3 distanceBetweenObject = playerCamera.position - otherDoor.position;

      transform.position = door.position + (otherDoor.rotation * door.rotation * distanceBetweenObject);
      transform.rotation = (door.rotation * playerCamera.rotation) * otherDoor.rotation;
    }
  }
}
