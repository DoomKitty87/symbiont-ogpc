using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyInfo : MonoBehaviour
{

  private Camera cam;

  [SerializeField] private LayerMask enemyLayer;
  [SerializeField] private GameObject enemyOverlayPrefab;
  [SerializeField] private GameObject enemyCanvas;

  private void Start() {
    cam = GetComponent<Camera>();
  }

  private string[] GenRobotData() {
    string modelNo = Random.Range(0, 999).ToString();
    while (modelNo.Length < 3) {
      modelNo = "0" + modelNo;
    }
    return new string[] {modelNo};
  }

  public void SwitchedAway() {
    for (int i = 0; i < enemyCanvas.transform.childCount; i++) {
      Destroy(enemyCanvas.transform.GetChild(i).gameObject);
    }
  }

  private void Update() {
    for (int i = 0; i < enemyCanvas.transform.childCount; i++) {
      Destroy(enemyCanvas.transform.GetChild(i).gameObject);
    }
    Collider[] cols = Physics.OverlapSphere(transform.position, 50f, enemyLayer);
    foreach (Collider col in cols) {
      Vector3 screenPos = cam.WorldToScreenPoint(col.gameObject.transform.position);
      if (col.gameObject != GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject) {
        if (screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height || screenPos.z < 0) continue;
        RaycastHit hit;
        Physics.Linecast(transform.position, col.gameObject.transform.position, out hit);
        if (hit.collider != col) continue;
        GameObject tmp = Instantiate(enemyOverlayPrefab, Vector3.zero, Quaternion.identity, enemyCanvas.transform);
        tmp.transform.GetChild(0).position = cam.WorldToScreenPoint(col.gameObject.transform.position + new Vector3(0, GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().bounds.size.y / 2, 0));
        tmp.transform.GetChild(1).position = cam.WorldToScreenPoint(col.gameObject.transform.position - new Vector3(0, GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().bounds.size.y / 2, 0));     

        tmp.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Model" + Random.Range(0, 999);
      }
    }
  }
}