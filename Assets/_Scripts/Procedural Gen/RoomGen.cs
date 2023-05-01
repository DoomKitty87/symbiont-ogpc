// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class RoomGen : MonoBehaviour
// {

//   [SerializeField] private float roomNumber;
//   [SerializeField] private GameObject[] genRooms;
//   [SerializeField] private GameObject doorFiller;
//   [SerializeField] private float roomSize;
//   [SerializeField] private LayerMask roomLayer;

//   private List<GameObject> generatedRooms = new List<GameObject>();

//   private GameObject lastRoom;

//   private void Start() {
//     GenerateDungeon(3);
//   }

//   public void TriggerNewRoom() {
//    GenerateRoom();
//   }

//   private void GenerateRoom() {
//     GameObject roomChoice = genRooms[Random.Range(0, genRooms.Length)];
//     GameObject instantiatedRoom = Instantiate(roomChoice, new Vector3(0, 0, 0), Quaternion.identity, transform);

//     int chosenDoor = Random.Range(0, lastRoom.transform.GetChild(1).childCount);
//     int connectingDoor = Random.Range(0, instantiatedRoom.transform.GetChild(1).childCount);
      
//     //instantiatedRoom.transform.position = lastRoom.transform.GetChild(1).GetChild(chosenDoor).position - instantiatedRoom.transform.GetChild(1).GetChild(connectingDoor).position;
//     //while (Mathf.Abs((lastRoom.transform.position - instantiatedRoom.transform.position).magnitude) < roomSize) {
//     //  instantiatedRoom.transform.RotateAround(lastRoom.transform.GetChild(1).GetChild(chosenDoor).position, Vector3.up, 180);
//     //}

//     Collider[] cols = Physics.OverlapSphere(instantiatedRoom.transform.position, instantiatedRoom.GetComponent<Renderer>().bounds.extent.x, roomLayer);

//     while (cols.Length > 1) {
//       instantiatedRoom.transform.RotateAround(lastRoom.transform.GetChild(1).GetChild(chosenDoor).position, Vector3.up, 180);
//       cols = Physics.OverlapSphere(instantiatedRoom.transform.position, instantiatedRoom.GetComponent<Renderer>().bounds.extent.x, roomLayer);
//     }

//     loop:
//       foreach (GameObject room in generatedRooms) {
//         cols = Physics.OverlapSphere(room.transform.position, room.GetComponent<Renderer>().bounds.extent.x, roomLayer);
//         if (cols.Length > 1) {
//           chosenDoor = Random.Range(0, lastRoom.transform.GetChild(1).childCount);
//           connectingDoor = Random.Range(0, instantiatedRoom.transform.GetChild(1).childCount);
//           instantiatedRoom.transform.position = lastRoom.transform.GetChild(1).GetChild(chosenDoor).position - instantiatedRoom.transform.GetChild(1).GetChild(connectingDoor).position;
//           cols = Physics.OverlapSphere(instantiatedRoom.transform.position, instantiatedRoom.GetComponent<Renderer>().bounds.extent.x, roomLayer);

//           while (cols.Length > 1) {
//             instantiatedRoom.transform.RotateAround(lastRoom.transform.GetChild(1).GetChild(chosenDoor).position, Vector3.up, 180);
//             cols = Physics.OverlapSphere(instantiatedRoom.transform.position, instantiatedRoom.GetComponent<Renderer>().bounds.extent.x, roomLayer);
//           }
//           goto loop;
//         }
//       }

//     Destroy(instantiatedRoom.transform.GetChild(1).GetChild(connectingDoor).gameObject);
//     generatedRooms.Add(lastRoom);
//     for (int door = 0; door < lastRoom.transform.GetChild(1).childCount; ++door) {
//       lastRoom.transform.GetChild(1).GetChild(door).gameObject.SetActive(true);
//      //Will cause undesired behavior but I'll deal with that in a bit
//     }

//     Destroy(generatedRooms[0]);
//     generatedRooms.RemoveAt(0);

//     lastRoom = instantiatedRoom;
//   }

//   private void GenerateDungeon(int rooms) {

//     GameObject _roomChoice = genRooms[Random.Range(0, genRooms.Length)];
//     GameObject _instantiatedRoom = Instantiate(_roomChoice, new Vector3(0, 0, 0), Quaternion.identity, transform);
//     lastRoom = _instantiatedRoom;

//     for (int i = 0; i < rooms - 1; i++) {
//       GameObject roomChoice = genRooms[Random.Range(0, genRooms.Length)];
//       GameObject instantiatedRoom = Instantiate(roomChoice, new Vector3(0, 0, 0), Quaternion.identity, transform);

//       int chosenDoor = Random.Range(0, lastRoom.transform.GetChild(1).childCount);
//       int connectingDoor = Random.Range(0, instantiatedRoom.transform.GetChild(1).childCount);
      
//       instantiatedRoom.transform.position = lastRoom.transform.GetChild(1).GetChild(chosenDoor).position - instantiatedRoom.transform.GetChild(1).GetChild(connectingDoor).position;
//       //while (Mathf.Abs((lastRoom.transform.position - instantiatedRoom.transform.position).magnitude) < roomSize) {
//       //  instantiatedRoom.transform.RotateAround(lastRoom.transform.GetChild(1).GetChild(chosenDoor).position, Vector3.up, 180);
//       //}

//       Collider[] cols = Physics.OverlapSphere(instantiatedRoom.transform.position, instantiatedRoom.GetComponent<Renderer>().bounds.extent.x, roomLayer);

//       while (cols.Length > 1) {
//         instantiatedRoom.transform.RotateAround(lastRoom.transform.GetChild(1).GetChild(chosenDoor).position, Vector3.up, 180);
//         cols = Physics.OverlapSphere(instantiatedRoom.transform.position, instantiatedRoom.GetComponent<Renderer>().bounds.extent.x, roomLayer);
//       }

//       loop:
//         foreach (GameObject room in generatedRooms) {
//           cols = Physics.OverlapSphere(room.transform.position, room.GetComponent<Renderer>().bounds.extent.x, roomLayer);
//           if (cols.Length > 1) {
//             chosenDoor = Random.Range(0, lastRoom.transform.GetChild(1).childCount);
//             connectingDoor = Random.Range(0, instantiatedRoom.transform.GetChild(1).childCount);
//             instantiatedRoom.transform.position = lastRoom.transform.GetChild(1).GetChild(chosenDoor).position - instantiatedRoom.transform.GetChild(1).GetChild(connectingDoor).position;
//             cols = Physics.OverlapSphere(instantiatedRoom.transform.position, instantiatedRoom.GetComponent<Renderer>().bounds.extent.x, roomLayer);

//             while (cols.Length > 1) {
//               instantiatedRoom.transform.RotateAround(lastRoom.transform.GetChild(1).GetChild(chosenDoor).position, Vector3.up, 180);
//               cols = Physics.OverlapSphere(instantiatedRoom.transform.position, instantiatedRoom.GetComponent<Renderer>().bounds.extent.x, roomLayer);
//             }
//             goto loop;
//           }
//         }

//       Destroy(instantiatedRoom.transform.GetChild(1).GetChild(connectingDoor).gameObject);
//       generatedRooms.Add(lastRoom);
//       for (int door = 0; door < lastRoom.transform.GetChild(1).childCount; ++door) {
//         if (door == chosenDoor) continue;
//         lastRoom.transform.GetChild(1).GetChild(door).gameObject.SetActive(true);
//       }

//       lastRoom = instantiatedRoom;
//     }
//   }
// }
