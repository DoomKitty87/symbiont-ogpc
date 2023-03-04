using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScreenAnimations : MonoBehaviour
{

    private Animator animator;
    private GameObject targetScene;

    [SerializeField] GameObject parentObject;

    private void Awake() {
        animator = parentObject.GetComponent<Animator>();
    }

    private void OnEnable() {
        animator.SetBool("open", true);
    }

    public IEnumerator ChangeMenuScreen(GameObject targetScene) {
        animator.SetBool("close", true);

        float timeToWait = 0.5f;
        yield return new WaitForSeconds(timeToWait);
        targetScene.SetActive(true);
        gameObject.SetActive(false);
    }

    public IEnumerator ChangeScreen(string targetScreen) {
        animator.SetBool("close", true);
        float timeToWait = 0.5f;
        yield return new WaitForSeconds(timeToWait);

        SceneManager.LoadScene("targetScreen");
    }
}
