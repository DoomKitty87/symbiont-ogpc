using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScript : MonoBehaviour
{

  [SerializeField] private GameObject _creditsContent;
  [SerializeField] private float _creditsScrollSpeed;

  public void StartCredits() {
    StopCoroutine("StartCreditsCoroutine");
    StartCoroutine(StartCreditsCoroutine());
  }

  private IEnumerator StartCreditsCoroutine() {
    _creditsContent.transform.localPosition = new Vector3(0, -Screen.height / 2f, 0);
    while (_creditsContent.transform.GetChild(_creditsContent.transform.childCount - 1).position.y < Screen.height / 2f) {
      _creditsContent.transform.localPosition += new Vector3(0, _creditsScrollSpeed * Time.deltaTime, 0);
      yield return null;
    }
  }
}