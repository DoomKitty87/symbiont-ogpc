using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovement : MonoBehaviour
{

    public float intelligence;

    private Transform player;
    private float noticePlayerRange;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        noticePlayerRange = Random.Range(intelligence * 4, intelligence * 6);
    }

    void Update() {
        if (Mathf.Abs(player.position.magnitude - transform.position.magnitude) < noticePlayerRange) {
            StartCoroutine(StartRunning());
        }
    }

    private IEnumerator StartRunning() {
        yield return null;
    }
}
