	using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class SliderScript : MonoBehaviour
{

	[SerializeField] private GameObject valueText;
	[SerializeField] private string settingsKey;
	[SerializeField] private float defaultValue;

	private float tempValue;

	private void Start() {
		tempValue = PlayerPrefs.GetFloat(settingsKey);
		GetComponent<Slider>().value = PlayerPrefs.GetFloat(settingsKey);
		ReplaceTextValue();
	}

	public void ReplaceTextValue() {
		valueText.GetComponent<TextMeshProUGUI>().text = System.Math.Round(GetSliderValue(), 2).ToString() + ExtraText(System.Math.Round(GetSliderValue(), 2));
		PlayerPrefs.SetFloat(settingsKey, GetSliderValue());
		GameObject.FindGameObjectWithTag("Handler").GetComponent<PlayerSettings>().ApplySettings(); // Probably at some point should clean this up
	}

	private string ExtraText(double value) {
		if (value == 0 || value == 1) {
			return ".00";
		} else if (value == 0.1 || value == 0.2 || value == 0.3 || value == 0.4 || value == 0.5 || value == 0.6 || value == 0.7 || value == 0.8 || value == 0.9) {
			// REALLY DUMB Need to fix this
			return "0";
		} else return null;
	}

	private float GetSliderValue() {
		return GetComponent<Slider>().value;
	}

	void OnEnable() {
		if (!PlayerPrefs.HasKey(settingsKey)) PlayerPrefs.SetFloat(settingsKey, defaultValue);
		GetComponent<Slider>().value = PlayerPrefs.GetFloat(settingsKey);
	}

}
