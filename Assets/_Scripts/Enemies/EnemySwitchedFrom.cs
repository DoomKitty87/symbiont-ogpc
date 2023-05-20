using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwitchedFrom : MonoBehaviour
{
	public void OnSwitch() {
    transform.parent.localRotation = Quaternion.identity;
		transform.localRotation = Quaternion.identity;
    transform.GetChild(1).localRotation = Quaternion.identity;
	}
}
