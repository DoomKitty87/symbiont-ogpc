using System.Collections.Generic;
using UnityEngine;

public class PauseHandler : MonoBehaviour 
{
	[SerializeField] private GameObject[] _objectsToBeHidden;

	[SerializeField] private List<GameObject> _currentActiveElements;

	[SerializeField] private IDictionary<string, GameObject> _menuScreens;

	[SerializeField] private string _defaultCurrentActiveElement;
	[SerializeField] private string _stringToShowOnPause;

	private void Awake() {
		_menuScreens = new Dictionary<string, GameObject>() {
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

		 _stringToShowOnPause ??= "Pause Menu";
	}

	private void Start() {
		_currentActiveElements.Add(_menuScreens[_defaultCurrentActiveElement]);
		// UnPause();
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			
			// Handles what pressing escape does depending on the active scene
			if (_currentActiveElements[0] == null) {
				if (_currentActiveElements.Count == 1) InitPause(); else if (_currentActiveElements.Count == 2) Unpause(); else RemovePause();
			} else {
				if (_currentActiveElements.Count > 1) RemovePause();
			}
		}
	}

	// Handles when pause key is first pressed
	public void InitPause() {
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		foreach (GameObject thing in _objectsToBeHidden) thing.SetActive(false);

		_currentActiveElements.Add(_menuScreens[_stringToShowOnPause]);
	
		_currentActiveElements[^1].SetActive(true);

		Time.timeScale = 0.0f;
	}

	// Adds an object to pause array
	public void AddPause(string s) {
		StartCoroutine(_currentActiveElements[^1].GetComponent<MenuScreenAnimations>().CloseScreen(_currentActiveElements[^1]));
		_currentActiveElements.Add(_menuScreens[s]);
	}

	// Removes an object from pause array
	public void RemovePause() {
		StartCoroutine(_currentActiveElements[^1].GetComponent<MenuScreenAnimations>().CloseScreen(_currentActiveElements[^1]));
		_currentActiveElements.RemoveAt(_currentActiveElements.Count - 1);
	}

	// Handles when pause fully unpauses menu
	public void Unpause() {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		foreach (GameObject thing in _objectsToBeHidden) thing.SetActive(true);

		_currentActiveElements[^1].SetActive(false);
		_currentActiveElements.RemoveAt(_currentActiveElements.Count - 1);

		Time.timeScale = 1.0f;
	}

	// Script called by MenuScreenAnimations after coroutine
	public void ChangeScreen() {
		if (_currentActiveElements.Count != 1 && _currentActiveElements[0] == null) _currentActiveElements[^1].SetActive(true); else _currentActiveElements[^1].SetActive(true);
	}

	// Returns GameObject in scene with name
	// Used because GameObject.Find doesn't work with GameObjects that are inactive
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
