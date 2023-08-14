using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PauseHandler : MonoBehaviour
{
	[FormerlySerializedAs("_pauseMenuHandler")]
	[Header("References")]
	[SerializeField] private PauseMenuShowHideHandler _pauseMenuShowHideHandler;
	
	[Header("Inputs")]
	[SerializeField] private List<KeyCode> _pauseKeycodes = new();
  public bool _disablePauseKeycodes = false;
  
  [Header("Pause State")]
  public PauseState _pauseState;
  public enum PauseState {
		Unpaused,
		Paused,
  }
  
  [Header("Other Settings")]
  public bool _ignoreTimescale;
  
  private void Start() {
	  _pauseState = PauseState.Unpaused;
		Unpause();
	  _disablePauseKeycodes = false;
  }

  private bool GetAnyKeyCodesDown(List<KeyCode> keyCodes) {
	  foreach (KeyCode currentKeyCode in keyCodes) {
		  if (Input.GetKeyDown(currentKeyCode)) return true;
	  }
	  return false;
  }
  private void Update() {
	  if (GetAnyKeyCodesDown(_pauseKeycodes) && !_disablePauseKeycodes) {
			TogglePause();
		}
  }
  
  // The continue button uses this because Unity's buttons don't configure the event system
  // to support calling functions with more than one boolean parameter, so Unpause() doesn't
  // work.
  public void TogglePause() {
	  switch(_pauseState) {
		  case PauseState.Unpaused: {
			  Pause(_ignoreTimescale, true);
			  break;
		  }
		  case PauseState.Paused: {
			  Unpause(_ignoreTimescale, true);
			  break;
		  }
	  }
  }
  
  public void Pause(bool ignoreTimescale = false, bool showMenu = true) {
	  _pauseState = PauseState.Paused;
	  if (!ignoreTimescale) {
		  Time.timeScale = 0.0f;
		  Cursor.visible = true;
		  Cursor.lockState = CursorLockMode.None;
	  }
	  if (showMenu) {
		  _pauseMenuShowHideHandler.ShowPauseMenu();
	  }
	  DisableOverlaysAndAim();
  }
  public void Unpause(bool ignoreTimescale = false, bool hideMenu = true) {
	  _pauseState = PauseState.Unpaused;
	  if (!ignoreTimescale) { 
		  Time.timeScale = 1.0f;
		  Cursor.visible = false;
		  Cursor.lockState = CursorLockMode.Locked;
	  }
	  if (hideMenu) {
		  _pauseMenuShowHideHandler.HidePauseMenu();
	  }
	  EnableOverlaysAndAim();
  }

  private void DisableOverlaysAndAim() {
	  GameObject playerEnemy = GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject;
	  try {
		  playerEnemy.GetComponent<SwitchableObject>()._raycastOrigin.gameObject.GetComponent<EnemyInfo>().SwitchedAway();
		  playerEnemy.GetComponent<SwitchableObject>()._raycastOrigin.gameObject.GetComponent<EnemyInfo>().enabled = false;
	  } 
	  catch {
		  playerEnemy.GetComponent<SwitchableObject>()._raycastOrigin.parent.GetChild(1).GetComponent<CameraOverlay>().enabled = false;
	  }
	  if (playerEnemy.transform.GetChild(1).GetComponent<PlayerAim>()) {
		  playerEnemy.transform.GetChild(1).GetComponent<PlayerAim>().enabled = false;
	  }
  }
  private void EnableOverlaysAndAim() {
	  GameObject enableEnemy = GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject;
	  try { 
		  enableEnemy.GetComponent<SwitchableObject>()._raycastOrigin.gameObject.GetComponent<EnemyInfo>().enabled = true;
	  } 
	  catch { 
		  enableEnemy.GetComponent<SwitchableObject>()._raycastOrigin.parent.GetChild(1).GetComponent<CameraOverlay>().enabled = true;
	  }
	  if (enableEnemy.transform.GetChild(1).GetComponent<PlayerAim>()) {
		  enableEnemy.transform.GetChild(1).GetComponent<PlayerAim>().enabled = true;
	  }
  }

  // public void ChangeScene(string sceneName) {
	// 	Time.timeScale = 1.0f;
	// 	SceneManager.LoadScene(sceneName);
	// }
	
	// public void RemovePause() {
	// 	print("Paused state reached");
	// 	_pauseState = PauseState.Unpaused;
	//
	// 	Time.timeScale = 1.0f;
	// 	Cursor.visible = false;
	// 	Cursor.lockState = CursorLockMode.Locked;
	//
	// 	_menuScreens[0].transform.GetChild(1).GetComponent<CanvasGroup>().alpha = 1.0f;
	// 	_menuScreens[0].transform.GetChild(1).GetComponent<CanvasGroup>().interactable = true;
	// 	_menuScreens[0].transform.GetChild(1).GetComponent<CanvasGroup>().blocksRaycasts = true;
	//
	// 	_menuScreens[0].transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 0f;
	// 	_menuScreens[0].transform.GetChild(2).GetComponent<CanvasGroup>().interactable = false;
	// 	_menuScreens[0].transform.GetChild(2).GetComponent<CanvasGroup>().blocksRaycasts = false;
	//
	// 	_menuScreens[0].transform.GetChild(3).GetComponent<CanvasGroup>().alpha = 0f;
	// 	_menuScreens[0].transform.GetChild(3).GetComponent<CanvasGroup>().interactable = false;
	// 	_menuScreens[0].transform.GetChild(3).GetComponent<CanvasGroup>().blocksRaycasts = false;
	//
	// 	_menuScreens[0].SetActive(false);
	// 	_menuLayers.Remove(_menuLayers[^1]);
	//
	// 	GameObject enableEnemy = GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject;
	// 	try {
	// 		enableEnemy.GetComponent<SwitchableObject>()._raycastOrigin.gameObject.GetComponent<EnemyInfo>().enabled = true;
	// 	} 
	// 	catch {
	// 		enableEnemy.GetComponent<SwitchableObject>()._raycastOrigin.parent.GetChild(1).GetComponent<CameraOverlay>().enabled = true;
	// 	}
	// 	if (enableEnemy.transform.GetChild(1).GetComponent<PlayerAim>()) enableEnemy.transform.GetChild(1).GetComponent<PlayerAim>().enabled = true;
	//
	// 	if (_menuLayers.Count > 1) _pauseState = PauseState.ContinueButton;
	// 	else if (_menuLayers.Count == 1) _pauseState = PauseState.Paused;
	// 	else _pauseState = PauseState.Unpaused;
	// }
	//
	// public void AddPause(string screenName) {
	// 	_menuLayers[^1].SetActive(false);
	// 	_menuLayers.Add(FindObjectWithName(screenName));
	// 	_menuLayers[^1].SetActive(true);
	//
	// 	if (_pauseState == PauseState.Unpaused) _pauseState = PauseState.Paused;
	// 	else if (_pauseState == PauseState.Paused) _pauseState = PauseState.ContinueButton;
	// }


	// // Returns GameObject in scene with name
	// // Used because GameObject.Find doesn't work with GameObjects that are inactive
	// private GameObject FindObjectWithName(string name) {
	// 	GameObject[] list = FindObjectsOfType<GameObject>(true);
	//
	// 	for (var i = 0; i < list.Length; i++) {
	// 		if (list[i].name == name) {
	// 			Debug.Log(list[i].name);
	// 			return list[i];
	// 		}
	// 	}
	// 	Debug.LogError("Unable to find GameObject " + name);
	// 	return null;
	// }
}
