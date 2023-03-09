using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimations : MonoBehaviour
{
	private Animator animator;

	private void Start() {
		animator = GetComponent<Animator>();
	}

	public void OnHover() {
		animator.SetBool("Hover", true);
	}

	public void OffHover() {
		animator.SetBool("Hover", false);
		animator.SetBool("Clicked", false);
	}

	public void OnClick() {
		animator.SetBool("Clicked", true);
	}
}
