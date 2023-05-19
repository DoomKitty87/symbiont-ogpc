using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedUpDisplay : MonoBehaviour
{

  [SerializeField] private GameObject _rechargeBar;
  [SerializeField] private float _timeToCharge;

  private float _rechargeAmt;
  private bool _speedingUp;
  
  private void Update() {
    if (Input.GetKeyDown(KeyCode.E)) TriggerSpeedUp();
    _rechargeAmt += Time.unscaledDeltaTime;
    if (Time.timeScale == 1f) _rechargeBar.GetComponent<Image>().fillAmount = _rechargeAmt / _timeToCharge;
  }

  private void TriggerSpeedUp() {
    if (_rechargeAmt < _timeToCharge) return;
    if (_speedingUp) return;
    StartCoroutine(SpeedUpGame());
  }

  private IEnumerator SpeedUpGame() {
    _rechargeAmt = 0;
    _rechargeBar.GetComponent<Image>().fillAmount = 0;
    _speedingUp = true;
    float timeElapsed = 0;
    float duration = 0.25f;
    while (timeElapsed < duration) {
      Time.timeScale = Mathf.SmoothStep(1, 2.5f, timeElapsed / duration);
      timeElapsed += Time.unscaledDeltaTime;
      yield return null;
    }
    Time.timeScale = 2.5f;
    yield return new WaitForSeconds(2f);
    timeElapsed = 0;
    while (timeElapsed < duration) {
      Time.timeScale = Mathf.SmoothStep(2.5f, 1, timeElapsed / duration);
      timeElapsed += Time.unscaledDeltaTime;
      yield return null;
    }
    Time.timeScale = 1f;
    _speedingUp = false;
  }
}