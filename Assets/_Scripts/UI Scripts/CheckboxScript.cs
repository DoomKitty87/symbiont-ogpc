using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CheckboxScript : MonoBehaviour
{

  [Header("Setup")]
	[SerializeField] private float _defaultValue;
	[SerializeField] private string _settingsKey;

	[Header("References")]
  [SerializeField] private Checkbox _checkbox;
  [SerializeField] private TextMeshProUGUI _text;

	[Header("Layout")]
	[SerializeField] private string _floatLayout;

  void OnEnable() {
    if (!PlayerPrefs.HasKey(_settingsKey)) PlayerPrefs.SetBool(_settingsKey, _defaultValue);

    _checkbox.value = PlayerPrefs.GetBool(_settingsKey);
  }

  private void Start() {
    _checkbox.value = PlayerPrefs.GetBool(_settingsKey);
    _checkbox.onValueChanged.AddListener((v) => {
      PlayerPrefs.SetBool(_settingsKey, v);
    })
  }
}
