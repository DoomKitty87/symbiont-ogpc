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
  [SerializeField] public int _currAmmo;
  [Header("Easing")]
  [SerializeField] private float _easeDuration;
  [SerializeField] private AnimationCurve _easeCurve;
  private float colorCoefficient;
  

  private void Start() {
    _imageToChangeColor = GetComponent<Image>();
    _defaultColor = _imageToChangeColor.color;
    _currAmmo = 0;
    _maxAmmo = 0;
  }

  // TODO: FIX THIS
  public void UpdateForNewValues(WeaponItem weaponItem, int ammoCount) {
    _currentWeaponItem = weaponItem;
    StartCoroutine(TweenTextValue(_maxAmmoText, _maxAmmo, weaponItem.magSize, _easeDuration, 0));
    _maxAmmo = weaponItem.magSize;
    StartCoroutine(TweenTextValue(_currentAmmoText, _currAmmo, ammoCount, _easeDuration, 0));
    _currAmmo = ammoCount;
  }

  public void DecrementAmmo() {
    _currAmmo--;
    UpdateColor();
    _currentAmmoText.text = _currAmmo.ToString();
  }

  public void ResetAmmo() {
    StartCoroutine(TweenTextValue(_currentAmmoText, _currAmmo, _maxAmmo, _easeDuration, 0));
    _currAmmo = _maxAmmo;
    UpdateColor();
  }


  private void UpdateColor() {
    if (_currAmmo != 0 && _maxAmmo != 0) {
			// If you aren't dividing by zero, exponentially raise colorCoefficient based on how small currAmmo is in respect to maxAmmo
			colorCoefficient = ((_maxAmmo/_currAmmo) * (255 / _maxAmmo));
    }
    _imageToChangeColor.color = Color.Lerp(_defaultColor, _lowAmmoColor, colorCoefficient / 255);
  }

  private void UpdateText() {
    
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
