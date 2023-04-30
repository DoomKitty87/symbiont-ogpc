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
      Vector3 distanceBetweenObject = otherDoor.position - playerCamera.position;

      transform.position = door.position + (door.rotation * Quaternion.Inverse(otherDoor.rotation) * new Vector3(distanceBetweenObject.x, -distanceBetweenObject.y, distanceBetweenObject.z)); // Other door position + Offset between player camera and door * Rotation offset
      transform.rotation = Quaternion.Euler(0, 180f, 0) * (door.rotation * Quaternion.Inverse(otherDoor.rotation)) * playerCamera.rotation; // Initial offset of 180 degrees on Y axis * Difference between two door rotations + Main camera rotaiton
    }
  }

    private Transform GetCurrentActivePlayer() {
    return GameObject.FindWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject
            .transform.GetChild(2).GetChild(0).GetChild(0).transform;
  }
}
