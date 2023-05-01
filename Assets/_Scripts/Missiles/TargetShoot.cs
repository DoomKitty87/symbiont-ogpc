using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetShoot : MonoBehaviour
{

  [SerializeField] private GameObject projectilePrefab;
  [SerializeField] private float fireRate;

  private Transform playerTransform;

  private float canFireTime = 0f;
  private void Start() {
    playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
  }

  private void Update() {
    canFireTime -= Time.deltaTime;
    if ((playerTransform.position - transform.position).magnitude < 30f)
    {
      TryShoot();
    }
  }

  private void TryShoot() {
    if (canFireTime > 0f)
    {
      return;
    }
    canFireTime = fireRate;
    Instantiate(projectilePrefab, transform.position, Quaternion.identity, transform);
  }
}
