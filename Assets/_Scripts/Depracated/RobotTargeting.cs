using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotTargeting : MonoBehaviour
{

  public bool controlled;
  public GameObject playerInhabiting;

  [SerializeField] private float fireRate;
  [SerializeField] private GameObject shotPrefab;

  private bool targeting;
  private bool LOS;
  private float fireCooldown;

  private void Start() {
    playerInhabiting = GameObject.FindGameObjectWithTag("Inhabited");
    InvokeRepeating("TargetingLoop", 0, 0.1f);
  }

  private void Update() {
    fireCooldown -= Time.deltaTime;
  }

  private void TargetingLoop() {
    RaycastHit hit;
    if (Physics.Raycast(transform.position, playerInhabiting.transform.position - transform.position, out hit)) {
      if (hit.collider.gameObject.CompareTag("Inhabited")) {
        if (!targeting) StartCoroutine(TurnToTarget());
      }
    }
    else LOS = false;
  }

  private IEnumerator TurnToTarget() {
    targeting = true;

    while (LOS) {
      transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(playerInhabiting.transform.position - transform.position), 1 * Time.deltaTime);
      if (fireCooldown < 0) {
        Rigidbody rb = Instantiate(shotPrefab, transform.position, transform.rotation, transform).GetComponent<Rigidbody>();
        rb.AddForce((playerInhabiting.transform.position - transform.position).normalized * 10, ForceMode.Impulse);

        fireCooldown = fireRate;
      }
      yield return null;
    }

    targeting = false;
  }
}