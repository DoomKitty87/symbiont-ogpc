using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ChangeSceneOnPress : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private Button _button;
  [SerializeField] private FadeElementInOut _fadingObject;
  [Header("Settings")]
  [SerializeField] private string _sceneToSwitchToName; 

  private void Start() {
    if (_button == null) {
      _button = gameObject.GetComponent<Button>();
    }
    _button.onClick.AddListener(OnStartClick);
  }

  public void OnStartClick() {
    _fadingObject._OnFadeComplete.AddListener(OnFadeFinish);
    _fadingObject.FadeIn(false);  
  }
  public void OnFadeFinish() {
    SceneManager.LoadScene(_sceneToSwitchToName);
  }

}
