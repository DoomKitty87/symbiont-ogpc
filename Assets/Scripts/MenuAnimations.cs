using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimations : MonoBehaviour
{
	private MenuButtonIndexing menuButtonIndexing;
	private Animator animator;
	// [SerializeField] private AnimatorFunctions animatorFunctions;
	[SerializeField] int thisIndex;

	private void Start() {
		menuButtonIndexing = GetComponent<MenuButtonIndexing>();
		animator = GetComponent<Animator>();
	}

	private void Update() {
		if (menuButtonIndexing.index == thisIndex) {
			animator.SetBool("selected", true);
			if (Input.GetAxis("Submit") == 1) {
				animator.SetBool("Pressed", true);
			} else if (animator.GetBool("pressed")) {
				animator.SetBool("Pressed", false);
				//animatorFunctions.disableOnce = true;
			}
		} else {
			animator.SetBool("selected", false);
		}
	}
}
