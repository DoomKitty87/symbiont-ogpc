using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CheckboxScript : MonoBehaviour
{

  [Header("Setup")]
	[SerializeField] private int _defaultValue;
	[SerializeField] private string _settingsKey;

	[Header("References")]
  [SerializeField] private Toggle _checkbox;

  private bool _bufferValue;

  void OnEnable() {
    if (!PlayerPrefs.HasKey(_settingsKey)) PlayerPrefs.SetInt(_settingsKey, _defaultValue);

    _checkbox.isOn = (PlayerPrefs.GetInt(_settingsKey) == 1);
    _bufferValue = _checkbox.isOn;
  }

  private void Start() {
    _checkbox.isOn = (PlayerPrefs.GetInt(_settingsKey) == 1);
    _checkbox.onValueChanged.AddListener((v) => {
      _bufferValue = v;
    });
  }

  public void ResetValue() {
    PlayerPrefs.SetInt(_settingsKey, _defaultValue);
    _checkbox.isOn = _defaultValue == 1;
  }

  public void Apply() {
    PlayerPrefs.SetInt(_settingsKey, _bufferValue ? 1 : 0);
  }
}
