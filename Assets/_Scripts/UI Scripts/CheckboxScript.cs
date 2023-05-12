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

	[Header("Layout")]
	[SerializeField] private string _floatLayout;

  void OnEnable() {
    if (!PlayerPrefs.HasKey(_settingsKey)) PlayerPrefs.SetInt(_settingsKey, _defaultValue);

    _checkbox.isOn = (PlayerPrefs.GetInt(_settingsKey) == 1);
  }

  private void Start() {
    _checkbox.isOn = (PlayerPrefs.GetInt(_settingsKey) == 1);
    _checkbox.onValueChanged.AddListener((v) => {
      PlayerPrefs.SetInt(_settingsKey, v ? 1 : 0);
    });
  }
}
