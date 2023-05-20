using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SpeedUpDisplay : MonoBehaviour
{

  public float _timeToCharge;

  [HideInInspector] public float _rechargeAmt;

  private PauseHandler _pauseHandler;
  private VolumeProfile _profile;
  private LensDistortion _lensDist;
  private ChromaticAberration _chromaticAb;

  private void Start() {
    _pauseHandler = GameObject.FindGameObjectWithTag("Handler").GetComponent<PauseHandler>();
    _profile = GameObject.FindGameObjectWithTag("Post Processing").GetComponent<Volume>().profile;
    _profile.TryGet(out _lensDist);
    _profile.TryGet(out _chromaticAb);
  }
  
  private void Update() {
    if (Input.GetKey(KeyCode.E) && _rechargeAmt > 0) TriggerSpeedUp();
    else StopSpeedUp();
    if (Time.timeScale > 0) _rechargeAmt += Time.unscaledDeltaTime * 0.5f;
    if (_rechargeAmt > _timeToCharge) _rechargeAmt = _timeToCharge;
  }

  private void TriggerSpeedUp() {
    if (_pauseHandler._pauseState != PauseHandler.PauseState.Unpaused) return;
    _rechargeAmt -= Time.unscaledDeltaTime * 1.5f;
    Time.timeScale = Mathf.Lerp(Time.timeScale, 2.5f, 0.1f);
    _lensDist.intensity.value = Mathf.Lerp(_lensDist.intensity.value, -0.6f, 0.05f);
    _chromaticAb.intensity.value = Mathf.Lerp(_chromaticAb.intensity.value, 8f, 0.05f);
  }

  private void StopSpeedUp() {
    if (Time.timeScale == 1f) return;
    if (_pauseHandler._pauseState != PauseHandler.PauseState.Unpaused) return;
    Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, 0.1f);
    _lensDist.intensity.value = Mathf.Lerp(_lensDist.intensity.value, 0f, 0.05f);
    _chromaticAb.intensity.value = Mathf.Lerp(_chromaticAb.intensity.value, 0f, 0.05f);
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