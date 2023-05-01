using System.Collections.Generic;
using UnityEngine;

public class RoomHandler : MonoBehaviour
{
	private RoomGenNew _roomGenNew;

	[HideInInspector] public List<GameObject> _arrayOfDoors;
	private int _numberOfDoors;

	[HideInInspector] public GameObject _previousDoor;
	[HideInInspector] public GameObject _instantiatedCamera;
	[HideInInspector] public GameObject _nextDoor;

	private int _numberOfEnemies;
	private bool _instantiatedNewRoom;

	private void Awake() {
		_roomGenNew = GameObject.FindWithTag("Handler").GetComponent<RoomGenNew>();

		_numberOfDoors = transform.GetChild(1).transform.childCount;

		// Fills _arrayOfDoors with all child doors
		for (int i = 0; i < _numberOfDoors; i++) {
			_arrayOfDoors.Add(transform.GetChild(1).GetChild(i).gameObject);
		}

		if (_numberOfDoors > 1) { // If there is more than one door in the room
			int randomIndex = Random.Range(0, _arrayOfDoors.Count);
			_previousDoor = _arrayOfDoors[randomIndex];
			_arrayOfDoors.Remove(_previousDoor);
		}
	}

	private void Update() {

		_numberOfEnemies = transform.GetChild(0).childCount;

		if (_numberOfEnemies == 1 && !_instantiatedNewRoom) {
			_instantiatedNewRoom = true;
			Pick_nextDoor();
			_roomGenNew.CreateNewRoom();
			_roomGenNew._currentRoom.GetComponent<RoomHandler>().CreateCameraPrefab();
		}
	}

	// Should be called when there is one enemy alives
	private void Pick_nextDoor() {
		// At this point the previous door should be removed from _arrayOfDoors

		_nextDoor = _arrayOfDoors[Random.Range(0, _arrayOfDoors.Count - 1)];
		InitiateSetUp(_nextDoor);
	}


	private void InitiateSetUp(GameObject _nextDoor) {
		Material doorMaterial = Resources.Load<Material>("Materials/DoorMaterial");
		_nextDoor.transform.GetChild(0).GetComponent<MeshRenderer>().material = doorMaterial;
		_nextDoor.transform.GetChild(0).tag = "DoorGraphic";
	}

	// Should be called by previous RoomHandler
	public void CreateCameraPrefab() {
		if (!_previousDoor) {
			Debug.LogError("GameObject doesn't have gameObject _previousDoor assigned.");
			return;
		}

		GameObject cameraPrefab = Resources.Load<GameObject>("Prefabs/DoorCameraPrefab");

		// Instantiates the camera for the previous door
		_instantiatedCamera = Instantiate(cameraPrefab, _previousDoor.transform, _previousDoor.transform);

	  Camera instantiatedCamera = _instantiatedCamera.GetComponent<Camera>();

		Material cameraMat = Resources.Load<Material>("Materials/DoorMaterial");

		instantiatedCamera.targetTexture?.Release();
		instantiatedCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
		cameraMat.mainTexture = instantiatedCamera.targetTexture;

		_instantiatedCamera.GetComponent<DoorCameraFollow>().otherDoor = GameObject.FindWithTag("Handler").
			GetComponent<RoomGenNew>()._previousRoom.GetComponent<RoomHandler>()._nextDoor.transform;
	}
}
