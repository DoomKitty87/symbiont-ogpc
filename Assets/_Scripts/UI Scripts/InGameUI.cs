using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{

  [SerializeField] private GameObject _healthHolder, _ammoHolder;
  [SerializeField] private AnimationCurve _easeCurve;
  [SerializeField] private float _easeTime;

  private float _maxAmmo;
  private float _currentAmmo;

  public void UpdateHealth(float initHealth, float health, float maxHealth) {
    StopCoroutine("TweenImage1");
    StartCoroutine(TweenImage1(_healthHolder.transform.GetChild(0).gameObject.GetComponent<Image>(), health / maxHealth, _easeTime));
  }

  public void UpdateAmmo(float initAmmo, float ammo, float maxAmmo) {
    StopCoroutine("TweenImage2");
    StartCoroutine(TweenImage2(_ammoHolder.transform.GetChild(0).gameObject.GetComponent<Image>(), ammo / maxAmmo, _easeTime));
  }

  public void OnFireAmmo(float currentAmmo) {
    _currentAmmo--;
    UpdateAmmo(currentAmmo, _currentAmmo, _maxAmmo);
  }

  public void OnReloadAmmo(int magSize) {
    _maxAmmo = magSize;
    _currentAmmo = magSize;
    UpdateAmmo(0, _currentAmmo, _maxAmmo);
  }

  public void UpdateForNewValues(WeaponItem weapon, int ammoCount) {
    _maxAmmo = weapon.magSize;
    UpdateAmmo(_currentAmmo, ammoCount, _maxAmmo);
    _currentAmmo = ammoCount;
  }

  private IEnumerator TweenImage1(Image image, float targetValue, float duration) {
    float timeElapsed = 0;
    float initFillAmount = image.fillAmount;
    while (timeElapsed < duration) {
      timeElapsed += Time.deltaTime;
      float t = timeElapsed / duration;
      t = _easeCurve.Evaluate(t);
      image.fillAmount = Mathf.Lerp(initFillAmount, targetValue, t);
      yield return null;
    }
    image.fillAmount = targetValue;
  }

  private IEnumerator TweenImage2(Image image, float targetValue, float duration) {
    float timeElapsed = 0;
    float initFillAmount = image.fillAmount;
    while (timeElapsed < duration) {
      timeElapsed += Time.deltaTime;
      float t = timeElapsed / duration;
      t = _easeCurve.Evaluate(t);
      image.fillAmount = Mathf.Lerp(initFillAmount, targetValue, t);
      yield return null;
    }
    image.fillAmount = targetValue;
  }
}