using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RoomHandler : MonoBehaviour
{

	[HideInInspector] public bool _playerIsInRoom = false;

	[HideInInspector] public int numberOfDoors;

	[HideInInspector] public List<GameObject> doors;
	public GameObject entryDoor;
	[HideInInspector] public GameObject nextDoor;

	[HideInInspector] public Camera instantiatedCamera;

	private void Awake() {
		numberOfDoors = transform.GetChild(1).transform.childCount; // Requires doors to be the second child of the room gameObject
		FillDoorsList();
	}

	/// <summary>
	/// First step in finding the next door to open
	/// Should be called when one player is alive
	/// </summary>
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

	/// <summary>
	/// Instantiates the camera and sets the material of the next door
	/// </summary>
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

		nextDoor.transform.GetChild(0).GetComponent<MeshRenderer>().material = cameraMat;
	}

	/// <summary>
	/// Used for first frame to select next door
	/// </summary>
	IEnumerator LateStart() {
		yield return null;
		instantiatedCamera.GetComponent<DoorCameraFollow>().otherDoor = GameObject.FindWithTag("Handler").GetComponent<RoomGenNew>()
	._previousRoom.GetComponent<RoomHandler>().nextDoor.transform;
	}
}
