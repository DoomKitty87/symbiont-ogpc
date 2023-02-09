using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHandler : MonoBehaviour {

	private bool isPaused;

	[SerializeField] private GameObject[] objectsToBeShown;
	[SerializeField] private GameObject[] objectsToBeHidden;

	private void Start() {
		UnPause();
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (!isPaused) Pause(); else UnPause();
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
		foreach (GameObject thing in objectsToBeShown) thing.SetActive(false);
		foreach (GameObject thing in objectsToBeHidden) thing.SetActive(true);
		Time.timeScale = 1.0f;
	}

}
