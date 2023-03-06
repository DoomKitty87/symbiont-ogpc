using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class FireGunLogic : MonoBehaviour
{
  // NOTE: This can also be used by Enemy AI scripts to fire at the player.
  // Just call the Fire() function from the EnemyAI script.

  [Header("Raycast Settings")]
  [SerializeField] private Transform _raycastOrigin;
  [SerializeField] private float _raycastDistance;
  [SerializeField] private LayerMask _layerMask;

  [Header("Fire/Hit Settings")]
  [SerializeField] private float _hitDamage;
  [SerializeField] private float _fireDelay;
  [SerializeField] private int _currentAmmo;
  [SerializeField] private int _magSize;
  [SerializeField] private float _reloadTime;
  
  [Header("Debug")]
  [SerializeField] private bool _debug;
  
  [Header("Events")]
  public UnityEvent _OnFire;
  public UnityEvent _OnReloadStart;
  public UnityEvent _OnReloadEnd;

  private float _secondsSinceLastFire = 0;
  private bool _isReloading = false;

  void Start() {
    if (_raycastOrigin == null) {
      Debug.LogError("FireRaycast: FireOrgin is null!");
    }
    if (_raycastDistance <= 0) {
      Debug.LogWarning("FireRaycast: FireDistance is less than or equal to 0. Is this intended?");
    }
    _currentAmmo = _magSize;
  }

  private void Update() {
    _secondsSinceLastFire += Time.deltaTime;
  }

  public void Fire() {
    if (_secondsSinceLastFire < _fireDelay) return;
    if (_currentAmmo <= 0) return;
    _secondsSinceLastFire = 0;
    _currentAmmo -= 1;
    _OnFire?.Invoke();
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

  public void Reload() {
    if (_isReloading) {
      return;
    }
    else {
      StartCoroutine(ReloadCoroutine());
    }
  }
  private IEnumerator ReloadCoroutine() {
    _isReloading = true;
    _OnReloadStart?.Invoke();
    float timeSinceReloadStart = 0f;
    while (timeSinceReloadStart <= _reloadTime) {
      yield return null;
      timeSinceReloadStart += Time.deltaTime;
    }
    _currentAmmo = _magSize; 
    _isReloading = false;
  }

  public void UpdateForNewValues(WeaponItem weaponItem) {
    _hitDamage = weaponItem.shotDamage;
    _fireDelay = weaponItem.fireRate;
    _magSize = weaponItem.magSize;
    _reloadTime = weaponItem.reloadTime;
  }

  private void DrawRaycastLine(Color lineColor, float durationSeconds)
  {
    if (_debug) {
      Debug.DrawLine(_raycastOrigin.position, (_raycastOrigin.forward * _raycastDistance) + _raycastOrigin.position, lineColor, durationSeconds);
    }
  }
}
