using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuButtonIndexing : MonoBehaviour
{

  public int index;
	[SerializeField] int maxIndex;
	bool keyDown;

	private void Update() {
		if (Input.GetAxisRaw("Vertical") != 0) {
			if (!keyDown) {
				if (Input.GetAxisRaw ("Vertical") == 1) {
					if (index < maxIndex) maxIndex++;
					else index = 0;
				} else {
					if (index > maxIndex) maxIndex--;
					else index = maxIndex;
				}
				keyDown = true;
			}
		} else {
			keyDown = false;
		}
	}
}
