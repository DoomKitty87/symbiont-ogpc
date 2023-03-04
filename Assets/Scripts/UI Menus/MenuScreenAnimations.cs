using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScreenAnimations : MonoBehaviour
{

  private Animator animator;

  [SerializeField] GameObject parentObject;

  private void Awake() {
    animator = parentObject.GetComponent<Animator>();
  }

	private void OnEnable() {
    animator.SetBool("closing", false);
	}

  // At some point I'll learn animation events but for now this is fine
	public IEnumerator ChangeMenuScreen(GameObject targetScene) {
    animator.SetBool("closing", true);  

    float timeToWait = 0.5f;
    yield return new WaitForSeconds(timeToWait);
    targetScene.SetActive(true);
    parentObject.SetActive(false);
  }

  public IEnumerator ChangeScene(string targetScreen) {
		animator.SetBool("closing", true);
    float timeToWait = 0.5f;
    yield return new WaitForSeconds(timeToWait);

    SceneManager.LoadScene(targetScreen);
  }
}
