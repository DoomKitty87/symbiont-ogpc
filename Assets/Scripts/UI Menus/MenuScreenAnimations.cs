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

  public IEnumerator ChangeMenuScreen(GameObject targetScene) {
    animator.SetBool("closing", true);  

    float timeToWait = 0.3f;
    yield return new WaitForSeconds(timeToWait);
    targetScene.SetActive(true);
    gameObject.SetActive(false);
  }

  public IEnumerator ChangeScene(string targetScreen) {
		animator.SetBool("closing", true);
    float timeToWait = 0.3f;
    yield return new WaitForSeconds(timeToWait);

    SceneManager.LoadScene(targetScreen);
  }
}
