using System.Collections.Generic;
using UnityEngine;

public class PauseHandler : MonoBehaviour 
{
	[SerializeField] private GameObject[] objectsToBeHidden;

	public List<string> currentActiveElements;

	public string defaultCurrentActiveElement;

	public ButtonScript buttonScript;
	private bool isPaused = false;

	private void Awake() {
		buttonScript = GetComponent<ButtonScript>();
	}

	private void Start() {
		currentActiveElements.Add(defaultCurrentActiveElement);
		// UnPause();
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (currentActiveElements.Count == 1) InitPause(); else if (currentActiveElements.Count == 2) Unpause(); else RemovePause();
		}
	}

	// Handles when pause key is first pressed
	public void InitPause() {

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		foreach (GameObject thing in objectsToBeHidden) thing.SetActive(false);

		buttonScript.ButtonChangeMenuScreen("Pause Menu");

		currentActiveElements.Add("Pause Menu");

		Time.timeScale = 0.0f;
	}

	// Adds an object to pause array
	public void AddPause() {

	}

	// Removes an object from pause array
	public void RemovePause() {

	}

	// Hanldes when pause fully unpauses menu
	public void Unpause() {

		isPaused = false;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;		

		// currentActiveElement.SetActive(false);

		foreach (GameObject thing in objectsToBeHidden) thing.SetActive(true);
		Time.timeScale = 1.0f;
	}

	// Glorified switch statement
	private void HandleNestedMenus() {
		/*switch(currentActiveElement.name) {
			case "Pause Menu":
				UnPause();
				break;
			case "Settings Menu":
				buttonScript.ButtonChangeMenuScreen("Pause Menu");
				break;
			case null:
				Debug.LogError("currentActiveElement null error");
				break;
			default:
				buttonScript.ButtonChangeMenuScreen("Settings Menu");
				break;

		}
		*/
	}
}
