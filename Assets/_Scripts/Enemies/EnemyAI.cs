using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : Monobehaviour
{

  [SerializeField] private float _fovDirect, _fovPeriph, _rangeDirect, _rangePeriph, _rangeInvis, _noticeChanceDirect, _noticeChancePeriph, _noticeChanceInvis;
  [SerializeField] private LayerMask _enemyLayer;

  private void Start() {

  }

  private void Update() {
    CheckForPlayer();
  }

  private void CheckForPlayer() {
    //Check first for player in direct vision range, then peripheral, then non-visible.
    Collider[] cols = Physics.OverlapSphere(transform.position, _rangeDirect, _enemyLayer);
    foreach (Collider col in cols) {
      if (col.gameObject != GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject) continue;
      float angleDiff = Vector3.Angle(transform.forward, transform.position - col.gameObject.transform.position);
      if (angleDiff <= _fovDirect / 2f) {
        //Found in direct range
        if (Random.random() < _noticeChanceDirect) {
          LockOntoPlayer();
        }
      }
      else if (angleDiff <= _fovPeriph / 2f) {
        //Found in peripheral range
        if (Random.random() < _noticeChancePeriph) {
          LockOntoPlayer();
        }
      }
      else {
        //Found in invisible range
        if (Random.random() < _noticeChanceInvis) {
          LockOntoPlayer();
        }
      }
    }
  }
}