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
  [SerializeField] private float _minScale, _maxScale, _maxRange;
  [SerializeField] private AnimationCurve _scaleCurve;

  private string[] _models = {"DXRM", "FNMT", "CRLX", "ZKBL"};
  private List<string[]> _robotData = new List<string[]>();
  private List<GameObject> _robotsSeen = new List<GameObject>();

  private void Start() {
    cam = GetComponent<Camera>();
  }

  private string[] GenRobotData() {
    string modelNo = Random.Range(0, 999).ToString();
    while (modelNo.Length < 3) {
      modelNo = "0" + modelNo;
    }
    string modelType = _models[Random.Range(0, _models.Length)];
    return new string[] {modelNo, modelType};
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
    Collider[] cols = Physics.OverlapSphere(transform.position, _maxRange, enemyLayer);
    foreach (Collider col in cols) {
      Vector3 screenPos = cam.WorldToScreenPoint(col.gameObject.transform.position);
      if (col.gameObject != GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject) {
        if (screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height || screenPos.z < 0) continue;
        RaycastHit hit;
        Physics.Linecast(transform.position, col.gameObject.transform.position, out hit);
        if (hit.collider != col) continue;
        GameObject tmp = Instantiate(enemyOverlayPrefab, Vector3.zero, Quaternion.identity, enemyCanvas.transform);
        tmp.transform.GetChild(0).position = cam.WorldToScreenPoint(col.gameObject.transform.position + new Vector3(0, GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().bounds.size.y / 1.2f, 0));
        tmp.transform.GetChild(1).position = cam.WorldToScreenPoint(col.gameObject.transform.position - new Vector3(0, GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().bounds.size.y / 1.2f, 0));     
        if (!_robotsSeen.Contains(col.gameObject)) {
          _robotsSeen.Add(col.gameObject);
          _robotData.Add(GenRobotData());
        }
        
        tmp.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "MODEL " + _robotData[_robotsSeen.IndexOf(col.gameObject)][1] + "- #" + _robotData[_robotsSeen.IndexOf(col.gameObject)][0];
        tmp.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "STATE: " + "ALERTED";
        tmp.transform.GetChild(0).localScale *= Mathf.Lerp(_maxScale, _minScale, _scaleCurve.Evaluate(Vector3.Distance(transform.position, col.gameObject.transform.position) / _maxRange));
        tmp.transform.GetChild(1).localScale *= Mathf.Lerp(_maxScale, _minScale, _scaleCurve.Evaluate(Vector3.Distance(transform.position, col.gameObject.transform.position) / _maxRange));
        
        tmp.transform.GetChild(2).position = cam.WorldToScreenPoint(col.gameObject.transform.position + new Vector3(0, GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().bounds.size.y / 2f, 0) + ((GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().bounds.size.x / 2) * Vector3.Cross(transform.forward, Vector3.up)));
        tmp.transform.GetChild(2).localScale *= Mathf.Lerp(_maxScale, _minScale, _scaleCurve.Evaluate(Vector3.Distance(transform.position, col.gameObject.transform.position) / _maxRange));
        tmp.transform.GetChild(3).position = cam.WorldToScreenPoint(col.gameObject.transform.position + new Vector3(0, GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().bounds.size.y / 2f, 0) - ((GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().bounds.size.x / 2) * Vector3.Cross(transform.forward, Vector3.up)));
        tmp.transform.GetChild(3).localScale *= Mathf.Lerp(_maxScale, _minScale, _scaleCurve.Evaluate(Vector3.Distance(transform.position, col.gameObject.transform.position) / _maxRange));
        tmp.transform.GetChild(4).position = cam.WorldToScreenPoint(col.gameObject.transform.position - new Vector3(0, GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().bounds.size.y / 2f, 0) + ((GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().bounds.size.x / 2) * Vector3.Cross(transform.forward, Vector3.up)));
        tmp.transform.GetChild(4).localScale *= Mathf.Lerp(_maxScale, _minScale, _scaleCurve.Evaluate(Vector3.Distance(transform.position, col.gameObject.transform.position) / _maxRange));
        tmp.transform.GetChild(5).position = cam.WorldToScreenPoint(col.gameObject.transform.position - new Vector3(0, GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().bounds.size.y / 2f, 0) - ((GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().bounds.size.x / 2) * Vector3.Cross(transform.forward, Vector3.up)));
        tmp.transform.GetChild(5).localScale *= Mathf.Lerp(_maxScale, _minScale, _scaleCurve.Evaluate(Vector3.Distance(transform.position, col.gameObject.transform.position) / _maxRange));
      }
    }
  }
}