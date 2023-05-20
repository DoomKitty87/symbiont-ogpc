using System.Collections.Generic;
using UnityEngine;

public class RoomGenNew : MonoBehaviour
{

	// Rooms should have child categories of:
	// Enemies
    // Doors
    // Components
    // Cameras

    [Header("Tutorial")]
    [SerializeField] private bool _isTutorial;
    [SerializeField] private List<GameObject> tutorialRooms;

  private GameObject[] _randomRooms;
  private GameObject[] _startingRooms;
  private GameObject[] _endingRooms;

  public List<string> _roomBag;
  private List<string> _roomBagEmpty;

  [SerializeField] private int _numberOfRoomsPerFloor;

	private Vector3 _nextCoordinates = new(-100, 0, -100);
  private bool _parity; // Used for things that require odd or even (Basically don't worry about it)
  private int _roomsGenerated = 0;

  private GameObject _roomEnemyIsInhabiting;
  private GameObject _tempRoomEnemyIsInhabiting;

  [HideInInspector] public GameObject _currentRoom;
  [HideInInspector] public GameObject _previousRoom;
  [HideInInspector] public GameObject _startingRoom;

	private void Awake() {
    _roomBag = new List<string>();
    _roomBagEmpty = new List<string>();

    _randomRooms = GameObject.FindGameObjectWithTag("Persistent").GetComponent<FloorManager>().GetCurrentFloorRooms();
    _startingRooms = GameObject.FindGameObjectWithTag("Persistent").GetComponent<FloorManager>().GetCurrentFloorStartRooms();
    _endingRooms = GameObject.FindGameObjectWithTag("Persistent").GetComponent<FloorManager>().GetCurrentFloorEndRooms();

    if (_isTutorial) _startingRoom = Instantiate(GetNextTutorialRoom(), Vector3.zero, Quaternion.identity, transform);
    else _startingRoom = Instantiate(_startingRooms[Random.Range(0, _startingRooms.Length)], Vector3.zero, Quaternion.identity, transform);
	  // Assigns the first room as the current object inhabited
	  GameObject.FindWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting = 
      _startingRoom.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<SwitchableObject>();
    _roomsGenerated++;
    _currentRoom = _startingRoom;
	}

	private void Update() {
		_roomEnemyIsInhabiting = GetRoom(GameObject.FindWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject, "Room");
    if (_roomEnemyIsInhabiting != _tempRoomEnemyIsInhabiting) {
			_tempRoomEnemyIsInhabiting = _roomEnemyIsInhabiting;
      _currentRoom.GetComponent<RoomHandler>().ClosePreviousDoor();
		}
	}

	public void CreateNewRoom() {
        if (_isTutorial) {
            TutorialScript();
            return;
        }

        if (_numberOfRoomsPerFloor == -1) {
          // If the number of rooms per floor is -1, then the ending room has been generated and no more rooms should be generated
          Debug.Log("Reached maximum amount of rooms per level. Update _numberOfRoomsPerLevel value to increase room number.");
          return;
        }

        // If the room bag is empty, refill it and empty the empty bag
        if (_roomBag.Count == 0) {
          _roomBagEmpty.Clear();
          foreach (GameObject room in _randomRooms) {
            _roomBag.Add(room.name);
          }
        }

        // Creates new room
        if (_previousRoom != null) Destroy(_previousRoom);
        _previousRoom = _currentRoom;
        if (_roomsGenerated == _numberOfRoomsPerFloor - 1) {
          _currentRoom = Instantiate(_endingRooms[Random.Range(0, _endingRooms.Length)], _nextCoordinates, Quaternion.identity, transform);
          _numberOfRoomsPerFloor = -1; // Effectively ending room generation
          return;
        }

        // Deals with creating new rooms
        GameObject roomToAdd = Instantiate(GetRoomWithName(_roomBag[Random.Range(0, _roomBag.Count)], _randomRooms), _nextCoordinates, Quaternion.identity, transform);

        _currentRoom = roomToAdd;
        _roomBagEmpty.Add(roomToAdd.name.Remove(roomToAdd.name.Length - 7));
        _roomBag.Remove(roomToAdd.name.Remove(roomToAdd.name.Length - 7));
    
        if (_parity) {
            _parity ^= true;
          _nextCoordinates.x *= -1;
        } else {
            _parity ^= true;
          _nextCoordinates.z *= -1;
        }
        _roomsGenerated++;
    }

    private void TutorialScript() {
        if (_previousRoom != null) Destroy(_previousRoom);
        _previousRoom = _currentRoom;

        GameObject roomToAdd = Instantiate(GetNextTutorialRoom(), _nextCoordinates, Quaternion.identity, transform);

        if (_parity) {
            _parity ^= true;
            _nextCoordinates.x *= -1;
        } else {
            _parity ^= true;
            _nextCoordinates.z *= -1;
        }
    }

    private GameObject GetRoom(GameObject reference, string tagName) {
    while (reference.transform.parent != null) {
      if (reference.transform.parent.CompareTag(tagName)) return reference.transform.parent.gameObject;
      else reference = reference.transform.parent.gameObject;
    }
    Debug.LogError("No parent with tag found on reference.");
    return null;
  }

    private GameObject GetNextTutorialRoom() {
        GameObject nextRoom = tutorialRooms[0];
        tutorialRooms.Remove(nextRoom);
        return nextRoom;
	}

  private GameObject GetRoomWithName(string s, GameObject[] roomArray) {
    foreach(GameObject gameObject in roomArray) {
      if (gameObject.name == s) {
        return gameObject;
      }
    }
    Debug.LogError("No gameObject with name " + s + " found in " + roomArray + " array.");
    return null;
  }
}
