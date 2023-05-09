using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ExitButtonManager : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private Button _exitButton;
  [SerializeField] private FadeElementInOut _fadingObject;

  private bool _exitClicked = false;

  private void Start() {
    if (_exitButton == null) {
      _exitButton = gameObject.GetComponent<Button>();
    }
    _exitButton.onClick.AddListener(OnStartClick);
  }

  public void OnStartClick() {
    if (_exitClicked) return;
    _exitClicked = true;
    _fadingObject._OnFadeComplete.AddListener(OnFadeFinish);
    _fadingObject.FadeIn(false);  
  }
  public void OnFadeFinish() {
    #if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
    #else
      Application.Quit();
    #endif
  }

}
