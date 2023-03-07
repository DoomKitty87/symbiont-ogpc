using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// This class handles the actual Raycasting used to determine a hit
// It also contains the logic for shot spread inaccuracy, reloading, and different fire types
// It also has the function UpdateForNewValues, which can be called with a new WeaponItem to set the
// values.

public class FireGunLogic : MonoBehaviour
{
  // NOTE: This can also be used by Enemy AI scripts to fire at the player.
  // Just call the Fire() function from the EnemyAI script.

  [Header("Raycast Settings")]
  [SerializeField] private Transform _raycastOrigin;
  [SerializeField] private float _raycastDistance;
  [SerializeField] private LayerMask _layerMask;

  [Header("Fire/Hit Settings")]
  [SerializeField] private WeaponItem.FireType _weaponFireType;
  [SerializeField] private float _maxShotDamage;
  [SerializeField] private float _fireDelay;

  [Header("Shot Spread Settings")]
  [SerializeField] private float _currentShotSpread;
  [SerializeField] private float _minShotSpread;
  [SerializeField] private float _maxShotSpread;
  [SerializeField] private float _spreadIncreasePerShot;
  [SerializeField] private float _shotSpreadRecovery;
  
  [Header("Ammo Settings")]
  [SerializeField] private int _currentAmmo;
  [SerializeField] private int _magSize;
  [SerializeField] private float _reloadTime;
 
  [Header("Burst Settings")]
  [SerializeField] private int _shotsPerBurst;
  [SerializeField] private float _secondsBetweenBurstShots;
  private bool _isFiringBurst = false;

  [Header("Charge Settings")]
  [SerializeField] private float _minShotDamage;
  [SerializeField] private float _chargeTime;
  private float _currentCharge;
  private float _timeSinceChargeStart;
  private bool _isCharging = false;
  
  [Header("Debug")]
  [SerializeField] private bool _debug;
  
