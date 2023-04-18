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
      transform.localPosition = playerOffsetFromDoor;

      // Quaternion fixedRotation = Quaternion.Euler(door.eulerAngles.x, door.eulerAngles.y, door.eulerAngles.z);

      //float angularDifferenceBetweenTwoDoors = Quaternion.Angle(fixedRotation, otherDoor.rotation);
      //Quaternion doorRotationDifference = Quaternion.AngleAxis(angularDifferenceBetweenTwoDoors, Vector3.forward);
      //Vector3 newCameraDirection = doorRotationDifference * -playerCamera.forward;
      //transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
      Debug.Log(playerCamera.transform.position);

      transform.rotation = playerCamera.transform.rotation * 
        Quaternion.Euler(door.rotation.eulerAngles.x, door.rotation.eulerAngles.y + 180, door.rotation.eulerAngles.z);
    }
  }
}
