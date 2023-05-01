using System.Collections;
using UnityEngine;

public class BounceEffect : MonoBehaviour
{
  [SerializeField][Range(0, 2)] private float _bounceScaleMultiplier = 0.9f;
  [SerializeField] private float _bounceDurationSeconds = 0.075f;
  [SerializeField] private float _reboundDurationSeconds = 0.1f;
  public void StartEffect() {
    StopCoroutine(Bounce());
    StartCoroutine(Bounce());
  }
  private IEnumerator Bounce() {
    Vector3 initScale = transform.localScale;
    float timer = 0f;
    while (timer < _bounceDurationSeconds) {
      transform.localScale = Vector3.Lerp(initScale, initScale * _bounceScaleMultiplier, timer / _bounceDurationSeconds);
      timer += Time.deltaTime;
      yield return null;
    }
    timer = 0f;
    while (timer < _reboundDurationSeconds) {
      transform.localScale = Vector3.Lerp(initScale * _bounceScaleMultiplier, initScale, timer / _reboundDurationSeconds);
      timer += Time.deltaTime;
      yield return null;
    }
    transform.localScale = initScale;
  }
}
