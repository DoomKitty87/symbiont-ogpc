using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// I really didn't want to, but the ==='s just make it so much easier to separate the sections of the script
// out mentally for me, and for later reference for whoever decides to edit or change this. 
// This is an annoyingly big script, and I wish I could have split it up into multiple files, but I really really 
// don't want to spend the time to:

// 1: figure out how to split them right now
// 2: deal with the headache of having to make sure the files are in the right place
// 3: deal with all the files referencing each other

// So this is just going to be how it is.

// ------------------------------

// This class handles the actual Raycasting used to determine a hit, and it also contains 
// the logic for shot spread inaccuracy, reloading, and the different fire types.

// UpdateForNewValues can be called with a new WeaponItem to set different weapon values.

// Event Classes
[System.Serializable]
public class FloatUnityEvent : UnityEvent<float> {}
[System.Serializable]
public class IntUnityEvent : UnityEvent<int> {}
[System.Serializable]
public class Vector3UnityEvent : UnityEvent<Vector3> {}

// NOTE: This can also be used by Enemy AI scripts to fire at the player.
// Just call the Fire() function from the EnemyAI script.
public class FireGunLogic : MonoBehaviour
{
  [Header("Weapon Item")]
  [SerializeField] private WeaponItem _weaponItem;

  [Header("Raycast Settings")]
  [SerializeField] private Transform _raycastOrigin;
  [SerializeField] private float _raycastDistance;
  [SerializeField] private LayerMask _layerMask;

  [Header("Fire/Hit Settings")]
  [SerializeField] private WeaponItem.FireType _weaponFireType;
  [SerializeField] private float _currentShotDamage;
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

  // from 0 to 1
  private float _currentCharge; 
  private bool _isCharging = false;
  
  [Header("Debug")]
  [SerializeField] private bool _debug;
  
  [Header("Fire Events")]
  [Tooltip("OnFireAmmo(ammoCount)")] public FloatUnityEvent _OnFireAmmo;
  [Tooltip("OnFireStartEndPos(hitPosition)")] public Vector3UnityEvent _OnFireHitPosition;

  [Header("Broadcast Events")]
  [Tooltip("OnBroadcastShotSpread(currentShotSpread)")] public FloatUnityEvent _OnBroadcastShotSpread;

  [Header("Reload Events")]
  [Tooltip("OnReloadStart(reloadTime)")] public FloatUnityEvent _OnReloadStart;
  [Tooltip("OnReloadEnd(magSize)")] public IntUnityEvent _OnReloadEnd;

  [Header("Charge Events")]
  public UnityEvent _OnChargeStart;
  public UnityEvent _OnChargeEnd;

  private float _secondsSinceLastFire = 0;
  private bool _isReloading = false;

  // ==================================================================================================

  void Start() {
    if (_raycastOrigin == null) {
      Debug.LogError("FireRaycast: FireOrgin is null!");
    }
    if (_raycastDistance <= 0) {
      Debug.LogWarning("FireRaycast: FireDistance is less than or equal to 0. Is this intended?");
    }
    _currentShotDamage = _maxShotDamage;
    _currentAmmo = _magSize;
    _currentShotSpread = _minShotSpread;
  }

  // A little messy looking, but that's the price of having a lot of different fire types.
  public void UpdateForNewValues(WeaponItem weaponItem, int ammoCount) {
    // Weapon Item
    _weaponItem = weaponItem;
    
    // Type
    _weaponFireType = weaponItem.fireType;
    
    // Global Stats
    _maxShotDamage = weaponItem.maxShotDamage;
    _currentShotDamage = _maxShotDamage;
    _fireDelay = weaponItem.fireDelaySeconds;
    _magSize = weaponItem.magSize;
    if (ammoCount < 0 || ammoCount > _magSize) {
      Debug.LogError("FireGunLogic: AmmoCount is out of range. Setting currentAmmo to 0.");
      _currentAmmo = 0;
    }
    else {
      _currentAmmo = ammoCount;
    }

    _reloadTime = weaponItem.reloadTimeSeconds;
    
    // Vertical Recoil is handled by RecoilOffset

    // Shot Spread
    _minShotSpread = weaponItem.minShotSpreadDegrees;
    _maxShotSpread = weaponItem.maxShotSpreadDegrees;
    _currentShotSpread = _minShotSpread;
    // Needed to reset crosshair spread
    _OnBroadcastShotSpread.Invoke(_currentShotSpread);
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
    _OnBroadcastShotSpread.Invoke(_currentShotSpread);
  }

