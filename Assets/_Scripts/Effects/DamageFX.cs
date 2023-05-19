using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DamageFX : MonoBehaviour
{

  private VolumeProfile profile;
  private MotionBlur motionBlur;

  private void Start() {
    profile = GameObject.FindGameObjectWithTag("Post Processing").GetComponent<Volume>().profile;
    profile.TryGet(out motionBlur);
  }

  public void OnHealthChanged(float initHealth, float newHealth, float maxHealth) {
    if (newHealth > initHealth) return;
    if (GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting != gameObject) return;
    StopAllCoroutines();
    StartCoroutine(ScreenBlurEffect());
  }

  private IEnumerator ScreenBlurEffect() {
    float timeElapsed = 0;
    float duration = 0.2f;
    float blurMax = 1;
    float initBlur = motionBlur.intensity.value;
    while (timeElapsed < duration) {
      motionBlur.intensity.Override(Mathf.SmoothStep(initBlur, blurMax, timeElapsed / duration));
      timeElapsed += Time.deltaTime;
      yield return null;
    }
    timeElapsed = 0;
    while (timeElapsed < duration) {
      motionBlur.intensity.Override(Mathf.SmoothStep(blurMax, 0f, timeElapsed / duration));
      timeElapsed += Time.deltaTime;
      yield return null;
    }
    motionBlur.intensity.Override(0f);
  }
}