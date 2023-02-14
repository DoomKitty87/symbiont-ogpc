using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehavior : MonoBehaviour
{

  [SerializeField] private GameObject missileExplodePrefab;

  private Transform target;
  private Rigidbody rb;
  void Start() {
    target = GameObject.FindGameObjectWithTag("MainCamera").transform;
    rb = GetComponent<Rigidbody>();
    transform.LookAt(target);
  }

  void Update() {
    transform.LookAt(target);
  }

  void FixedUpdate() {
    rb.AddForce(transform.forward * 5);
  }

  void OnCollisionEnter(Collision col) {
    if (!col.gameObject.CompareTag("Player")) return;
    col.gameObject.transform.parent.gameObject.GetComponent<PlayerHealth>().DamagePlayer(5);
    GameObject emitter = Instantiate(missileExplodePrefab, transform.position, Quaternion.identity);
    emitter.GetComponent<ParticleSystem>().Play();
    Destroy(this.gameObject);
  }
}