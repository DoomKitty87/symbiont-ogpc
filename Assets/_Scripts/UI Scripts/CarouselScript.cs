using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class CarouselItem {
  [Header("Display")]
  public string displayName;
  public enum Type { Int, Float, Bool };
  [Header("Type")]
  public Type type;
  [Header("Values")]
  public int intValue;
  public float floatValue;
  public bool boolValue;
}

public class CarouselScript : MonoBehaviour
{

	[Header("References")]
	[SerializeField] private Button _previousButton;
  [SerializeField] private Button _nextButton;
  [SerializeField] private TextMeshProUGUI _displayText;
	
	[Header("Setup")]
	[SerializeField] private string _settingsKey;
  
  [Header("Settings")]
	[SerializeField] private List<CarouselItem> _presets;
  [SerializeField] private int _currentPresetIndex;

  private void Start() {
    if (_presets.Count == 0) {
      Debug.LogError("CarouselScript: No presets!");
      return;
    }
    if (_currentPresetIndex < 0 || _currentPresetIndex >= _presets.Count) {
      Debug.LogError("CarouselScript: Current preset index is out of range!");
      _currentPresetIndex = 0;
      return;
    }
    _previousButton.onClick.AddListener(PreviousItem);
    _nextButton.onClick.AddListener(NextItem);
    UpdateDisplayAndValues();
  }

  public void NextItem() {
    if (_currentPresetIndex + 1 >= _presets.Count) {
      _currentPresetIndex = 0;
    } else {
      _currentPresetIndex++;
    }
    UpdateDisplayAndValues();
  }

  public void PreviousItem() {
    if (_currentPresetIndex - 1 < 0) {
      _currentPresetIndex = _presets.Count - 1;
    } else {
      _currentPresetIndex--;
    }
    UpdateDisplayAndValues();
  }

	private void UpdateDisplayAndValues() {
    CarouselItem currentItem = _presets[_currentPresetIndex];
    _displayText.text = currentItem.displayName;
    switch (currentItem.type) {
      case CarouselItem.Type.Int:
        PlayerPrefs.SetInt(_settingsKey, currentItem.intValue);
        break;
      case CarouselItem.Type.Float:
        PlayerPrefs.SetFloat(_settingsKey, currentItem.floatValue);
        break;
      case CarouselItem.Type.Bool:
        PlayerPrefs.SetInt(_settingsKey, currentItem.boolValue ? 1 : 0);
        break;
    }
  }
}
