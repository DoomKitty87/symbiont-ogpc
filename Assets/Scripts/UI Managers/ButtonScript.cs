using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{

	public IDictionary<string, GameObject> menuScreens;

	[HideInInspector] public GameObject currentActiveElement;

	// private PauseHandler pauseHandler;
	private MenuScreenAnimations menuScreenAnimations;

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
	}

	public void ButtonHideScreen() {
		currentActiveElement = null;
		StartCoroutine(menuScreenAnimations.ChangeMenuScreen(null));
	}

	public void ButtonChangeMenuScreen(string targetScreen){
		Debug.Log(menuScreenAnimations);
		if (targetScreen == "") {
			GameObject.FindWithTag("Handler").GetComponent<PauseHandler>().UnPause();
		} else {
			StartCoroutine(menuScreenAnimations.ChangeMenuScreen(menuScreens[targetScreen]));
			currentActiveElement = menuScreens[targetScreen];
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
