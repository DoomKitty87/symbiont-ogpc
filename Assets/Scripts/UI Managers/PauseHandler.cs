using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHandler : MonoBehaviour 
{
	[SerializeField] private GameObject[] objectsToBeShown;
	[SerializeField] private GameObject[] objectsToBeHidden;

	private ButtonScript buttonScript;
	private bool isPaused = false;

	private void Start() {
		UnPause();
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (isPaused) UnPause(); else Pause();
		}
	}

	// Handles hiding and unhiding objects when paused
	public void Pause() {

		isPaused = true;

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		foreach (GameObject thing in objectsToBeShown) thing.SetActive(true);
		foreach (GameObject thing in objectsToBeHidden) thing.SetActive(false);
		Time.timeScale = 0.0f;
	}

	// Function called from ButtonScript to handle unpausing
	public void UnPause() {

		isPaused = false;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;		

		foreach (GameObject thing in objectsToBeHidden) thing.SetActive(true);
		foreach (GameObject thing in objectsToBeShown) thing.SetActive(false);
		Time.timeScale = 1.0f;
	}

	// Glorified switch statement
	private void HandleNestedMenus() {
		switch(buttonScript.currentActiveElement) {
			case null:
				buttonScript.ButtonHideScreen();
				break;
			default:
				buttonScript.ButtonChangeMenuScreen("Pause Menu");
				break;

		}
	}
}
