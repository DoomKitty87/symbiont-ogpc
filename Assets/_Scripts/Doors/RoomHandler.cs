using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RoomHandler : MonoBehaviour
{

	[HideInInspector] public bool _playerIsInRoom = false;
	private bool _hasEnteredRoom = false;

	[HideInInspector] public int numberOfDoors;

	[HideInInspector] public List<GameObject> doors;
	private GameObject entryDoor;
	[HideInInspector] public GameObject nextDoor;

	[HideInInspector] public Camera instantiatedCamera;

	private void Awake() {
		numberOfDoors = transform.GetChild(1).transform.childCount; // Requires doors to be the second child of the room gameObject
		FillDoorsList();
	}

	private void Update() {
		if (transform.GetChild(0).childCount <= 1) {
			// OpenNextDoor();
		}

		if (_playerIsInRoom && !_hasEnteredRoom) {
			_hasEnteredRoom = true;
		}
	}

	public void CloseDoor(GameObject door) {
		// TODO: Close doors
	}

	private void FillDoorsList() {
		// Get all doors in the gameobject
		for (int i = 0; i < numberOfDoors; i++) {
			doors.Add(transform.GetChild(1).transform.GetChild(i).gameObject);
		}

		// Chooses a random door to be the previous door
		if (numberOfDoors == 1) {
			nextDoor = doors[0];
		} else {

			int randomIndex = Random.Range(0, doors.Count);
			entryDoor = doors[randomIndex];
			doors.Remove(entryDoor);

			InitiateDoorStartup();
		}
	}

	private void InitiateDoorStartup() {

		// Instantiates cameras for each door
		GameObject prefab = Resources.Load<GameObject>("Prefabs/DoorCameraPrefab");

		// Instantiates the camera for the current active door
		GameObject instantiatedObject = Instantiate(prefab, entryDoor.transform.position, entryDoor.transform.rotation, entryDoor.transform);

		instantiatedCamera = instantiatedObject.GetComponent<Camera>();
		StartCoroutine(LateStart());

		Material cameraMat = Resources.Load<Material>("Materials/DoorMaterial");

		instantiatedCamera.targetTexture?.Release();
		instantiatedCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
		cameraMat.mainTexture = instantiatedCamera.targetTexture;

		entryDoor.transform.GetChild(0).GetComponent<MeshRenderer>().material = cameraMat;
		Debug.Log("AddedTexture");
	}

	IEnumerator LateStart() {
		yield return null;
		instantiatedCamera.GetComponent<DoorCameraFollow>().otherDoor = GameObject.FindWithTag("Handler").GetComponent<RoomGenNew>()
	._previousRoom.GetComponent<RoomHandler>().nextDoor.transform;
	}
}
