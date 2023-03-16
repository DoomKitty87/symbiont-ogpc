using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{


	private MenuScreenAnimations menuScreenAnimations;
	private PauseHandler pauseHandler;

	private void Awake() {
		menuScreenAnimations = GetComponent<MenuScreenAnimations>();
		pauseHandler = GameObject.FindWithTag("Handler").GetComponent<PauseHandler>();
	}

	public void ButtonChangeScene(string targetScene) {
		StartCoroutine(menuScreenAnimations.ChangeScene(targetScene));
	}

	public void ButtonInitPause() {
		pauseHandler.InitPause();
	}

	public void ButtonAddPause(string s) {
		pauseHandler.AddPause(s);
	}

	public void ButtonRemovePause() {
		pauseHandler.RemovePause();
	}


}
