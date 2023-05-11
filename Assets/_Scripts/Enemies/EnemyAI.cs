using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

  [SerializeField] private float _fovDirect, _fovPeriph, _rangeDirect, _rangePeriph, _rangeInvis, _noticeChanceDirect, _noticeChancePeriph, _noticeChanceInvis, _lookSpeed;
  [SerializeField] private LayerMask _enemyLayer;
  [SerializeField] private Transform _raycastOrigin;

  [HideInInspector] public bool _targetingPlayer;
  [HideInInspector] public float _secondsSinceTargeting;

  private float _fireCooldown;

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
    CheckForPlayer();
  }

  public void StopTracking() {
    _targetingPlayer = false;
  }

  private void LockOntoPlayer() {
    _targetingPlayer = true;
    StartCoroutine(TargetingPlayer());
  }
  
  private void Shoot() {
    //Fire at player
    GetComponent<FireGunLogic>().FireCurrent();
  }

  private IEnumerator TargetingPlayer() {
    GameObject player = GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject;
    while (_targetingPlayer) {
      transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up);
      //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.position - player.transform.position), _lookSpeed * Time.deltaTime);
      if (Physics.Raycast(transform.position, _raycastOrigin.forward, _rangeInvis, _enemyLayer)) {
        Shoot();
      }
      yield return null;
    }
  }

  private void CheckForPlayer() {
    //Check first for player in direct vision range, then peripheral, then non-visible.
    Collider[] cols = Physics.OverlapSphere(transform.position, _rangeDirect, _enemyLayer);
    foreach (Collider col in cols) {
      if (col.gameObject == transform.parent.gameObject) continue;
      if (col.gameObject != GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject) continue;
      float angleDiff = Vector3.Angle(_raycastOrigin.forward, transform.position - col.gameObject.transform.position);
      if (angleDiff <= _fovDirect / 2f) {
        //Found in direct range
        if (Random.value < _noticeChanceDirect) {
          LockOntoPlayer();
        }
      }
      else if (angleDiff <= _fovPeriph / 2f && Vector3.Distance(col.gameObject.transform.position, transform.position) < _rangePeriph) {
        //Found in peripheral range
        if (Random.value < _noticeChancePeriph) {
          LockOntoPlayer();
        }
      }
      else {
        //Found in invisible range
        if (Random.value < _noticeChanceInvis && Vector3.Distance(col.gameObject.transform.position, transform.position) < _rangeInvis) {
          LockOntoPlayer();
        }
        else {
          _targetingPlayer = false;
        }
      }
    }
  }
}