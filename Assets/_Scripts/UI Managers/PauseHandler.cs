using System.Collections.Generic;
using UnityEngine;

public class PauseHandler : MonoBehaviour 
{
	[SerializeField] private GameObject[] objectsToBeHidden;

	public List<GameObject> currentActiveElements;

	public IDictionary<string, GameObject> menuScreens;

	public string defaultCurrentActiveElement;
	public string stringToShowOnPause;

	private ButtonScript buttonScript;

	private void Awake() {
		menuScreens = new Dictionary<string, GameObject>() {
			{ "Main Menu", FindObjectOfName("Main Menu") },
			{ "Pause Menu", FindObjectOfName("Pause Menu") },
			{ "Settings Menu", FindObjectOfName("Settings Menu") },
			{ "Check Menu", FindObjectOfName("Check Menu") },
			{ "Controls Menu", FindObjectOfName("Controls Menu") },
			{ "Audio Menu", FindObjectOfName("Audio Menu") },
			{ "Video Menu", FindObjectOfName("Video Menu") },
			{ "Gun Menu", FindObjectOfName("Gun Menu") },
			{ "Armor Menu", FindObjectOfName("Armor Menu") },
			{ "", null}
		};

		 if (stringToShowOnPause == null) stringToShowOnPause = "Pause Menu";
	}

	private void Start() {
		currentActiveElements.Add(menuScreens[defaultCurrentActiveElement]);
		// UnPause();
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (currentActiveElements[0] == null) {
				if (currentActiveElements.Count == 1) InitPause(); else if (currentActiveElements.Count == 2) Unpause(); else RemovePause();
			} else {
				if (currentActiveElements.Count > 1) RemovePause();
			}
		}
	}

	// Handles when pause key is first pressed
	public void InitPause() {

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		foreach (GameObject thing in objectsToBeHidden) thing.SetActive(false);

		currentActiveElements.Add(menuScreens[stringToShowOnPause]);
	
		currentActiveElements[currentActiveElements.Count - 1].SetActive(true);

		Time.timeScale = 0.0f;
	}

	// Adds an object to pause array
	public void AddPause(string s) {
		StartCoroutine(currentActiveElements[currentActiveElements.Count - 1].GetComponent<MenuScreenAnimations>().CloseScreen(currentActiveElements[currentActiveElements.Count - 1]));
		currentActiveElements.Add(menuScreens[s]);
	}

	// Removes an object from pause array
	
	public void RemovePause() {
		StartCoroutine(currentActiveElements[currentActiveElements.Count - 1].GetComponent<MenuScreenAnimations>().CloseScreen(currentActiveElements[currentActiveElements.Count - 1]));
		currentActiveElements.RemoveAt(currentActiveElements.Count - 1);
	}

	// Handles when pause fully unpauses menu
	private void Unpause() {

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;		

		RemovePause();

		foreach (GameObject thing in objectsToBeHidden) thing.SetActive(true);
		Time.timeScale = 1.0f;
	}

	public void ChangeScreen() {
		if (currentActiveElements.Count != 1) currentActiveElements[currentActiveElements.Count - 1].SetActive(true);
	}

	private GameObject FindObjectOfName(string name) {
		GameObject[] list = FindObjectsOfType<GameObject>(true);

		for (var i = 0; i < list.Length; i++) {
			if (list[i].name == name) {
				return list[i];
			}
		}
		// Debug.LogError("Unable to find GameObject " + name);
		return null;
	}
}
