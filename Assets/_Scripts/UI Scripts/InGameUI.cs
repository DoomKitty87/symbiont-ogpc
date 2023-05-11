using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameUI : MonoBehaviour
{

  [SerializeField] private GameObject _healthHolder, _ammoHolder;
  [SerializeField] private AnimationCurve _easeCurve;
  [SerializeField] private float _easeTime;

  private float _maxAmmo;
  private float _currentAmmo;

  public void InitializeHealth(float maxHealth) {
    _healthHolder.transform.GetChild(0).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = ((int)maxHealth).ToString();
    _healthHolder.transform.GetChild(0).GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = ((int)(maxHealth / 2)).ToString();
  }

  public void InitializeAmmo(WeaponItem[] weapons, WeaponItem weapon) {
    _ammoHolder.transform.GetChild(0).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = ((int)weapon.magSize).ToString();
    _ammoHolder.transform.GetChild(0).GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = ((int)(weapon.magSize / 2)).ToString();
  }

  public void UpdateAmmoValues(float currMax, float maxAmmo) {
    StartCoroutine(TweenAmmoVals(_ammoHolder.transform.GetChild(0).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>(), _ammoHolder.transform.GetChild(0).GetChild(3).gameObject.GetComponent<TextMeshProUGUI>(), currMax, maxAmmo, _easeTime));
  }

  public void UpdateHealth(float initHealth, float health, float maxHealth) {
    StopCoroutine("TweenImage1");
    StartCoroutine(TweenImage1(_healthHolder.transform.GetChild(0).gameObject.GetComponent<Image>(), _healthHolder.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Image>(), health / maxHealth, _easeTime));
  }

  public void UpdateAmmo(float initAmmo, float ammo, float maxAmmo) {
    StopCoroutine("TweenImage2");
    StartCoroutine(TweenImage2(_ammoHolder.transform.GetChild(0).gameObject.GetComponent<Image>(), _ammoHolder.transform.GetChild(0).GetChild(1).GetComponent<Image>(), ammo / maxAmmo, _easeTime));
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
    UpdateAmmoValues(_maxAmmo, weapon.magSize);
    _maxAmmo = weapon.magSize;
    UpdateAmmo(_currentAmmo, ammoCount, _maxAmmo);
    _currentAmmo = ammoCount;
  }

  private IEnumerator TweenAmmoVals(TextMeshProUGUI text1, TextMeshProUGUI text2, float oldAmmo, float newAmmo, float duration) {
    float timeElapsed = 0;
    while (timeElapsed < duration) {
      timeElapsed += Time.deltaTime;
      float t = timeElapsed / duration;
      t = _easeCurve.Evaluate(t);
      text1.text = ((int)Mathf.Lerp(oldAmmo, newAmmo, t)).ToString();
      text2.text = ((int)Mathf.Lerp(oldAmmo / 2, newAmmo / 2, t)).ToString();
      yield return null;
    }
    text1.text = ((int)(newAmmo)).ToString();
    text2.text = ((int)(newAmmo / 2)).ToString();
  }

  private IEnumerator TweenImage1(Image image, Image image2, float targetValue, float duration) {
    float timeElapsed = 0;
    float initFillAmount = image.fillAmount;
    float initFillAmount2 = image2.fillAmount;
    while (timeElapsed < duration) {
      timeElapsed += Time.deltaTime;
      float t = timeElapsed / duration;
      t = _easeCurve.Evaluate(t);
      image.fillAmount = Mathf.Lerp(initFillAmount, targetValue, t);
      image2.fillAmount = Mathf.Lerp(initFillAmount2, targetValue, t);
      yield return null;
    }
    image.fillAmount = targetValue;
    image2.fillAmount = targetValue;
  }

  private IEnumerator TweenImage2(Image image, Image image2, float targetValue, float duration) {
    float timeElapsed = 0;
    float initFillAmount = image.fillAmount;
    float initFillAmount2 = image2.fillAmount;
    while (timeElapsed < duration) {
      timeElapsed += Time.deltaTime;
      float t = timeElapsed / duration;
      t = _easeCurve.Evaluate(t);
      image.fillAmount = Mathf.Lerp(initFillAmount, targetValue, t);
      image2.fillAmount = Mathf.Lerp(initFillAmount2, targetValue, t);
      yield return null;
    }
    image.fillAmount = targetValue;
    image2.fillAmount = targetValue;
  }
}