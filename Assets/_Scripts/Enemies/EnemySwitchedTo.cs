using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwitchedTo : MonoBehaviour
{
	public void OnSwitch() {
		// Finds parent object with tag 'Room'
		Transform parent = gameObject.transform;
		while (true) {
			if (parent.CompareTag("Room")) break;
			else parent = parent.parent;

			if (parent == null) {
				Debug.LogError("Could not find parent object with tag 'Room' above" + gameObject.name);
				break;
			}
		}
		parent.GetComponent<RoomHandler>()._playerIsInRoom = true;
	}
}
