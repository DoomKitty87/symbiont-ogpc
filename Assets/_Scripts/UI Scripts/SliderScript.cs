using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class SliderScript : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private Slider _slider;
  [SerializeField] private TextMeshProUGUI _text;
	[SerializeField] private TMP_InputField _inputField;
	[SerializeField] private string _settingsKey;
	[SerializeField] private float _defaultValue;
  [SerializeField] private bool _addZerosToTenths;
	
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
    if (_slider.value != PlayerPrefs.GetFloat(_settingsKey)) {
      ReplaceTextValue();
    }
  }

	private void ReplaceTextValue() {
		string value = System.Math.Round(GetSliderValue(), 2).ToString();
    if (_addZerosToTenths) value += ExtraText(System.Math.Round(GetSliderValue(), 2));
    
		PlayerPrefs.SetFloat(_settingsKey, GetSliderValue());
    if (_text != null) _text.text = value;
    if (_inputField != null) _inputField.text = value;
	}

  // Called by event in Input Field or Text
  public void SetSliderValue(string value) {
    float floatValue = float.Parse(value);
    if (floatValue > _slider.maxValue) floatValue = _slider.maxValue;
    if (floatValue < _slider.minValue) floatValue = _slider.minValue;
    _slider.value = floatValue;
    ReplaceTextValue();
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
