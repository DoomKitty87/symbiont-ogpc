using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRaycast : MonoBehaviour
{
  [Header("Raycast Settings")]
  [SerializeField] private Transform _fireOrigin;
  [SerializeField] private float _fireDistance;
  [SerializeField] private LayerMask _layerMask;

  [Header("Hit Settings")]
  [SerializeField] private float _hitDamage;
  
  [Header("Debug")]
  [SerializeField] private bool _debug;
  
  void Start() {
    if (_fireOrigin == null) {
      Debug.LogError("FireRaycast: FireOrgin is null!");
    }
    if (_fireDistance <= 0) {
      Debug.LogWarning("FireRaycast: FireDistance is less than or equal to 0. Is this intended?");
    }
  }

  public void Fire() {
    DrawRaycastLine(Color.green, 1f);
    if (Physics.Raycast(_fireOrigin.position, _fireOrigin.forward, out RaycastHit hit, _fireDistance, _layerMask)) {
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
      Debug.DrawLine(_fireOrigin.position, (_fireOrigin.forward * _fireDistance) + _fireOrigin.position, lineColor, durationSeconds);
    }
  }
}
