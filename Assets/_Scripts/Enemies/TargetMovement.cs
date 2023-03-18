using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class TargetMovement : MonoBehaviour
{

  public float intelligence;

  private Transform player;
  private float noticePlayerRange;
  private float runSpeed;
  private float xSize;
  private bool running;
  private bool donerunning;

  void Start() {
    player = GameObject.FindGameObjectWithTag("Player").transform;
    noticePlayerRange = Random.Range(intelligence * 4, intelligence * 6);
    runSpeed = Random.Range(intelligence / 4.5f, intelligence / 5.5f);
    xSize = GetComponent<Renderer>().bounds.size.x;
  }

  void Update() {
    if (Mathf.Abs(player.position.magnitude - transform.position.magnitude) < noticePlayerRange && !running && !donerunning) {
      StartCoroutine(StartRunning());
    }
  }

  private IEnumerator StartRunning() {
    running = true;
    while (player.position.magnitude - transform.position.magnitude < noticePlayerRange * 1.5f && !donerunning) {
      Vector3 initPos = transform.position;
      Vector3 targetPos = transform.position - (new Vector3(player.position.x - transform.position.x, -1 * (player.position.y - transform.position.y), player.position.z - transform.position.z)) / 4;
      float timer = 0;
      while (timer < 1) {
        transform.position = Vector3.Lerp(initPos, targetPos, timer);
        if (Physics.OverlapBox(transform.position, new Vector3(xSize / 1.5f, xSize / 1.5f, xSize / 1.5f)).Length > 1) {
          donerunning = true;
          break;
        }
        timer += Time.deltaTime * runSpeed;
        yield return null;
      }
    }
    yield return null;
    running = false;
  }
}
*/

public class TargetMovement : MonoBehaviour{

  [SerializeField] private float health;
  [SerializeField] private float defense;
  [SerializeField] private float movementSpeed;

}