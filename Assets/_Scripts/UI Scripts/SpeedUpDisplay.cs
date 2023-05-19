using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpDisplay : MonoBehaviour
{

  public float _timeToCharge;

  [HideInInspector] public float _rechargeAmt;
  
  private void Update() {
    if (Input.GetKeyDown(KeyCode.E)) TriggerSpeedUp();
    else StopSpeedUp();
    if (Time.timeScale > 0) _rechargeAmt += Time.unscaledDeltaTime;
  }

  private void TriggerSpeedUp() {
    if (_rechargeAmt <= 0) return;
    _rechargeAmt -= Time.deltaTime / Time.timeScale;
    Time.timeScale = Mathf.Lerp(Time.timeScale, 2.5f, 0.8f);
  }

  private void StopSpeedUp() {
    if (Time.timeScale == 1f) return;
    Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, 0.8f);
  }

  /*
  private IEnumerator SpeedUpGame() {
    _rechargeAmt = 0;
    Time.timeScale = 0.99f;
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
    yield return new WaitForSeconds(5f);
    timeElapsed = 0;
    while (timeElapsed < duration) {
      Time.timeScale = Mathf.SmoothStep(2.5f, 1, timeElapsed / duration);
      timeElapsed += Time.unscaledDeltaTime;
      yield return null;
    }
    Time.timeScale = 1f;
    _speedingUp = false;
  }
  */
}