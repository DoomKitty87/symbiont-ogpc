using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
	
	[Header("Setup")]
	[SerializeField] private float _minValue;
	[SerializeField] private float _maxValue;
	[SerializeField] private float _defaultValue;
	[SerializeField] private string _settingsKey;

	[Header("References")]
  [SerializeField] private Slider _slider;
  [SerializeField] private TextMeshProUGUI _text;
	[SerializeField] private TMP_InputField _inputField;

	[Header("Layout")]
	[SerializeField] private string _floatLayout;

  void OnEnable() {
		if (!PlayerPrefs.HasKey(_settingsKey)) PlayerPrefs.SetFloat(_settingsKey, _defaultValue);

		// Sets values of slider and slider text to PlayerPrefs value
		_slider.value = PlayerPrefs.GetFloat(_settingsKey);
		if (_inputField) _inputField.text = _slider.value.ToString(_floatLayout);
		if (_text) _text.text = _slider.value.ToString(_floatLayout);
	}

	private void Start() {
    _slider.value = PlayerPrefs.GetFloat(_settingsKey);

		_slider.onValueChanged.AddListener((v) => {
			if (_inputField) _inputField.text = v.ToString(_floatLayout);
			if (_text) _text.text = v.ToString(_floatLayout);
			PlayerPrefs.SetFloat(_settingsKey, v);
		});

		_slider.minValue = _minValue;
		_slider.maxValue = _maxValue;

		_inputField.onEndEdit.AddListener(SetSliderValue);
	}

	private void SetSliderValue(string value) {
    // Changed activation to onEndEdit; player may not always press enter
		float updateNum = Mathf.Clamp(float.Parse(value), _slider.minValue, _slider.maxValue);
		_slider.value = updateNum;
	}

  public void ResetValue() {
    PlayerPrefs.SetFloat(_settingsKey, _defaultValue);
    _slider.value = _defaultValue;
  }
}
