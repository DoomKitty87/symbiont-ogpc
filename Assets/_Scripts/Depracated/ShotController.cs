using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{

  [SerializeField] private GameObject shotHitParticles;

  private void OnCollisionEnter() {
    Instantiate(shotHitParticles, transform.position, Quaternion.identity);
    Destroy(gameObject);
  }
}