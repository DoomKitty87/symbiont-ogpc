using UnityEngine;

public class RoomGenNew : MonoBehaviour
{

	// Rooms should have child categories of:
	// Room Components
	// Door Object
	// Door fillers (for doors not in use)

  [SerializeField] private GameObject startingRoom;
  [SerializeField] private GameObject[] randomRooms;
  [SerializeField] private GameObject[] endingRooms;

  [SerializeField] private int _numberOfRoomsPerFloor;

	private Vector3 _nextCoordinates = new(-100, 0, -100);
  private bool _parody; // Used for things that require odd or even (Basically don't worry about it)
  private int roomsGenerated = 0;

  [HideInInspector] public GameObject _currentRoom;
  [HideInInspector] public GameObject _previousRoom;

	private void Awake() {
	  // Assigns the first room as the current object inhabited
	  GameObject.FindWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting = 
      startingRoom.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<SwitchableObject>();

    _currentRoom = startingRoom;
	}

  public void CreateNewRoom() {
    // Creates new room
    if (_previousRoom != null) Destroy(_previousRoom);
    _previousRoom = _currentRoom;
    if (roomsGenerated == _numberOfRoomsPerFloor) {
      _currentRoom = Instantiate(endingRooms[Random.Range(0, endingRooms.Length)], _nextCoordinates, Quaternion.identity, transform);
      return;
    }
    _currentRoom = Instantiate(randomRooms[Random.Range(0, randomRooms.Length)], _nextCoordinates, Quaternion.identity, transform);
    
    if (_parody) {
      _parody ^= true;
      _nextCoordinates.x *= -1;
    } else {
      _parody ^= true;
      _nextCoordinates.z *= -1;
    }
    roomsGenerated++;
  }
}
