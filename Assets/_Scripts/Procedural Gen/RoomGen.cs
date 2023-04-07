using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGen : MonoBehaviour
{

  //[SerializeField] private float roomNumber;
  //[SerializeField] private GameObject[] genRooms;
  //[SerializeField] private GameObject doorFiller;

  //private List<GameObject> generatedRooms = new List<GameObject>();

  //private void Start() {
  //  GenerateDungeon(3);
  //}

  //public void TriggerNewRoom() {
  //  GenerateRoom();
  //}

  //private void GenerateRoom() {
  //  GameObject roomChoice = genRooms[Random.Range(0, genRooms.Length)];
  //  GameObject instantiatedRoom = Instantiate(roomChoice, new Vector3(0, 0, 0), Quaternion.identity, transform);

  //  int chosenDoor = Random.Range(0, lastRoom.transform.GetChild(1).childCount);
  //  int connectingDoor = Random.Range(0, instantiatedRoom.transform.GetChild(1).childCount);
      
  //  instantiatedRoom.transform.position = lastRoom.transform.GetChild(1).GetChild(chosenDoor).position - instantiatedRoom.transform.GetChild(1).GetChild(connectingDoor).position;
  //  while (Mathf.Abs((lastRoom.transform.position - instantiatedRoom.transform.position).magnitude) < 2) {
  //    instantiatedRoom.transform.RotateAround(lastRoom.transform.GetChild(1).GetChild(chosenDoor).position, Vector3.up, 180);
  //  }

  //  loop:
  //    foreach (GameObject room in generatedRooms) {
  //      if (Mathf.Abs(Vector3.Distance(room.transform.position, instantiatedRoom.transform.position)) < 2) {
  //        chosenDoor = Random.Range(0, lastRoom.transform.GetChild(1).childCount);
  //        connectingDoor = Random.Range(0, instantiatedRoom.transform.GetChild(1).childCount);
  //        instantiatedRoom.transform.position = lastRoom.transform.GetChild(1).GetChild(chosenDoor).position - instantiatedRoom.transform.GetChild(1).GetChild(connectingDoor).position;
  //        while (Mathf.Abs(Vector3.Distance(lastRoom.transform.position, instantiatedRoom.transform.position)) < 2) {
  //          instantiatedRoom.transform.RotateAround(lastRoom.transform.GetChild(1).GetChild(chosenDoor).position, Vector3.up, 180);
  //        }
  //        goto loop;
  //      }
  //    }

  //  Destroy(instantiatedRoom.transform.GetChild(1).GetChild(connectingDoor).gameObject);
  //  generatedRooms.Add(lastRoom);
  //  foreach (GameObject door in lastRoom.transform.GetChild(1).childCount) {
  //    lastRoom.transform.GetChild(1).GetChild(door).gameObject.setActive(true);
  //    //Will cause undesired behavior but I'll deal with that in a bit
  //  }

  //  Destroy(generatedRooms[0]);
  //  generatedRooms.RemoveAt(0);

  //  lastRoom = instantiatedRoom;
  //}

  //private void GenerateDungeon(int rooms) {
  //  GameObject lastRoom;

  //  GameObject _roomChoice = genRooms[Random.Range(0, genRooms.Length)];
  //  GameObject _instantiatedRoom = Instantiate(_roomChoice, new Vector3(0, 0, 0), Quaternion.identity, transform);
  //  lastRoom = _instantiatedRoom;

  //  for (int i = 0; i < rooms - 1; i++) {
  //    GameObject roomChoice = genRooms[Random.Range(0, genRooms.Length)];
  //    GameObject instantiatedRoom = Instantiate(roomChoice, new Vector3(0, 0, 0), Quaternion.identity, transform);

  //    int chosenDoor = Random.Range(0, lastRoom.transform.GetChild(1).childCount);
  //    int connectingDoor = Random.Range(0, instantiatedRoom.transform.GetChild(1).childCount);
      
  //    instantiatedRoom.transform.position = lastRoom.transform.GetChild(1).GetChild(chosenDoor).position - instantiatedRoom.transform.GetChild(1).GetChild(connectingDoor).position;
  //    while (Mathf.Abs((lastRoom.transform.position - instantiatedRoom.transform.position).magnitude) < 2) {
  //      instantiatedRoom.transform.RotateAround(lastRoom.transform.GetChild(1).GetChild(chosenDoor).position, Vector3.up, 180);
  //    }

  //    loop:
  //      foreach (GameObject room in generatedRooms) {
  //        if (Mathf.Abs(Vector3.Distance(room.transform.position, instantiatedRoom.transform.position)) < 2) {
  //          chosenDoor = Random.Range(0, lastRoom.transform.GetChild(1).childCount);
  //          connectingDoor = Random.Range(0, instantiatedRoom.transform.GetChild(1).childCount);

  //          instantiatedRoom.transform.position = lastRoom.transform.GetChild(1).GetChild(chosenDoor).position - instantiatedRoom.transform.GetChild(1).GetChild(connectingDoor).position;
  //          while (Mathf.Abs(Vector3.Distance(lastRoom.transform.position, instantiatedRoom.transform.position)) < 2) {
  //            instantiatedRoom.transform.RotateAround(lastRoom.transform.GetChild(1).GetChild(chosenDoor).position, Vector3.up, 180);
  //          }
  //          goto loop;
  //        }
  //      }

  //    Destroy(instantiatedRoom.transform.GetChild(1).GetChild(connectingDoor).gameObject);
  //    generatedRooms.Add(lastRoom);
  //    foreach (GameObject door in lastRoom.transform.GetChild(1).childCount) {
  //      lastRoom.transform.GetChild(1).GetChild(door).gameObject.setActive(true);
  //    }

  //    lastRoom = instantiatedRoom;
  //  }
  //}
}
