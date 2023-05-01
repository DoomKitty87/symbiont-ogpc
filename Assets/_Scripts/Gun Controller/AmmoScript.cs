using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoScript : MonoBehaviour
{
  // For display only
  [SerializeField] private WeaponItem _currentWeaponItem;

  [Header("UI")]
  [SerializeField] private Image _imageToChangeColor;
  [SerializeField] private TextMeshProUGUI _currentAmmoText;
  [SerializeField] private TextMeshProUGUI _maxAmmoText;

  [Header("Colors")]
  [SerializeField] private Color32 _defaultColor;
  [SerializeField] private Color32 _lowAmmoColor;
  [Header("Values")]
  // Needs to be public because GunController depends
  [SerializeField] public int _maxAmmo;
  [SerializeField] public int _currentAmmo;
  [Header("Easing")]
  [SerializeField] private float _easeDuration;
  private float _reloadDuration;
  [SerializeField] private AnimationCurve _easeCurve;

  private void Start() {
    _imageToChangeColor = GetComponent<Image>();
    _defaultColor = _imageToChangeColor.color;
  }

  public void UpdateForNewValues(WeaponItem weaponItem, int ammoCount) {
    _currentWeaponItem = weaponItem;
    StartCoroutine(TweenTextValue(_maxAmmoText, _maxAmmo, weaponItem.magSize, _easeDuration, 0));
    StartCoroutine(TweenTextValue(_currentAmmoText, _currentAmmo, ammoCount, _easeDuration, 0));
    _maxAmmo = weaponItem.magSize;
    _currentAmmo = ammoCount;
    _reloadDuration = weaponItem.reloadTimeSeconds;
    UpdateColor();
  }

  public void DecrementAmmo() {
    _currentAmmo--;
    UpdateColor();
    _currentAmmoText.text = _currentAmmo.ToString();
  }

  // Despite ResetAmmo being called OnReloadStart, we still want to wait until the reload is 85% done before starting the easing 
  public void ResetAmmo() {
    Invoke("ResetAmmoDelay", _reloadDuration * 0.85f);
  }
  private void ResetAmmoDelay() {
    StartCoroutine(TweenTextValue(_currentAmmoText, _currentAmmo, _maxAmmo, _easeDuration, 0));
    _currentAmmo = _maxAmmo;
    UpdateColor();
  }

  private void UpdateColor() {
    if (_currentAmmo != 0 && _maxAmmo != 0) {
			// If you aren't dividing by zero, exponentially raise colorCoefficient based on how small currAmmo is in respect to maxAmmo
			float colorCoefficient = ((_maxAmmo / _currentAmmo) * (255 / _maxAmmo));
      _imageToChangeColor.color = Color.Lerp(_defaultColor, _lowAmmoColor, colorCoefficient / 255);
    }
  }

  private IEnumerator TweenTextValue(TextMeshProUGUI text, float startValue, float targetValue, float duration, int decimalPlaces)
  {
    float timeElapsed = 0;
    // float initSliderValue = slider.value;
    while (timeElapsed < duration)
    {
      timeElapsed += Time.deltaTime;
      float t = timeElapsed / duration;
      t = _easeCurve.Evaluate(t);
      text.text = Math.Round(Mathf.Lerp(startValue, targetValue, t), decimalPlaces).ToString();
      yield return null;
    }
    text.text = targetValue.ToString();
  }
}
