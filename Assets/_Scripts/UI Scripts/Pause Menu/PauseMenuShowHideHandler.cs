using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuShowHideHandler : MonoBehaviour
{
  [Header("References")] 
  [SerializeField] private Canvas _mainCanvas;
  [SerializeField] private CanvasGroup _initPageCanvasGroup;
  [SerializeField] private List<CanvasGroup> _hiddenCanvasGroups;

  public void ShowPauseMenu() {
    ResetCanvasGroupValues();
    _mainCanvas.enabled = true;
  }
  public void HidePauseMenu() {
    _mainCanvas.enabled = false;
  }

  public void ResetCanvasGroupValues() {
    _initPageCanvasGroup.alpha = 1.0f;
    _initPageCanvasGroup.interactable = true;
    _initPageCanvasGroup.blocksRaycasts = true;
    
    foreach (CanvasGroup canvasGroup in _hiddenCanvasGroups) {
      canvasGroup.alpha = 0f;
      canvasGroup.interactable = false;
      canvasGroup.blocksRaycasts = false;
    }
  }
}
