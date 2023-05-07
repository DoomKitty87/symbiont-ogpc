using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMountedCamera : MonoBehaviour
{
  private Transform playerTransform;
	private Transform localPlayerTransform;

	private void Update() {
		playerTransform = GameObject.FindWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject.transform;
		transform.LookAt(playerTransform, Vector3.back);
    transform.rotation = Quaternion.Euler(transform.rotation.x, -transform.rotation.y, transform.rotation.z);
	}
}
