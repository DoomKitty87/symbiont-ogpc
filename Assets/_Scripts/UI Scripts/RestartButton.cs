using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{

  public void RestartGame() {
    Time.timeScale = 1f;
    GameObject.FindGameObjectWithTag("Handler").GetComponent<PauseHandler>().Unpause();
    SceneManager.LoadScene("Game");
  }
}