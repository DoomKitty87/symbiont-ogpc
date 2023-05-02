using UnityEngine;

public class DoorCameraFollow : MonoBehaviour {
   public Transform door;
   public Transform otherDoor;
   public Transform playerCamera;

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

      transform.position = door.position + (door.rotation * Quaternion.Inverse(otherDoor.rotation) *
        new Vector3(distanceBetweenObject.x, -distanceBetweenObject.y, distanceBetweenObject.z)); // Position of door + door rotation offset + distance offset between doors

      transform.rotation = Quaternion.Euler(0, 180f, 0) * (door.rotation * Quaternion.Inverse(otherDoor.rotation)) * playerCamera.rotation; // Initial 180 degree y rotation + door rotation offset + player camera rotation
    }

    Transform GetCurrentActivePlayer() {
      return GameObject.FindWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject.transform.GetChild(1).GetChild(0).GetChild(0).transform;
    }
  }
}
