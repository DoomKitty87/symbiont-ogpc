using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{

	public IDictionary<string, GameObject> menuScreens;

	private MenuScreenAnimations menuScreenAnimations;
	private PauseHandler pauseHandler;

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
			{ "Armor Menu", FindObjectOfName("Armor Menu") }
		};

		menuScreenAnimations = GetComponent<MenuScreenAnimations>();
		pauseHandler = GameObject.FindWithTag("Handler").GetComponent<PauseHandler>();
	}

	public void ButtonHideScreen() {
		// pauseHandler.currentActiveElement = null;
		StartCoroutine(menuScreenAnimations.ChangeMenuScreen(null));
	}

	public void ButtonChangeMenuScreen(string targetScreen){
		if (targetScreen == "") {
			// GameObject.FindWithTag("Handler").GetComponent<PauseHandler>().UnPause();
		} else {
			try {
				StartCoroutine(menuScreenAnimations.ChangeMenuScreen(menuScreens[targetScreen]));
			} catch (System.Exception ex) {
				menuScreens[targetScreen].SetActive(true);
			}
			// pauseHandler.currentActiveElement = menuScreens[targetScreen];
			// Debug.Log(pauseHandler.currentActiveElement);
		}
	}

	public void ButtonChangeScene(string targetScene) {
		StartCoroutine(menuScreenAnimations.ChangeScene(targetScene));
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
