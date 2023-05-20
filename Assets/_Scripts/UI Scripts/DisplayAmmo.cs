using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayAmmo : MonoBehaviour
{
  [Header("Text")]
  [SerializeField] private TextMeshProUGUI _maxAmmoText, _middleAmmoText;
  [Header("Images")]
  [SerializeField] private Image _ammoBar, _ammoCircle;
  [SerializeField] private AnimationCurve _easeCurve;
  [SerializeField] private float _easeTime;

  private float _maxAmmo;
  private float _currentAmmo;

  // Called by WeaponInventory
  public void InitializeAmmo(WeaponItem[] weapons, WeaponItem weapon) {
    _maxAmmoText.text = weapon.magSize.ToString();
    _middleAmmoText.text = (weapon.magSize / 2).ToString();
    _currentAmmo = weapon.magSize;
    _maxAmmo = weapon.magSize;
  }
  
  // Called by WeaponInventory
  public void UpdateForNewValues(WeaponItem weapon, int ammoCount) {
    StartCoroutine(TweenTextValue(_maxAmmoText, _maxAmmo, weapon.magSize, _easeTime));
    _maxAmmo = weapon.magSize;
    _currentAmmo = ammoCount;
  }
  // Called by FireWeaponLogic
  public void OnFireAmmo(float currentAmmo) {
    _currentAmmo--;
    UpdateAmmoImages(_currentAmmo, _maxAmmo);
  }
  // Called by FireWeaponLogic
  public void OnReloadAmmo(int magSize) {
    _maxAmmo = magSize;
    _currentAmmo = magSize;
    UpdateAmmoImages(_currentAmmo, _maxAmmo);
  }

  private void UpdateAmmoImages(float currentAmmo, float maxAmmo) {
    StartCoroutine(TweenImage(_ammoBar, currentAmmo / maxAmmo, _easeTime));
    StartCoroutine(TweenImage(_ammoCircle, currentAmmo / maxAmmo, _easeTime));
  }

  private IEnumerator TweenTextValue(TextMeshProUGUI text, float startValue, float targetValue, float duration) {
    float timeElapsed = 0;
    while (timeElapsed < duration) {
      timeElapsed += Time.deltaTime;
      float t = timeElapsed / duration;
      t = _easeCurve.Evaluate(t);
      text.text = Mathf.Floor(Mathf.Lerp(startValue, targetValue, t)).ToString();
      yield return null;
    }
    text.text = targetValue.ToString();
  }
  private IEnumerator TweenImage(Image image, float targetValue, float duration) {
    float timeElapsed = 0;
    float initImageValue = image.fillAmount;
    while (timeElapsed < duration) {
      timeElapsed += Time.deltaTime;
      float t = timeElapsed / duration;
      t = _easeCurve.Evaluate(t);
      image.fillAmount = Mathf.Lerp(initImageValue, targetValue, t);
      yield return null;
    }
    image.fillAmount = targetValue;
  }
}