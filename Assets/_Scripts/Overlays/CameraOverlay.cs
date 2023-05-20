using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOverlay : MonoBehaviour
{

  [SerializeField] private LayerMask _enemyLayer;
  [SerializeField] private float _maxRange, _minScale, _maxScale;
  [SerializeField] private AnimationCurve _scaleCurve;
  [SerializeField] private Camera _cam;
  [SerializeField] private GameObject _overlayPrefab;
  [SerializeField] private Transform _overlayCanvas;

  public void StopOverlay() {
    for (int i = 0; i < _overlayCanvas.childCount; i++) {
      Destroy(_overlayCanvas.GetChild(i).gameObject);
    }
  }

  private void Update() {
    for (int i = 0; i < _overlayCanvas.childCount; i++) {
      Destroy(_overlayCanvas.GetChild(i).gameObject);
    }

    Collider[] cols = Physics.OverlapSphere(transform.position, _maxRange, _enemyLayer);
    foreach (Collider col in cols) {
      Vector3 screenPos = _cam.WorldToScreenPoint(col.gameObject.transform.position);
      if (screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height || screenPos.z < 0) continue;
      RaycastHit hit;
      Physics.Linecast(transform.position, col.gameObject.transform.position, out hit);
      if (hit.collider != col) continue;
      GameObject tmp = Instantiate(_overlayPrefab, Vector3.zero, Quaternion.identity, _overlayCanvas);
      tmp.transform.GetChild(0).position = screenPos;
      tmp.transform.GetChild(0).localScale *= Mathf.Lerp(_maxScale, _minScale, _scaleCurve.Evaluate(Vector3.Distance(transform.position, col.gameObject.transform.position)));
    }
  }
}