using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRaycast : MonoBehaviour
{
  // TODO: Find a way to configure which event Fire() uses in FireInput.cs
  // EX: Pistols will want to use FireInput.OnFireInputDown because of semi-auto,
  // but Rifles will need OnFireInputHeld.

  // Charge weapons will also need to use OnFireInputHeld and OnFireInput up; this might
  // be a lot more complicated then before.

  [Header("Raycast Settings")]
  [SerializeField] private Transform _raycastOrigin;
  [SerializeField] private float _raycastDistance;
  [SerializeField] private LayerMask _layerMask;

  [Header("Fire/Hit Settings")]
  [SerializeField] private float _hitDamage;
  [SerializeField] private float _fireDelay;
  
  [Header("Debug")]
  [SerializeField] private bool _debug;

  private float _secondsSinceLastFire = 0;
  
  void Start() {
    if (_raycastOrigin == null) {
      Debug.LogError("FireRaycast: FireOrgin is null!");
    }
    if (_raycastDistance <= 0) {
      Debug.LogWarning("FireRaycast: FireDistance is less than or equal to 0. Is this intended?");
    }
  }

  private void Update() {
    _secondsSinceLastFire += Time.deltaTime;
  }

  public void Fire() {
    if (_secondsSinceLastFire < _fireDelay) return;
    _secondsSinceLastFire = 0;

    DrawRaycastLine(Color.green, 1f);
    if (Physics.Raycast(_raycastOrigin.position, _raycastOrigin.forward, out RaycastHit hit, _raycastDistance, _layerMask)) {
      GameObject hitGameObject = hit.collider.gameObject;
      HealthManager healthManager = hitGameObject.GetComponent<HealthManager>();
      if (healthManager != null) {
        healthManager.Damage(_hitDamage);
        DrawRaycastLine(Color.yellow, 1f);
      }
    }
  }

  private void DrawRaycastLine(Color lineColor, float durationSeconds)
  {
    if (_debug) {
      Debug.DrawLine(_raycastOrigin.position, (_raycastOrigin.forward * _raycastDistance) + _raycastOrigin.position, lineColor, durationSeconds);
    }
  }
}