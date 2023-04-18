using UnityEngine;

public class DoorCameraFollow : MonoBehaviour
{
  [HideInInspector] public Transform door;
  [HideInInspector] public Transform otherDoor;

  public Transform playerCamera;

  private void Awake() {
    playerCamera = Camera.main.transform;
    door = transform.parent;
  }

  private void Update() {
    if (otherDoor) {
        Vector3 playerOffsetFromDoor = otherDoor.position - playerCamera.position;
            playerOffsetFromDoor = Quaternion.AngleAxis(Quaternion.Angle(door.rotation, otherDoor.rotation), otherDoor.position) * playerOffsetFromDoor;
        playerOffsetFromDoor = new Vector3(playerOffsetFromDoor.x,
            -playerOffsetFromDoor.y * (float)(1 / 0.4),
            playerOffsetFromDoor.z * (float)(1 / 0.4));



      transform.localPosition = playerOffsetFromDoor;

      transform.rotation = Quaternion.Euler(door.rotation.eulerAngles.x, door.rotation.eulerAngles.y + 180, door.rotation.eulerAngles.z) * Quaternion.Euler(new Vector3(playerCamera.transform.rotation.eulerAngles.x, playerCamera.transform.rotation.eulerAngles.y, playerCamera.transform.rotation.eulerAngles.z));
    }
  }
}