  // ======================== Firing Functions ========================

  // These end up calling Fire() after all the checks and modifiers.

  // When this script recieves OnInputDown, FireSemiAuto should fire, ChargeWeapon should fire, and FireBurst should fire
  // When this script recieves OnInputHeld, FireAuto should fire.
  // When this script recieves OnInputUp, ReleaseCharge should fire.

  // ^ is for reference so an EnemyAI script can call the right manager functions.

  // Again, if you're annoyed by the ---'s and ==='s, refer to line 6.

  public void FireSemiAuto() {
    if (_weaponFireType != WeaponItem.FireType.SemiAuto) return;
    if (!HasAmmo()) {
      Reload();
      return;
    }
    if (!CanFire()) return;
    _secondsSinceLastFire = 0;
    _currentShotDamage = _maxShotDamage;
    Fire();
  }

  // ------------------------

  public void FireBurst() {
    if (_weaponFireType != WeaponItem.FireType.Burst) return;
    if (!HasAmmo()) {
      Reload();
      return;
    }
    if (!CanFire()) return;
    _secondsSinceLastFire = 0;
    StartCoroutine(FireBurstCoroutine());
  }
  private IEnumerator FireBurstCoroutine() {
    _isFiringBurst = true;
    for (int i = 0; i < _shotsPerBurst; i++) {
      if (!HasAmmo()) break;
      _currentShotDamage = _maxShotDamage;
      Fire();
      yield return new WaitForSeconds(_secondsBetweenBurstShots);
    }
    _isFiringBurst = false;
  }

  // ------------------------

  public void FireFullAuto() {
    if (_weaponFireType != WeaponItem.FireType.FullAuto) return;
    if (!HasAmmo()) {
      Reload();
      return;
    }
    if (!CanFire()) return;
    _secondsSinceLastFire = 0;
    _currentShotDamage = _maxShotDamage;
    Fire();
  }

  // ------------------------

  public void ChargeWeapon() {
    if (_weaponFireType != WeaponItem.FireType.Charge) return;
    if (!HasAmmo()) {
      Reload();
      return;
    }
    if (!CanFire()) return;
    if (_isCharging) {
      // Just in case; should never happen
      Debug.LogError("FireGunLogic: ChargeWeapon() invoked before ReleaseCharge() set _isCharging to false! This usually means the coroutine is still running, or that inputs were set up wrong.");
      return;
    } 
    _OnChargeStart?.Invoke();
    StartCoroutine(ChargeWeaponCoroutine());
  }
  private IEnumerator ChargeWeaponCoroutine() {
    _isCharging = true;
    float timeSinceChargeStart = 0;
    while (_isCharging) {
      timeSinceChargeStart += Time.deltaTime;
      _currentCharge = Mathf.Clamp(timeSinceChargeStart / _chargeTime, 0, 1);
      _currentShotDamage = Mathf.Lerp(_minShotDamage, _maxShotDamage, _currentCharge);
      if (_currentCharge == 1) {
        _currentShotDamage = _maxShotDamage;
        ReleaseCharge();
      }
      yield return null;
    }
  }
  public void ReleaseCharge() {
    if (_weaponFireType != WeaponItem.FireType.Charge) return;
    if (!_isCharging) return;
    if (!HasAmmo()) return; // Double check just in case
    _OnChargeEnd?.Invoke();
    _isCharging = false;
    _secondsSinceLastFire = 0;
    StopCoroutine(ChargeWeaponCoroutine());
    Fire();
    _currentCharge = 0;
  }

  // ---------------------------------

