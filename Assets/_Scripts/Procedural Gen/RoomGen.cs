using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Room
{

  public GameObject prefab;
  public Vector3 roomSize;
  public Vector3[] doorways;
  public Vector3[] doorRotations;
  public int connectedEntrance;
}

public class RoomGen : MonoBehaviour
{

  [SerializeField] private float roomNumber;
  [SerializeField] private Room[] genRooms;
  [SerializeField] private GameObject doorFiller;

  private void Start() {
    GenerateDungeon();
  }

  private void GenerateDungeon() {
    Room lastRoom;
    Vector3 lastPos;
    Quaternion lastRot;

    Room _roomChoice = genRooms[Random.Range(0, genRooms.Length - 1)];
    GameObject _instantiatedRoom = Instantiate(_roomChoice.prefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
    lastRoom = _roomChoice;
    lastPos = _instantiatedRoom.transform.position; 
    lastRot = _instantiatedRoom.transform.rotation;

    for (int i = 0; i < roomNumber - 1; i++) {
      Room roomChoice = genRooms[Random.Range(0, genRooms.Length - 1)];
      GameObject instantiatedRoom = Instantiate(roomChoice.prefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
      int tryingDoor = Random.Range(0, lastRoom.doorways.Length);
      while (tryingDoor == -1 || tryingDoor == lastRoom.connectedEntrance) tryingDoor = Random.Range(0, lastRoom.doorways.Length);
      instantiatedRoom.transform.position = lastRoom.doorways[tryingDoor] + lastPos + roomChoice.doorways[(Random.value < 0.5f) ? 0 : 1];
      roomChoice.connectedEntrance = tryingDoor;
      lastRoom = roomChoice;
      lastPos = instantiatedRoom.transform.position;
      lastRot = instantiatedRoom.transform.rotation;
      for (int n = 0; n < lastRoom.doorways.Length; n++) {
        // if (n != lastRoom.connectedEntrance && n != tryingDoor) Instantiate(doorFiller, lastPos + lastRoom.doorways[n], lastRot * lastRoom.doorRotations[n]);
      }
    }
  }
}