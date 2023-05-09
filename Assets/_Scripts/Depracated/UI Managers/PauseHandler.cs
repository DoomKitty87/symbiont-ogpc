using System.Collections.Generic;
using UnityEngine;

public class PauseHandler : MonoBehaviour 
{
	[SerializeField] private GameObject[] _objectsToBeHiddenOnPause;

	public List<GameObject> _menuScreens; // First string is default pause string

	public List<GameObject> _menuLayers;

	private enum PauseState {
		Unpaused,
		FirstPause,
		Paused
	}
	private PauseState _pauseState;

	private void Awake() {
		_pauseState = PauseState.Unpaused;
	}

	private void Start() {
		_menuLayers = new List<GameObject>();
	}

	private void Update() {
		HandlePausing();
	}

	private void HandlePausing() {

		if (Input.GetKeyDown(KeyCode.Escape)) {

			switch(_pauseState) {

				case PauseState.Unpaused:
					_pauseState = PauseState.FirstPause;

					Time.timeScale = 0.0f;
					Cursor.visible = true;
					Cursor.lockState = CursorLockMode.None;

					_menuLayers.Add(_menuScreens[0]);
					_menuLayers[0].SetActive(true);

					foreach(GameObject thing in _objectsToBeHiddenOnPause) thing.SetActive(false);

					break;

				case PauseState.FirstPause:
					_pauseState = PauseState.Unpaused;

					Time.timeScale = 1.0f;
					Cursor.visible = false;
					Cursor.lockState = CursorLockMode.Locked;

					_menuLayers[0].SetActive(false);
					_menuLayers.Remove(_menuScreens[0]);

					foreach (GameObject thing in _objectsToBeHiddenOnPause) thing.SetActive(true);

					break;

				case PauseState.Paused:
					RemovePause();
					break;
			}
		}
	}

	public void RemovePause() {

		_menuLayers[^1].SetActive(false);
		_menuLayers.Remove(_menuLayers[^1]);

		if (_menuLayers.Count > 0) _pauseState = PauseState.FirstPause;
		else {
			_pauseState = PauseState.Unpaused;

			Time.timeScale = 1.0f;
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
	}

	public void AddPause(string screenName) {
		_menuLayers[^1].SetActive(false);
		_menuLayers.Add(FindObjectWithName(screenName));
		_menuLayers[^1].SetActive(true);
	}

	// Returns GameObject in scene with name
	// Used because GameObject.Find doesn't work with GameObjects that are inactive
	private GameObject FindObjectWithName(string name) {
		GameObject[] list = FindObjectsOfType<GameObject>(true);

		for (var i = 0; i < list.Length; i++) {
			if (list[i].name == name) {
				Debug.Log(list[i].name);
				return list[i];
			}
		}
		Debug.LogError("Unable to find GameObject " + name);
		return null;
	}
}
