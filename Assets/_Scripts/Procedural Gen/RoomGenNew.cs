using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenNew : MonoBehaviour
{

  //Rooms should have child categories of:
  //Things in room
  //Cameras for doors
  //Material plane (for camera)
  //Door fillers (for doors not in use)

  [SerializeField] private GameObject[] genRooms;
  [SerializeField] private int floorRooms;

  private void Start() {
    GenerateFloor(floorRooms);
  }

  public void GenNewFloor() {
    for (int i = 0; i < transform.childCount; ++i) {
      Destroy(transform.GetChild(i));
    }
    GenerateFloor(floorRooms);
  }

  private void GenerateFloor(int rooms) {
    float traveled = 0;
    for (int i = 0; i < rooms; ++i) {
      
      GameObject instantiatedRoom = Instantiate(genRooms[Random.Range(0, genRooms.Length)], new Vector3(traveled, 0, 0), Quaternion.identity, transform);
      traveled += 200;

      if (instantiatedRoom.transform.GetChild(2).childCount > 2) {
        for (int i = instantiatedRoom.transform.GetChild(2).childCount - 2, i > 0, --i) {
          if (instantiatedRoom.transform.GetChild(2).GetChild(i - 1).gameObject.activeSelf) {
            instantiatedRoom.transform.GetChild(1).GetChild(i - 1).gameObject.SetActive(false);
            instantiatedRoom.transform.GetChild(2).GetChild(i - 1).gameObject.SetActive(false);
            instantiatedRoom.transform.GetChild(3).GetChild(i - 1).gameObject.SetActive(true);
          }
          else ++i;
        }
      }
    }
  }
}