using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMountedCamera : MonoBehaviour
{
  private Transform playerTransform;
	private Transform localPlayerTransform;

	private void Update() {
		playerTransform = GameObject.FindWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject.transform;
		transform.rotation = Quaternion.LookRotation((playerTransform.position - transform.position).normalized);
    transform.rotation *= Quaternion.Euler(25, 180, 0);
	}
}
