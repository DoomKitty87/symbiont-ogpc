using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using System.Linq;

public class PathRenderer : MonoBehaviour
{
  [SerializeField] private GameObject linePrefab;
  [SerializeField] private LayerMask enemyLayer;

  private int startChildren;

  private void Start()
  {
    startChildren = transform.childCount;
    InvokeRepeating("ReloadPaths", 0, 0.5f);
  }

  public void StopOverlay() {
    CancelInvoke("ReloadPaths");
    for (int i = transform.childCount - 1; i >= 0; --i) {
      Destroy(transform.GetChild(i).gameObject);
    }
  }

  public void StartOverlay() {
    pass;
  }

  private void ReloadPaths() {
    for (int i = transform.childCount - 1; i >= 0; --i) {
      Destroy(transform.GetChild(i).gameObject);
    }
    Collider[] cols = Physics.OverlapSphere(transform.position, 50f, enemyLayer);
    foreach (Collider col in cols) {
      GameObject lineRend = Instantiate(linePrefab, col.gameObject.transform.parent.position, Quaternion.identity, transform);
      if (col.gameObject.GetComponent<TargetMovement>()._loop) {
        lineRend.GetComponent<LineRenderer>().positionCount = col.gameObject.transform.parent.gameObject.GetComponent<Waypoints>().points.Length + 1;
        lineRend.GetComponent<LineRenderer>().SetPositions(col.gameObject.transform.parent.gameObject.GetComponent<Waypoints>().points.Concat(new Vector3[] {col.gameObject.transform.parent.gameObject.GetComponent<Waypoints>().points[0]}).ToArray());
      }
      else {
        lineRend.GetComponent<LineRenderer>().positionCount = col.gameObject.transform.parent.gameObject.GetComponent<Waypoints>().points.Length;
        lineRend.GetComponent<LineRenderer>().SetPositions(col.gameObject.transform.parent.gameObject.GetComponent<Waypoints>().points);
      }
    }
  }
}
