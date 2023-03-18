using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Room
{

  public GameObject prefab;
  public Vector3 roomSize;
  public Vector3[] doorways;
  public int connectedEntrance;
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
    Room lastRoom;
    Vector3 lastPos = new Vector3(0, 0, 0);

    Room _roomChoice = genRooms[Random.Range(0, genRooms.Length - 1)];
    GameObject _instantiatedRoom = Instantiate(_roomChoice.prefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
    lastRoom = _roomChoice;
    lastPos = _instantiatedRoom.transform.position;

    for (int i = 0; i < roomNumber - 1; i++) {
      Room roomChoice = genRooms[Random.Range(0, genRooms.Length - 1)];
      GameObject instantiatedRoom = Instantiate(roomChoice.prefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
      int tryingDoor = Random.Range(0, lastRoom.doorways.Length);
      while (tryingDoor == -1 || tryingDoor == lastRoom.connectedEntrance) tryingDoor = Random.Range(0, lastRoom.doorways.Length);
      instantiatedRoom.transform.position = lastRoom.doorways[tryingDoor] + lastPos + roomChoice.doorways[(Random.value < 0.5f) ? 0 : 1];
      lastRoom = roomChoice;
      lastPos = instantiatedRoom.transform.position;
    }
  }
}