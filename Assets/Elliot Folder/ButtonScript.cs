using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] PauseHandler pauseHandler;

    public void game_RESUME() {
        pauseHandler.UnPause();
    }

    public void game_RESTART(string currentScene) {
        pauseHandler.UnPause();
        SwitchScene(currentScene);
    }

    public void game_LOAD() {

    }

    public void game_SETTINGS() {

    }

    public void game_QUIT() {
        SwitchScene("MainMenu");
    }

    public void menu_PLAY() {
        SwitchScene("SampleScene");
    }  

    public void menu_SETTINGS() {

    }

    public void menu_QUIT() {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    private void SwitchScene(string targetScene) {
        SceneManager.LoadScene(targetScene);
    }
}
