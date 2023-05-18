using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleUIElement : MonoBehaviour
{
	private bool toggleState;

	[SerializeField] private GameObject object1;
	[SerializeField] private GameObject object2;

	[SerializeField] private float duration;

	private IEnumerator ToggleCoroutine() {
		float time = 0;
		while (time < duration / 2) { // Half the duration then switch icon
			time += Time.unscaledDeltaTime;
			yield return null;
		}
		object1.SetActive(toggleState);
		object2.SetActive(!toggleState);
		toggleState = !toggleState;
	}

  public void Toggle() {
		StartCoroutine(ToggleCoroutine());
	}
}
