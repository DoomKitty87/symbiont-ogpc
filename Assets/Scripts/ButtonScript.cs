using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
  
  public GameObject pauseScreen;
	public GameObject settingsScreen;

  [SerializeField] private GameObject checkScreen;
	[SerializeField] private GameObject keyboardScreen;
	[SerializeField] private GameObject audioScreen;
  [SerializeField] private GameObject videoScreen;
  [SerializeField] private GameObject gunList;
  [SerializeField] private GameObject armorList;

	private PauseHandler pauseHandler;

  [HideInInspector] public GameObject currentActiveElement;

	private void Start() {
		pauseHandler = GetComponent<PauseHandler>();
		currentActiveElement = pauseScreen;
	}

  // --------------------------------
  // Game Button Scripts
  // --------------------------------
	public void game_RESUME() {
    pauseHandler.UnPause();
  }

  public void game_RESTART(string currentScene) {
    pauseHandler.UnPause();
    ChangeScene(currentScene);
  }

  public void game_LOAD() {
    // TODO
  }

  public void game_SETTINGS() {
    ChangeActiveSettingsElement(settingsScreen);
  }

  public void game_QUIT() {
	  ChangeScene("MainMenu");
  }

	// --------------------------------
	// Main Menu Button Scripts
	// --------------------------------
	public void menu_PLAY() {
    ChangeActiveSettingsElement(checkScreen);
  }  

  public void menu_SETTINGS() {
    ChangeActiveSettingsElement(settingsScreen);
  }

  public void menu_GUNLIST() {
    if (gunList.activeSelf) {
      gunList.SetActive(false);
    } else {
      gunList.SetActive(true);
    }
  }

  public void menu_ARMORLIST() {
    if (armorList.activeSelf) {
      armorList.SetActive(false);
    } else {
      armorList.SetActive(true);
    }
  }

  public void menu_QUIT() {
    Debug.Log("Quit Game");
    Application.Quit();
  }

	// --------------------------------
	// Settings Button Scripts
	// --------------------------------

  public void settings_CONTROLS() {
		ChangeActiveSettingsElement(keyboardScreen);
	}

  public void settings_AUDIO() {
		ChangeActiveSettingsElement(audioScreen);
	}

  public void settings_VIDEO() {
		ChangeActiveSettingsElement(videoScreen);
	}

  public void settings_BACK() {
    ChangeActiveSettingsElement(pauseScreen);
  }

	// --------------------------------

  public void default_BACK() {
    ChangeActiveSettingsElement(settingsScreen);
  }

	private void ChangeScene(string targetScene) {
    SceneManager.LoadScene(targetScene);
  }

  public void ResetScreen() {
    currentActiveElement.SetActive(false);
    currentActiveElement = pauseScreen;
  }

  private void ChangeActiveSettingsElement(GameObject targetElement) {
    if (gunList != null && gunList.activeSelf) gunList.SetActive(false);
    if (armorList != null && armorList.activeSelf) armorList.SetActive(false);
    currentActiveElement.SetActive(false);
    currentActiveElement = targetElement;
    currentActiveElement.SetActive(true);
  }
}
