using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class OnSceneLoad : MonoBehaviour
{
  // Dumb, I know, but it didn't feel right as a part of the FadeElementInOut script.  
  public UnityEvent onSceneLoad;

  private void Start() {
      onSceneLoad.Invoke();
  }
}
