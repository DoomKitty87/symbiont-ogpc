using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class SliderScript : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private Slider _slider;
	[SerializeField] private TextMeshProUGUI valueText;
	[SerializeField] private string _settingsKey;
	[SerializeField] private float _defaultValue;
	
  void OnEnable() {
		if (!PlayerPrefs.HasKey(_settingsKey)) PlayerPrefs.SetFloat(_settingsKey, _defaultValue);
		_slider.value = PlayerPrefs.GetFloat(_settingsKey);
	}

	private void Start() {
    _slider = gameObject.GetComponent<Slider>();
    _slider.value = PlayerPrefs.GetFloat(_settingsKey);
		ReplaceTextValue();
	}

  private void Update() {
    ReplaceTextValue();
  }

	public void ReplaceTextValue() {
		valueText.text = System.Math.Round(GetSliderValue(), 2).ToString() + ExtraText(System.Math.Round(GetSliderValue(), 2));
		PlayerPrefs.SetFloat(_settingsKey, GetSliderValue());
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
		return _slider.value;
	}

}
