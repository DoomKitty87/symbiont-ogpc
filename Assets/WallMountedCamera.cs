using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMountedCamera : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

	private void Update() {
		if (playerTransform) {
			transform.LookAt(playerTransform);
		} else {
			playerTransform = GameObject.FindWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject.transform;
		}
	}
}
