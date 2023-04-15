using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCameraPrefab : MonoBehaviour
{
    [SerializeField] private GameObject cameraPrefab;

	private DoorCameraFollow doorCameraFollow;

	private void Start() {
		GameObject camera = Instantiate(cameraPrefab, transform.position, Quaternion.identity);

		doorCameraFollow = camera.GetComponent<DoorCameraFollow>();

		doorCameraFollow.door = this.gameObject.transform;
		//doorCameraFollow.playerCamera = 
	}
}
