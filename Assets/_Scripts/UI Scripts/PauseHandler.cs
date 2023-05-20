using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseHandler : MonoBehaviour 
{
	public List<GameObject> _menuScreens; // First string is default pause string

	public List<GameObject> _menuLayers;

	public enum PauseState {
		Unpaused,
		FirstPause,
		Paused,
		Dead
	}
	public PauseState _pauseState;

  public bool _ignoreTimescale;

	private void Awake() {
		_pauseState = PauseState.Unpaused;
	}

	private void Start() {
		_menuLayers = new List<GameObject>();
	}

	private void Update() {
		HandlePausing();
		CheckForDeath();
	}

	private void HandlePausing() {

		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton9)) {

			switch(_pauseState) {

				case PauseState.Unpaused:
					_pauseState = PauseState.FirstPause;

          if (!_ignoreTimescale) {
            Time.timeScale = 0.0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
          }

					_menuLayers.Add(_menuScreens[0]);
					_menuLayers[0].SetActive(true);

					GameObject disableEnemy = GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject;
					try {disableEnemy.GetComponent<SwitchableObject>()._raycastOrigin.gameObject.GetComponent<EnemyInfo>().SwitchedAway();
						disableEnemy.GetComponent<SwitchableObject>()._raycastOrigin.gameObject.GetComponent<EnemyInfo>().enabled = false;
					} catch {
						disableEnemy.GetComponent<SwitchableObject>()._raycastOrigin.parent.GetChild(1).GetComponent<CameraOverlay>().enabled = false;
					}
					if (disableEnemy.transform.GetChild(1).GetComponent<PlayerAim>()) disableEnemy.transform.GetChild(1).GetComponent<PlayerAim>().enabled = false;


					break;

				case PauseState.FirstPause:
					_pauseState = PauseState.Unpaused;
          if (!_ignoreTimescale) {
            Time.timeScale = 1.0f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
          }

					_menuScreens[0].transform.GetChild(1).GetComponent<CanvasGroup>().alpha = 1.0f;
					_menuScreens[0].transform.GetChild(1).GetComponent<CanvasGroup>().interactable = true;
					_menuScreens[0].transform.GetChild(1).GetComponent<CanvasGroup>().blocksRaycasts = true;
											
					_menuScreens[0].transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 0f;
					_menuScreens[0].transform.GetChild(2).GetComponent<CanvasGroup>().interactable = false;
					_menuScreens[0].transform.GetChild(2).GetComponent<CanvasGroup>().blocksRaycasts = false;
											
					_menuScreens[0].transform.GetChild(3).GetComponent<CanvasGroup>().alpha = 0f;
					_menuScreens[0].transform.GetChild(3).GetComponent<CanvasGroup>().interactable = false;
					_menuScreens[0].transform.GetChild(3).GetComponent<CanvasGroup>().blocksRaycasts = false;

					_menuLayers[0].SetActive(false);
					_menuLayers.Remove(_menuLayers[^1]);

					GameObject enableEnemy = GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject;
					try { enableEnemy.GetComponent<SwitchableObject>()._raycastOrigin.gameObject.GetComponent<EnemyInfo>().enabled = true;
					} catch { enableEnemy.GetComponent<SwitchableObject>()._raycastOrigin.parent.GetChild(1).GetComponent<CameraOverlay>().enabled = true;
					}
					if (enableEnemy.transform.GetChild(1).GetComponent<PlayerAim>()) enableEnemy.transform.GetChild(1).GetComponent<PlayerAim>().enabled = true;

					break;

				case PauseState.Paused:
					RemovePause();
					break;
			}
		}
	}

	private void CheckForDeath() {
		if (_pauseState == PauseState.Dead) {
			GameObject disableEnemy = GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject;
			try {
				disableEnemy.GetComponent<SwitchableObject>()._raycastOrigin.gameObject.GetComponent<EnemyInfo>().SwitchedAway();
				disableEnemy.GetComponent<SwitchableObject>()._raycastOrigin.gameObject.GetComponent<EnemyInfo>().enabled = false;
			} catch {
				disableEnemy.GetComponent<SwitchableObject>()._raycastOrigin.parent.GetChild(1).GetComponent<CameraOverlay>().enabled = false;
			}
			if (disableEnemy.transform.GetChild(1).GetComponent<PlayerAim>()) {
        disableEnemy.transform.GetChild(1).GetComponent<PlayerAim>().enabled = false;
      }
      Time.timeScale = 0.0f;
      Cursor.visible = true;
      Cursor.lockState = CursorLockMode.None;
		}
	}

	public void RemovePause() {

		_pauseState = PauseState.Unpaused;

		Time.timeScale = 1.0f;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		_menuScreens[0].transform.GetChild(1).GetComponent<CanvasGroup>().alpha = 1.0f;
		_menuScreens[0].transform.GetChild(1).GetComponent<CanvasGroup>().interactable = true;
		_menuScreens[0].transform.GetChild(1).GetComponent<CanvasGroup>().blocksRaycasts = true;

		_menuScreens[0].transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 0f;
		_menuScreens[0].transform.GetChild(2).GetComponent<CanvasGroup>().interactable = false;
		_menuScreens[0].transform.GetChild(2).GetComponent<CanvasGroup>().blocksRaycasts = false;

		_menuScreens[0].transform.GetChild(3).GetComponent<CanvasGroup>().alpha = 0f;
		_menuScreens[0].transform.GetChild(3).GetComponent<CanvasGroup>().interactable = false;
		_menuScreens[0].transform.GetChild(3).GetComponent<CanvasGroup>().blocksRaycasts = false;

		_menuScreens[0].SetActive(false);
		_menuLayers.Remove(_menuLayers[^1]);

		GameObject enableEnemy = GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject;
		try {
			enableEnemy.GetComponent<SwitchableObject>()._raycastOrigin.gameObject.GetComponent<EnemyInfo>().enabled = true;
		} catch {
			enableEnemy.GetComponent<SwitchableObject>()._raycastOrigin.parent.GetChild(1).GetComponent<CameraOverlay>().enabled = true;
		}
		if (enableEnemy.transform.GetChild(1).GetComponent<PlayerAim>()) enableEnemy.transform.GetChild(1).GetComponent<PlayerAim>().enabled = true;

		if (_menuLayers.Count > 1) _pauseState = PauseState.Paused;
		else if (_menuLayers.Count == 1) _pauseState = PauseState.FirstPause;
		else _pauseState = PauseState.Unpaused;
	}

	public void AddPause(string screenName) {
		_menuLayers[^1].SetActive(false);
		_menuLayers.Add(FindObjectWithName(screenName));
		_menuLayers[^1].SetActive(true);

		if (_pauseState == PauseState.Unpaused) _pauseState = PauseState.FirstPause;
		else if (_pauseState == PauseState.FirstPause) _pauseState = PauseState.Paused;
	}

	public void ChangeScene(string sceenName) {
		Time.timeScale = 1.0f;
		SceneManager.LoadScene(sceenName);
	}

	public void Unpause() {
		_pauseState = PauseState.Unpaused;
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