  public void Fire() {
    DrawFireLine(Color.green, 1f);
    _OnFireAmmo?.Invoke(_currentAmmo);
    Vector3 raycastDirection = _raycastOrigin.forward + CalculateDirectionWithShotSpread(_minShotDamage);
    if (Physics.Raycast(_raycastOrigin.position, raycastDirection, out RaycastHit hit, _raycastDistance, _layerMask)) {
      //if (hit.collider.gameObject.CompareTag("DoorGraphic")) {
      //  _OnFireHitPosition?.Invoke(hit.point);
      //  if (Physics.Raycast(GameObject.FindGameObjectWithTag("Handler").GetComponent<RoomGenNew>()._currentRoom.GetComponent<RoomHandler>()._instantiatedCamera.transform.position, GameObject.FindGameObjectWithTag("Handler").GetComponent<RoomGenNew>()._currentRoom.GetComponent<RoomHandler>()._instantiatedCamera.transform.forward + raycastDirection, out hit, _raycastDistance, _layerMask)) {
      //    GameObject hitGameObject = hit.collider.gameObject;
      //    HealthManager healthManager = hitGameObject.GetComponent<HealthManager>();
      //    if (healthManager != null) {
      //      healthManager.Damage(_currentShotDamage);
      //      DrawFireLine(Color.yellow, 1f);
      //    }
      //  }
      //}
      //else {
      //Above manages shooting through doors, disabled due to problems and unnecessary
      _OnFireHitPosition?.Invoke(hit.point);
      GameObject hitGameObject = hit.collider.gameObject;
      HealthManager healthManager = hitGameObject.GetComponent<HealthManager>();
      if (healthManager != null) {
        //Disabled on purpose, causing bug with room generation
        healthManager.Damage(_currentShotDamage);
        DrawFireLine(Color.yellow, 1f);
      }
      //}
    }
    else {
      _OnFireHitPosition?.Invoke((raycastDirection * _raycastDistance) + _raycastOrigin.position);
    }
    IncreaseShotSpread();
    _currentAmmo -= 1;
  }

  // ==================== Supporting Functions ====================

  // Needed for Fire method and managers; Obvious by name
  private Vector3 CalculateDirectionWithShotSpread(float shotSpreadDegrees) {
    return new Vector3(Random.Range(-shotSpreadDegrees, shotSpreadDegrees), Random.Range(-_minShotSpread, _minShotSpread));
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
    if (_isReloading) return false;
    if (_isFiringBurst) return false;
    else {
      return true;
    }
  }

  // =================== Reloading ===================

  // This needs to be called by an input script
  public void Reload() {
    if (_isReloading || _currentAmmo == _magSize) {
      return;
    }
    else {
      StartCoroutine(ReloadCoroutine());
    }
  }
  private IEnumerator ReloadCoroutine() {
    _isReloading = true;
    _OnReloadStart?.Invoke(_reloadTime);
    float timeSinceReloadStart = 0f;
    while (timeSinceReloadStart <= _reloadTime) {
      yield return null;
      timeSinceReloadStart += Time.deltaTime;
    }
    _currentAmmo = _magSize;
    _OnReloadEnd?.Invoke(_magSize);
    _isReloading = false;
  }

  // =================== Debug Functions ===================

  private void DrawFireLine(Color lineColor, float durationSeconds)
  {
    if (_debug) {
      Debug.DrawLine(_raycastOrigin.position, (CalculateDirectionWithShotSpread(_minShotSpread) * _raycastDistance) + _raycastOrigin.position, lineColor, durationSeconds);
      Physics.Raycast(_raycastOrigin.position, CalculateDirectionWithShotSpread(_minShotSpread), out RaycastHit hit, _raycastDistance, _layerMask);
      DrawCube(hit.point, 1, Color.blue, durationSeconds);
    }
  }
  private void DrawCube(Vector3 center, float size, Color color, float durationSeconds) {
    // genereated by Github Copilot so i don't have to write it myself
    // thanks github
    Vector3 halfSize = new Vector3(size, size, size) * 0.5f;
    Debug.DrawLine(center + halfSize, center + new Vector3(-halfSize.x, halfSize.y, halfSize.z), color, durationSeconds);
    Debug.DrawLine(center + halfSize, center + new Vector3(halfSize.x, -halfSize.y, halfSize.z), color, durationSeconds);
    Debug.DrawLine(center + halfSize, center + new Vector3(halfSize.x, halfSize.y, -halfSize.z), color, durationSeconds);
    Debug.DrawLine(center - halfSize, center + new Vector3(-halfSize.x, -halfSize.y, -halfSize.z), color, durationSeconds);
    Debug.DrawLine(center - halfSize, center + new Vector3(-halfSize.x, -halfSize.y, halfSize.z), color, durationSeconds);
    Debug.DrawLine(center - halfSize, center + new Vector3(-halfSize.x, halfSize.y, -halfSize.z), color, durationSeconds);
    Debug.DrawLine(center - halfSize, center + new Vector3(halfSize.x, -halfSize.y, -halfSize.z), color, durationSeconds);
    Debug.DrawLine(center - halfSize, center + new Vector3(halfSize.x, -halfSize.y, halfSize.z), color, durationSeconds);
    Debug.DrawLine(center - halfSize, center + new Vector3(halfSize.x, halfSize.y, -halfSize.z), color, durationSeconds);
  }
}