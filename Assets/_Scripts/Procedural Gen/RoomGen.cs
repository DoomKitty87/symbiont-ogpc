using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Room
{

  public GameObject prefab;
  public Vector3 roomSize;
  public Vector3[] doorways;
  public Vector3[] connectedEntrance;
}

public class RoomGen : MonoBehaviour
{

  [SerializeField] private float roomNumber;
  [SerializeField] private Room[] genRooms;

  private void Start() {
    GenerateDungeon();
  }

  private void Update() {

  }

  private void GenerateDungeon() {
    Room lastRoom = null;
    Vector3 lastPos = null;

    for (int i = 0; i < roomNumber; i++) {
      Room roomChoice = genRooms[Random.range(0, genRooms.Length - 1)];
      GameObject instantiatedRoom = Instantiate(roomChoice.prefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
      bool connected = false;
      if (i == 0) connected = true;
      int tryingDoor = -1;
      while (tryingDoor == -1 || tryingDoor == lastRoom.connectedEntrance) tryingDoor = Random.range(0, lastRoom.doorways.Length);
      instantiatedRoom.transform.position = lastRoom.doorways[tryingDoor] + lastPos + roomChoice.doorways[(Random.value < 0.5f) ? 0 : 1];
      lastRoom = roomChoice;
      lastPos = instantiatedRoom.transform.position;
    }
  }
}