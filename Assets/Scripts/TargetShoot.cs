using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetShoot : MonoBehaviour
{

  [SerializeField] private GameObject projectilePrefab;
  [SerializeField] private float fireRate;

  private Transform player;

  private float canFireTime = 0f;

  private void Update() {
    canFireTime -= Time.deltaTime;
    if ((player.position - transform.position).magnitude < 30f) TryShoot();
  }

  private void Start() {
    player = GameObject.FindGameObjectWithTag("Player").transform;
  }

  private void TryShoot() {
    if (canFireTime > 0f) return;
    canFireTime = fireRate;
    Instantiate(projectilePrefab, transform.position, Quaternion.identity, transform);
  }
}
