using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetShoot : MonoBehaviour
{

  [SerializeField] private GameObject projectilePrefab;
  [SerializeField] private float fireRate;

  private float canFireTime = 0;

  void Update() {
    canFireTime -= Time.deltaTime;
  }

  void OnTriggerStay(Collider col) {
    if (canFireTime > 0) return;
    canFireTime = fireRate;
    Instantiate(projectilePrefab, transform.position, Quaternion.identity);
  }
}
