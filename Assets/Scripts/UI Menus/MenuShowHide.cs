using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuShowHide : MonoBehaviour
{
  [SerializeField] private List<GameObject> _menuElements;
  [Header("For Fading - Optional")]
  [SerializeField] private bool _fading;
  [SerializeField] private CanvasGroup _canvasGroup;
  [SerializeField] private float _fadeTime;

  // Start is called before the first frame update
  private void Start() {
    if (_menuElements.Count == 0) {
      Debug.LogError("MenuShowHide: Menu Elements are null!", gameObject);
      this.enabled = false;
    }
    if (_fading && _canvasGroup == null)
    {
      Debug.LogError("MenuShowHide: Canvas Group is null!", gameObject);
      this.enabled = false;
    }
  }
  public void Show() {
    if (_fading) {
      foreach (GameObject element in _menuElements) {
        element.SetActive(true);
      }
      StartCoroutine(ShowCanvasGroup());
    }
    else {
      foreach (GameObject element in _menuElements) {
        element.SetActive(true);
      }
    }
  }
  public void Hide() {
    if (_fading) {
      foreach (GameObject element in _menuElements) {
        element.SetActive(false);
      }
      StartCoroutine(HideCanvasGroup());
    }
    else {
      foreach (GameObject element in _menuElements) {
        element.SetActive(false);
      }
    }
  }
  private IEnumerator ShowCanvasGroup()
  {
    float timeElapsed = 0;
    while (timeElapsed < _fadeTime) {
      timeElapsed += Time.deltaTime;
      _canvasGroup.alpha = timeElapsed / _fadeTime;
      yield return null;
    }
    _canvasGroup.alpha = 1;
  } 
  private IEnumerator HideCanvasGroup()
  {
    float timeElapsed = 0;
    while (timeElapsed < _fadeTime) {
      timeElapsed += Time.deltaTime;
      _canvasGroup.alpha = 1 - (timeElapsed / _fadeTime);
      yield return null;
    }
    _canvasGroup.alpha = 0;
  } 
}

