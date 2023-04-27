using UnityEngine;

public class RoomGenNew : MonoBehaviour
{

	// Rooms should have child categories of:
	// Room Components
	// Door Object
	// Door fillers (for doors not in use)

  [SerializeField] private GameObject startingRoom;
  [SerializeField] private GameObject[] randomRooms;

  [SerializeField] private int _numberOfRoomsPerFloor;

	private Vector3 _nextCoordinates = new(-100, 0, -100);
  private bool _parody; // Used for things that require odd or even (Basically don't worry about it)

  public GameObject _currentRoom;
  public GameObject _previousRoom;

	private void Awake() {
	  // Assigns the first room as the current object inhabited
	  GameObject.FindWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting = 
      startingRoom.transform.GetChild(0).transform.GetChild(0).GetComponent<SwitchableObject>();

    _currentRoom = startingRoom;
	}

  public void CreateNewRoom() {
    // Creates new room
    if (_previousRoom != null) Destroy(_previousRoom);
    _previousRoom = _currentRoom;
    _currentRoom = Instantiate(randomRooms[0], _nextCoordinates, Quaternion.identity, transform);
    
    if (_parody) {
      _parody ^= true;
      _nextCoordinates.x *= -1;
    } else {
      _parody ^= true;
      _nextCoordinates.z *= -1;
    }
  }
}
