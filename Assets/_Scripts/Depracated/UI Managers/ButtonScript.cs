using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
	private PauseHandler _pauseHandler;
	private Button _button;

	private enum ButtonType {
		AddPause,
		RemovePause,
		SwitchSceen
	}
	[SerializeField] private ButtonType _buttonType;

	[SerializeField] private string _screenName;

	private void Awake() {
		_pauseHandler = GameObject.FindWithTag("Handler").GetComponent<PauseHandler>();
		_button = GetComponent<Button>();
	}

	private void Start() {
		switch(_buttonType) {
			case ButtonType.AddPause:
				_button.onClick.AddListener(() => ButtonAddPause(_screenName));
				break;
			case ButtonType.RemovePause:
				_button.onClick.AddListener(() => ButtonRemovePause());
				break;
			case ButtonType.SwitchSceen:
				_button.onClick.AddListener(() => ButtomSwitchScreen(_screenName));
				break;
		}
	}

	public void ButtonAddPause(string s) {
		_pauseHandler.AddPause(s);
	}

	public void ButtonRemovePause() {
		_pauseHandler.RemovePause();
	}

	public void ButtomSwitchScreen(string s) {
	}
}
