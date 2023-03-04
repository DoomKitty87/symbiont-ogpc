using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{

	// Really bad but only runs on startup
	IDictionary<string, GameObject> menuScreens;

	[HideInInspector] public GameObject currentActiveElement;

	private PauseHandler pauseHandler;
	private MenuScreenAnimations menuScreenAnimations;

	private void Awake() {

		menuScreens = new Dictionary<string, GameObject>() {
			{ "pauseScreen", GameObject.Find("Pause Menu") },
			{ "settingsScreen", GameObject.Find("Settings Menu") },
			{ "checkScreen", GameObject.Find("Check Menu") },
			{ "controlsScreen", GameObject.Find("Controls Menu") },
			{ "audioScreen", GameObject.Find("Audio Menu") },
			{ "videoScreen", GameObject.Find("Video Menu") },
			{ "gunList", GameObject.Find("Gun Menu") },
			{ "armorList", GameObject.Find("Armor Menu") }
		};

		pauseHandler = GetComponent<PauseHandler>();
		currentActiveElement = menuScreens["pauseScreen"];
		menuScreenAnimations = GetComponent<MenuScreenAnimations>();
	}

	public void buttonChangeMenuScreen(string targetScreen){
		StartCoroutine(menuScreenAnimations.ChangeMenuScreen(menuScreens[targetScreen]));
	}

	public void buttonChangeScene(string targetScene) {
		StartCoroutine(menuScreenAnimations.ChangeScreen(targetScene));
	}

/*
	// --------------------------------
	// Start Button Scripts
	// --------------------------------

  public void start_PLAY() {
    // StartCoroutine(ChangeScene("MainMenu"));
  }

  public void start_SETTINGS() {
    ChangeActiveSettingsElement(settingsScreen);
  }

  public void start_QUIT() {
		Debug.Log("Quit Game");
		Application.Quit();
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
    ChangeActiveSettingsElement(gunList);
  }

  // public void menu_ARMORLIST() {
  //   if (armorList.activeSelf) {
  //     armorList.SetActive(false);
  //   } else {
  //     armorList.SetActive(true);
  //   }
  // }

  public void menu_QUIT() {
		// StartCoroutine(ChangeScene("StartMenu"));
  }

	// --------------------------------
	// Game Button Scripts
	// --------------------------------

	public void game_RESUME() {
		pauseHandler.UnPause();
	}

	public void game_RESTART(string currentScene) {
		pauseHandler.UnPause();
		// StartCoroutine(ChangeScene(currentScene));
	}

	public void game_LOAD() {
		// TODO
	}

	public void game_SETTINGS() {
		ChangeActiveSettingsElement(settingsScreen);
	}

	public void game_QUIT() {
    Time.timeScale = 1.0f;
		// StartCoroutine(ChangeScene("MainMenu"));
	}

	// --------------------------------
	// Settings Button Scripts
	// --------------------------------

	public void settings_CONTROLS() {
		ChangeActiveSettingsElement(controlsScreen);
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
  
  public void settings_RESET() {
    PlayerPrefs.DeleteAll();
    GetComponent<PlayerSettings>().ApplySettings();
  }

	// --------------------------------

  public void default_BACK() {
    ChangeActiveSettingsElement(settingsScreen);
  }

   IEnumerator ChangeScene(string targetScene) {
    yield return new WaitForSeconds(0.1f);
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
*/
}
