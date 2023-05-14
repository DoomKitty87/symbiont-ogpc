using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

  [SerializeField] private float _fovDirect, _fovPeriph, _rangeDirect, _rangePeriph, _rangeInvis, _noticeChanceDirect, _noticeChancePeriph, _noticeChanceInvis, _lookSpeed;
  [SerializeField] private LayerMask _enemyLayer;
  [SerializeField] private Transform _raycastOrigin;
  [SerializeField] private Transform _rotateX, _rotateY;

  [Header("Debug")]
  [SerializeField] private bool _debug;
  
  [HideInInspector] public bool _targetingPlayer;
  [HideInInspector] public float _secondsSinceTargeting;

  private bool _lookingForPlayer;

  private float _fireCooldown;
  private float _targetRot;
  private float _initRot;
  private float _timeElapsed;

  private void Start() {
    float[] stats = GameObject.FindGameObjectWithTag("Persistent").GetComponent<FloorManager>().GetRandEnemyAIStats();
    _rangeDirect = stats[0];
    _rangePeriph = stats[0] * 0.75f;
    _rangeInvis = stats[0] * 0.5f;

    _fovDirect = stats[1];
    _fovPeriph = stats[1] * 1.2f;

    _noticeChanceDirect = stats[2];
    _noticeChancePeriph = stats[2] * 0.4f;
    _noticeChanceInvis = stats[2] * 0.05f;

    _lookSpeed = stats[3];
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

  public void StopTracking() {
    _targetingPlayer = false;
    _lookingForPlayer = false;
    _timeElapsed = 0;
  }

  private void LockOntoPlayer() {
    if (!_targetingPlayer) _timeElapsed = 0;
    _targetingPlayer = true;
    _lookingForPlayer = false;
  }
  
  private void Shoot() {
    //Fire at player
    GetComponent<FireGunLogic>().FireCurrent();
  }

  private void TargetingPlayer() {
    GameObject player = GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject;
    Vector3 rel = player.transform.position - _rotateX.transform.position;
    float radVal = Mathf.Abs(Mathf.Atan2(rel.y, rel.z));
    if (rel.z > 0 && rel.y > 0) {
      radVal = Mathf.PI / 2 - radVal;
    } 
    else if (rel.z < 0 && rel.y > 0) {
      radVal += Mathf.PI;
    }
    else if (rel.z < 0 && rel.y < 0) {
      radVal = (-Mathf.PI / 2) - radVal;
    }
    else if (rel.z > 0 && rel.y < 0) {
      radVal = (-Mathf.PI / 2) + radVal;
    }
    else if (rel.z == 0) {
      if (rel.y > 0) radVal = Mathf.PI / 2;
      else radVal = -Mathf.PI / 2;
    }
    else if (rel.y == 0) {
      if (rel.z > 0) radVal = 0;
      else radVal = 0;
    }
    else if (rel.z == 0 && rel.y == 0) radVal = 0;

    //if (radVal == -Mathf.PI) radVal = 0;
    _rotateX.localRotation = Quaternion.Lerp(_rotateX.localRotation, Quaternion.Euler(new Vector3(radVal * Mathf.Rad2Deg, 0, 0)), _timeElapsed * _lookSpeed / 2);
    //Rotation on X axis
    rel = player.transform.position - _rotateY.transform.position;
    rel.y = 0;
    _rotateY.localRotation = Quaternion.Lerp(_rotateY.localRotation, Quaternion.LookRotation(rel), _timeElapsed * _lookSpeed / 2);
    //Rotation on Y axis
    RaycastHit hit;
    if (Physics.Raycast(transform.position, _raycastOrigin.forward, out hit, _rangeDirect)) {
      if (hit.collider.gameObject == player) Shoot();
    }
    _timeElapsed += Time.deltaTime;
  }

  private void LookingForPlayer() {
    _rotateY.localRotation = Quaternion.Euler(new Vector3(0, Mathf.Lerp(_initRot, _targetRot, _timeElapsed * _lookSpeed / 2)));
    if (_timeElapsed * _lookSpeed / 2 > 1.5f) {
      _timeElapsed = 0;
      _initRot = _targetRot;
      _targetRot = Random.Range(0, 360);
    }
    _timeElapsed += Time.deltaTime;
  }

  private void CheckForPlayer() {
    //Check first for player in direct vision range, then peripheral, then non-visible.
    Collider[] cols = Physics.OverlapSphere(transform.position, _rangeDirect, _enemyLayer);
    foreach (Collider col in cols) {
      if (col.gameObject == transform.parent.gameObject) continue;
      if (col.gameObject != GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject) continue;
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
        if (Vector3.Distance(col.gameObject.transform.position, transform.position) < _rangeInvis) {
          if (Random.value < _noticeChanceInvis) LockOntoPlayer();
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
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, _rangeDirect);
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, _rangePeriph);
    Gizmos.color = Color.blue;
    Gizmos.DrawWireSphere(transform.position, _rangeInvis);
    Gizmos.color = Color.blue;
    Gizmos.DrawRay(_raycastOrigin.position, _raycastOrigin.forward * _rangeDirect);
    Gizmos.DrawRay(_raycastOrigin.position, Quaternion.Euler(0, _fovDirect / 2f, 0) * _raycastOrigin.forward * _rangeDirect);
    Gizmos.DrawRay(_raycastOrigin.position, Quaternion.Euler(0, -_fovDirect / 2f, 0) * _raycastOrigin.forward * _rangeDirect);
    Gizmos.DrawRay(_raycastOrigin.position, Quaternion.Euler(0, _fovPeriph / 2f, 0) * _raycastOrigin.forward * _rangePeriph);
    Gizmos.DrawRay(_raycastOrigin.position, Quaternion.Euler(0, -_fovPeriph / 2f, 0) * _raycastOrigin.forward * _rangePeriph);
  }
}