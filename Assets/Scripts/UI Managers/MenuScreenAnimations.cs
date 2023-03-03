using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreenAnimations : MonoBehaviour
{

    private Animator animator;
    private GameObject targetScene;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void ChangeScene(string targetScene) {
        targetScene = GameObject.FindWithTag("Handler").GetComponent<ButtonScript>().
    }
}