  [Header("Events")]
  public UnityEvent _OnFire;
  public UnityEvent _OnReloadStart;
  public UnityEvent _OnReloadEnd;
  [Header("Charge Events")]
  public UnityEvent _OnChargeStart;
  public UnityEvent _OnChargeEnd;

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
    _currentShotSpread = _minShotSpread;
  }

  public void UpdateForNewValues(WeaponItem weaponItem) {
    // Type
    _weaponFireType = weaponItem.fireType;
    
    // Global Stats
    _maxShotDamage = weaponItem.maxShotDamage;
    _fireDelay = weaponItem.fireDelaySeconds;
    _magSize = weaponItem.magSize;
    _currentAmmo = _magSize;
    _reloadTime = weaponItem.reloadTimeSeconds;
    
    // Vertical Recoil is handled by RecoilOffset

    // Shot Spread
    _minShotSpread = weaponItem.minShotSpreadDegrees;
    _maxShotSpread = weaponItem.maxShotSpreadDegrees;
    _currentShotSpread = _minShotSpread;
    _spreadIncreasePerShot = weaponItem.shotSpreadFireInaccuracyDegrees;
    _shotSpreadRecovery = weaponItem.shotSpreadRecovery;

    // Burst Stats
    _shotsPerBurst = weaponItem.shotsPerBurst;
    _secondsBetweenBurstShots = weaponItem.secondsBetweenBurstShots;
    
    // Charge Stats
    _minShotDamage = weaponItem.minShotDamage;
    _chargeTime = weaponItem.chargeTimeSeconds;
  }

  private void Update() {
    _secondsSinceLastFire += Time.deltaTime;
    _currentShotSpread = Mathf.Clamp(_currentShotSpread -= _shotSpreadRecovery * Time.deltaTime, _minShotSpread, _maxShotSpread);
  }

  // When this script recieves OnInputDown, FireSemiAuto should fire, ChargeWeapon should fire, and FireBurst should fire
  // When this script recieves OnInputHeld, FireAuto should fire.
  // When this script recieves OnInputUp, ReleaseCharge should fire.

  // ^ is so an EnemyAI script can call the right manager functions

  // Firing Manager Methods
  public void FireSemiAuto() {
    if (_weaponFireType != WeaponItem.FireType.SemiAuto) return;
    if (!(HasAmmo() && CanFire())) return;
    _secondsSinceLastFire = 0;
    Fire();
  }
  public void FireBurst() {
    if (_weaponFireType != WeaponItem.FireType.Burst) return;
    if (!(HasAmmo() && CanFire())) return;
    _secondsSinceLastFire = 0;
    StartCoroutine(FireBurstCoroutine());
  }
  private IEnumerator FireBurstCoroutine() {
    _isFiringBurst = true;
    for (int i = 0; i < _shotsPerBurst; i++) {
      if (!HasAmmo()) break;
      Fire();
      yield return new WaitForSeconds(_secondsBetweenBurstShots);
    }
    _isFiringBurst = false;
  }
  public void FireFullAuto() {
    if (_weaponFireType != WeaponItem.FireType.FullAuto) return;
    if (!(HasAmmo() && CanFire())) return;
    _secondsSinceLastFire = 0;
    Fire();
  }
  public void ChargeWeapon() {
    if (_weaponFireType != WeaponItem.FireType.Charge) return;
    if (!(HasAmmo() && CanFire())) return;
  }
  public void ReleaseCharge() {
    if (_weaponFireType != WeaponItem.FireType.Charge) return;
    _secondsSinceLastFire = 0;
  }
  public void Fire() {
    _OnFire?.Invoke();
    DrawFireLine(Color.green, 1f);
    if (Physics.Raycast(_raycastOrigin.position, CalculateDirectionWithShotSpread(_minShotSpread), out RaycastHit hit, _raycastDistance, _layerMask)) {
      GameObject hitGameObject = hit.collider.gameObject;
      HealthManager healthManager = hitGameObject.GetComponent<HealthManager>();
      if (healthManager != null) {
        healthManager.Damage(_maxShotDamage);
        DrawFireLine(Color.yellow, 1f);
      }
    }
    IncreaseShotSpread();
    _currentAmmo -= 1;
  }

  // Needed for Fire method; Obvious by name
  private Vector3 CalculateDirectionWithShotSpread(float shotSpreadDegrees) {
    return _raycastOrigin.forward + new Vector3(Random.Range(-shotSpreadDegrees, shotSpreadDegrees), Random.Range(-_minShotSpread, _minShotSpread));
  }
  private void IncreaseShotSpread() {
    _currentShotSpread += _spreadIncreasePerShot;
    if (_currentShotSpread > _maxShotSpread) {
      _currentShotSpread = _maxShotSpread;
    }
  }

  private bool HasAmmo() {
    if (_currentAmmo <= 0) return false;
    else {
      return true;
    }
  }
  private bool CanFire() {
    if (_secondsSinceLastFire < _fireDelay) return false;
    if (_isFiringBurst) return false;
    else {
      return true;
    }
  }

  // This is called by the FireInput script
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
    _OnReloadEnd?.Invoke();
    _isReloading = false;
  }

  // Debug functions, used to draw fire lines in the editor
  private void DrawFireLine(Color lineColor, float durationSeconds)
  {
    if (_debug) {
      Debug.DrawLine(_raycastOrigin.position, (CalculateDirectionWithShotSpread(_minShotSpread) * _raycastDistance) + _raycastOrigin.position, lineColor, durationSeconds);
      Physics.Raycast(_raycastOrigin.position, CalculateDirectionWithShotSpread(_minShotSpread), out RaycastHit hit, _raycastDistance, _layerMask);
      DrawSquare(hit.point, 0.1f, Color.blue, durationSeconds);
    }
  }
  private void DrawSquare(Vector3 center, float size, Color color, float durationSeconds) {
    Vector3 topLeft = center + new Vector3(-size, 0, size);
    Vector3 topRight = center + new Vector3(size, 0, size);
    Vector3 bottomLeft = center + new Vector3(-size, 0, -size);
    Vector3 bottomRight = center + new Vector3(size, 0, -size);
    Debug.DrawLine(topLeft, topRight, color, durationSeconds);
    Debug.DrawLine(topRight, bottomRight, color, durationSeconds);
    Debug.DrawLine(bottomRight, bottomLeft, color, durationSeconds);
    Debug.DrawLine(bottomLeft, topLeft, color, durationSeconds);
  }
}
