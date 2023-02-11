using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHandler : MonoBehaviour 
{

	[SerializeField] private GameObject[] objectsToBeShown;
	[SerializeField] private GameObject[] objectsToBeHidden;

	private ButtonScript buttonScript;
  private bool isPaused;

	private void Start() {
		buttonScript = GetComponent<ButtonScript>();
		UnPause();
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (!isPaused) Pause(); else HandleNestedMenus();
		}
	}

	public void Pause() {
		Cursor.lockState = CursorLockMode.None;
		isPaused = true;
		foreach (GameObject thing in objectsToBeShown) thing.SetActive(true);
		foreach (GameObject thing in objectsToBeHidden) thing.SetActive(false);
		Time.timeScale = 0.0f;
	}

	public void UnPause() {
		Cursor.lockState = CursorLockMode.Locked;
		isPaused = false;
		
		buttonScript.ResetScreen();

		foreach (GameObject thing in objectsToBeHidden) thing.SetActive(true);
		Time.timeScale = 1.0f;
	}

	private void HandleNestedMenus() {
		if (buttonScript.currentActiveElement == buttonScript.pauseScreen) {
			buttonScript.game_RESUME();
		} else if (buttonScript.currentActiveElement == buttonScript.settingsScreen) {
			buttonScript.settings_BACK();
		} else {
			buttonScript.default_BACK();
		}
	}
}
