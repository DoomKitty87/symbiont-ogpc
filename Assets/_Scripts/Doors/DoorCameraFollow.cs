using UnityEngine;

public class DoorCameraFollow : MonoBehaviour
{
  [HideInInspector] public Transform door;
  [HideInInspector] public Transform otherDoor;
  [HideInInspector] public Transform playerCamera;

  private Transform tempPlayerCamera;

  private void Awake() {
    door = transform.parent;
  }

  private void Update() {

    // Finds and sets the current active player object
    if (tempPlayerCamera != GetCurrentActivePlayer()) {
      tempPlayerCamera = GetCurrentActivePlayer();
      playerCamera = GetCurrentActivePlayer();
    }

    if (otherDoor) {
      Vector3 distanceBetweenObject = playerCamera.position - otherDoor.position;

      transform.position = door.position + (otherDoor.rotation * door.rotation * distanceBetweenObject);
      transform.rotation = (door.rotation * playerCamera.rotation) * otherDoor.rotation;
    }
  }

  private Transform GetCurrentActivePlayer() {
    return GameObject.FindWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject.transform.parent.transform;
  }
}
