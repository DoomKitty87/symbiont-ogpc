using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
  public GameObject pauseScreen;
	public GameObject settingsScreen;
	public GameObject mouseScreen;
	public GameObject keyboardScreen;
	public GameObject audioScreen;
  public GameObject videoScreen;

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
		ChangeScene("SampleScene");
  }  

  public void menu_SETTINGS() {
    ChangeActiveSettingsElement(settingsScreen);
  }

  public void menu_QUIT() {
    Debug.Log("Quit Game");
    Application.Quit();
  }

	// --------------------------------
	// Settings Button Scripts
	// --------------------------------
	public void settings_MOUSE() {
		ChangeActiveSettingsElement(mouseScreen);
	}

  public void settings_KEYBINDS() {
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
	// Mouse Button Scripts
	// --------------------------------

  public void mouse_SENSITIVITY() {
  // TODO: Change Sensitivity
  }

  // --------------------------------
	// Keybinds Button Scripts
	// --------------------------------

	// --------------------------------
	// Audio Button Scripts
	// --------------------------------

  public void audio_GLOBAL() {
    // TODO: Change Global Volume
  }

  public void audio_SFX() {
    // TODO: Change Effect Volume
  }

  public void audio_MUSIC() {
    // TODO: Change Music Volume
  }

	// --------------------------------
	// Video Button Scripts
	// --------------------------------

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
    currentActiveElement.SetActive(false);
    currentActiveElement = targetElement;
    currentActiveElement.SetActive(true);
  }
}
