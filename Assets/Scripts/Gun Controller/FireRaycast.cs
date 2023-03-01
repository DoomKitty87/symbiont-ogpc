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
  // Start is called before the first frame update
  void Start() {
    if (_fireOrigin == null) {
      Debug.LogError("FireRaycast: FireOrgin is null!");
    }
    if (_fireDistance <= 0) {
      Debug.LogWarning("FireRaycast: FireDistanve is less than or equal to 0. Is this intended?");
    }
  }

  public Vector3 Fire() {
    if (Physics.Raycast(_fireOrigin.position, _fireOrigin.forward, out RaycastHit hit, _fireDistance, _layerMask)) {
      GameObject hitGameObject = hit.collider.gameObject;
      HealthManager healthManager = hitGameObject.GetComponent<HealthManager>();
      if (healthManager != null) {
        healthManager.Damage(_hitDamage);
      }
      return hit.point;
    }
    else {
        return (_fireOrigin.forward * _fireDistance) + _fireOrigin.position;
    }
  }
}
