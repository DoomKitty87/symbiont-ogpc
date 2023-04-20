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
            Vector3 distanceBetweenObject = playerCamera.position - otherDoor.position;

            transform.position = door.position + (otherDoor.rotation * door.rotation * distanceBetweenObject);
            transform.rotation = (door.rotation * playerCamera.rotation) * otherDoor.rotation;
        }
  }
}
