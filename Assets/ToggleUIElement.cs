using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleUIElement : MonoBehaviour
{
	private bool toggleState;

	[SerializeField] private GameObject object1;
	[SerializeField] private GameObject object2;

    public void Toggle() {
		object1.SetActive(toggleState);
		object2.SetActive(!toggleState);
		toggleState = !toggleState;
	}
}
