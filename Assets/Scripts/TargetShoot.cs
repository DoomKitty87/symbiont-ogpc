using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetShoot : MonoBehaviour
{

  [SerializeField] private GameObject projectilePrefab;
  [SerializeField] private float fireRate;

  private float canFireTime = 0f;

  private void Update() {
    canFireTime -= Time.deltaTime;
  }

  private void OnTriggerStay(Collider col) {
    print("found object in trigger field");
    if (canFireTime > 0f) return;
    canFireTime = fireRate;
    print("attempting to instantiate");
    Instantiate(projectilePrefab, transform.position, Quaternion.identity);
  }
}
