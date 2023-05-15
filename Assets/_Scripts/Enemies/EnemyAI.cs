using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
  [Header("Outside References")]
  [SerializeField] private FloorManager _floorManager;
  [SerializeField] private bool _useFloorManagerValues;
  [SerializeField] private ViewSwitcher _viewSwitcher;

  [Header("Prefab References")]
  [SerializeField] private FireGunLogic _fireLogicManager;
  
  [Header("Enemy Raycast Settings")]
  [SerializeField] private LayerMask _enemyLayer;
  [SerializeField] private Transform _raycastOrigin;

  [Header("Movement Settings")]
  [SerializeField] private float _lookSpeed;
  [SerializeField] private Transform _rotateX, _rotateY;
  
  [Header("Vision Field Settings")]
  [SerializeField] private float _fovDirect;
  [SerializeField] private float _fovPeriph;
  
  [Header("Detection Range Settings")]
  [SerializeField] private float _rangeDirect;
  [SerializeField] private float _rangePeriph;
  [SerializeField] private float _rangeNoVision; 
  
  [Header("Notice Chance Settings")]
  [SerializeField] private float _noticeChanceDirect;
  [SerializeField] private float _noticeChancePeriph;
  [SerializeField] private float _noticeChanceNoVision;

  [Header("Debug")]
  [SerializeField] private bool _debug;
  [SerializeField] private bool _showViewRanges;
  public bool _targetingPlayer;
  public float _secondsSinceTargeting;

  private bool _lookingForPlayer;

  private float _fireCooldown;
  private float _targetRot;
  private float _initRot;
  private float _timeElapsed;

  private void Start() {
    if (_fireLogicManager == null) {
      _fireLogicManager = gameObject.GetComponent<FireGunLogic>();
    }
    if (_viewSwitcher == null) {
      _viewSwitcher = GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>();
    }
    if (_floorManager == null && _useFloorManagerValues) {
      _floorManager = GameObject.FindGameObjectWithTag("Persistent").GetComponent<FloorManager>();
    }
    if (_useFloorManagerValues) {
      float[] stats = _floorManager.GetRandEnemyAIStats();
      _rangeDirect = stats[0];
      _rangePeriph = stats[0] * 0.75f;
      _rangeNoVision = stats[0] * 0.5f;

      _fovDirect = stats[1];
      _fovPeriph = stats[1] * 1.2f;

      _noticeChanceDirect = stats[2];
      _noticeChancePeriph = stats[2] * 0.4f;
      _noticeChanceNoVision = stats[2] * 0.05f;

      _lookSpeed = stats[3];
    }
  }

  private void Update() {
    if (_targetingPlayer) {
      _secondsSinceTargeting = 0;
    }
    else {
      _secondsSinceTargeting += Time.deltaTime;
    }

    if (_targetingPlayer) {
      TargetingPlayer();
    }
    else if (_lookingForPlayer) {
      LookingForPlayer();
    }

    CheckForPlayer();
  }

  // Called by what?
  public void StopTracking() {
    _targetingPlayer = false;
    _lookingForPlayer = false;
    _timeElapsed = 0;
  }

  // ==================== Targeting Player ====================

  private void TargetingPlayer() {
    GameObject player = _viewSwitcher._currentObjectInhabiting.gameObject;
    // Calculate the angle between the player and the camera
    Vector3 relativeVector = player.transform.position - _rotateX.transform.position;
    if (_debug) {
      Debug.DrawRay(_rotateX.transform.position, relativeVector, Color.red);
    }

    float radVal = Mathf.Abs(Mathf.Atan2(relativeVector.y, relativeVector.z));

    // If the player is above the camera
    if (relativeVector.z > 0 && relativeVector.y > 0) {
      radVal = Mathf.PI / 2 - radVal;
    } 
    // If the player is to the left of the camera
    else if (relativeVector.z < 0 && relativeVector.y > 0) {
      radVal += Mathf.PI;
    }
    // If the player is below the camera
    else if (relativeVector.z < 0 && relativeVector.y < 0) {
      radVal = (-Mathf.PI / 2) - radVal;
    }
    // If the player is to the right of the camera
    else if (relativeVector.z > 0 && relativeVector.y < 0) {
      radVal = (-Mathf.PI / 2) + radVal;
    }
    // If the player is directly above or below the camera
    else if (relativeVector.z == 0) {
      if (relativeVector.y > 0) radVal = Mathf.PI / 2;
      else radVal = -Mathf.PI / 2;
    }
    // If the player is directly to the left or right of the camera
    else if (relativeVector.y == 0) {
      if (relativeVector.z > 0) radVal = 0;
      else radVal = 0;
    }
    // If the player is in the same position as the camera
    else if (relativeVector.z == 0 && relativeVector.y == 0) radVal = 0;

    //if (radVal == -Mathf.PI) radVal = 0;
    _rotateX.localRotation = Quaternion.Lerp(_rotateX.localRotation, Quaternion.Euler(new Vector3(radVal * Mathf.Rad2Deg, 0, 0)), _timeElapsed * _lookSpeed / 2);
    //Rotation on X axis
    relativeVector = player.transform.position - _rotateY.transform.position;
    relativeVector.y = 0;
    _rotateY.localRotation = Quaternion.Lerp(_rotateY.localRotation, Quaternion.LookRotation(relativeVector), _timeElapsed * _lookSpeed / 2);
    //Rotation on Y axis
    if (Physics.Raycast(transform.position, _raycastOrigin.forward, out RaycastHit hit, _rangeDirect)) {
      if (hit.collider.gameObject == player) Shoot();
    }
    _timeElapsed += Time.deltaTime;
  }

  private void LockOntoPlayer() {
    if (!_targetingPlayer) _timeElapsed = 0;
    _targetingPlayer = true;
    _lookingForPlayer = false;
  }
  
  private void Shoot() {
    //Fire at player
    _fireLogicManager.FireCurrent();
  }

  // ==================== Looking For Player ====================

  private void LookingForPlayer() {
    _rotateY.localRotation = Quaternion.Euler(new Vector3(0, Mathf.Lerp(_initRot, _targetRot, _timeElapsed * _lookSpeed / 2)));
    if (_timeElapsed * _lookSpeed / 2 > 1.5f) {
      _timeElapsed = 0;
      _initRot = _targetRot;
      _targetRot = Random.Range(0, 360);
    }
    _timeElapsed += Time.deltaTime;
  }

  // ==================== Check For Player ====================

  private void CheckForPlayer() {
    //Check first for player in direct vision range, then peripheral, then non-visible.
    Collider[] cols = Physics.OverlapSphere(transform.position, _rangeDirect, _enemyLayer);
    foreach (Collider col in cols) {
      if (col.gameObject == transform.parent.gameObject) continue;
      if (col.gameObject != _viewSwitcher._currentObjectInhabiting.gameObject) continue;
      RaycastHit hit;
      if (Physics.Linecast(transform.position, col.gameObject.transform.position, out hit)) if (hit.collider.gameObject != col.gameObject) continue;
      float angleDiff = Vector3.Angle(_raycastOrigin.forward, transform.position - col.gameObject.transform.position);
      if (angleDiff <= _fovDirect / 2f) {
        //Found in direct range
        if (Random.value < _noticeChanceDirect) LockOntoPlayer();
      }
      else if (angleDiff <= _fovPeriph / 2f && Vector3.Distance(col.gameObject.transform.position, transform.position) < _rangePeriph) {
        //Found in peripheral range
        if (Random.value < _noticeChancePeriph) LockOntoPlayer();
      }
      else {
        //Found in invisible range
        if (Vector3.Distance(col.gameObject.transform.position, transform.position) < _rangeNoVision) {
          if (Random.value < _noticeChanceNoVision) LockOntoPlayer();
        }
        else {
          if (_targetingPlayer && _timeElapsed > 0.1f) {
            if (_targetingPlayer) _timeElapsed = 0;
            _targetingPlayer = false;
            _lookingForPlayer = true;
          }
        }
      }
    }
  }

  private void OnDrawGizmos() {
    if (!_debug) return;
    Gizmos.color = Color.red;
    Gizmos.DrawFrustum(transform.position, _fovDirect, _rangeDirect, 0, 1);
    Gizmos.color = Color.yellow;
    Gizmos.DrawFrustum(transform.position, _fovPeriph, _rangePeriph, 0, 1);
    if (!_showViewRanges) return;
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, _rangeDirect);
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, _rangePeriph);
    Gizmos.color = Color.blue;
    Gizmos.DrawWireSphere(transform.position, _rangeNoVision);
  }
}