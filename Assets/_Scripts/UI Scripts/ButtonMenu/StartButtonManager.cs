using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class StartButtonManager : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private Button _startButton;
  [SerializeField] private FadeElementInOut _fadingObject;
  [Header("Settings")]
  [SerializeField] private string _sceneToSwitchToName; 

  private void Start() {
    if (_startButton == null) {
      _startButton = gameObject.GetComponent<Button>();
    }
    _startButton.onClick.AddListener(OnStartClick);
  }

  public void OnStartClick() {
    _fadingObject._OnFadeComplete.AddListener(OnFadeFinish);
    _fadingObject.FadeIn(false);  
  }
  public void OnFadeFinish() {
    SceneManager.LoadScene(_sceneToSwitchToName);
  }

}
