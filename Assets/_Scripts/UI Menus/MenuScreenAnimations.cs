using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScreenAnimations : MonoBehaviour
{

  private GameObject parentObject;
  private Animator animator;
  private PauseHandler pauseHandler;
  private ButtonScript buttonScript;

  private void Awake() {
    parentObject = TryGetParentObject();
    buttonScript = GetComponent<ButtonScript>();

  }

  private void Start() {
    animator = parentObject.GetComponent<Animator>();
    
    pauseHandler = GameObject.FindWithTag("Handler").GetComponent<PauseHandler>();
  }

	private void OnEnable() {
    // animator.SetBool("closing", false);
	}

  private GameObject TryGetParentObject() {
    GameObject tempObject = gameObject;
    while (tempObject != null) {
      if (tempObject.CompareTag("MenuScreen")) return tempObject;
      else tempObject = tempObject.transform.parent.gameObject;
    }
    Debug.LogError("Script MenuScreenAnimations contains no GameObject tagged 'MenuScreen' above it");
    return null;
  }

  public IEnumerator CloseScreen(GameObject objectThatIsClosing) {
    animator.SetBool("closing", true);

    float timeToWait = 0.6f;
    yield return new WaitForSecondsRealtime(timeToWait);
    objectThatIsClosing.SetActive(false);
    pauseHandler.ChangeScreen();
  }

  public IEnumerator ChangeScene(string targetScreen) {
		animator.SetBool("closing", true);
    float timeToWait = 0.6f;
    yield return new WaitForSecondsRealtime(timeToWait);

    Time.timeScale = 1f;
    SceneManager.LoadScene(targetScreen);
  }
}
