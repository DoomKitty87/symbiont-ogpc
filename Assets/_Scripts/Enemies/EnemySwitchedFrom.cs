using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwitchedFrom : MonoBehaviour
{
	public void OnSwitch() {
		transform.localRotation = Quaternion.identity;
	}
}
