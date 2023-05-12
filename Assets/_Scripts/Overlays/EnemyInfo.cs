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
  private FloorManager _floorManager;

  private void Start() {
    cam = GetComponent<Camera>();
    _floorManager = GameObject.FindGameObjectWithTag("Persistent").GetComponent<FloorManager>();
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
        GameObject tmp;
        if (gameObject.CompareTag("DoorCamera")) {
          //RaycastHit hit2;
          //Physics.Linecast(GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject.transform.position, GetComponent<DoorCameraFollow>().otherDoor.GetChild(0).position, out hit2);
          //if (hit2.collider.gameObject.name != "DoorGraphic") continue;
          if (screenPos.x > cam.WorldToScreenPoint(GetComponent<DoorCameraFollow>().door.position + GetComponent<DoorCameraFollow>().door.forward * (GetComponent<DoorCameraFollow>().door.GetChild(1).gameObject.GetComponent<Renderer>().bounds.size.x / 2)).x) continue;
          if (screenPos.x < cam.WorldToScreenPoint(GetComponent<DoorCameraFollow>().door.position - GetComponent<DoorCameraFollow>().door.forward * (GetComponent<DoorCameraFollow>().door.GetChild(1).gameObject.GetComponent<Renderer>().bounds.size.x / 2)).x) continue;
          if (screenPos.y > cam.WorldToScreenPoint(GetComponent<DoorCameraFollow>().door.position + new Vector3(0, GetComponent<DoorCameraFollow>().door.GetChild(1).gameObject.GetComponent<Renderer>().bounds.size.y / 2, 0)).y) continue;
          if (screenPos.y < cam.WorldToScreenPoint(GetComponent<DoorCameraFollow>().door.position - new Vector3(0, GetComponent<DoorCameraFollow>().door.GetChild(1).gameObject.GetComponent<Renderer>().bounds.size.y / 2, 0)).y) continue;
          tmp = Instantiate(enemyOverlayPrefab, Vector3.zero, Quaternion.identity, GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting._objectCameras[0].GetComponent<EnemyInfo>().enemyCanvas.transform);
        }
        else {
          tmp = Instantiate(enemyOverlayPrefab, Vector3.zero, Quaternion.identity, enemyCanvas.transform);
        }
        tmp.transform.GetChild(0).position = cam.WorldToScreenPoint(col.gameObject.transform.position + new Vector3(0, GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject.transform.GetChild(0).gameObject.GetComponent<ReferenceMainTorso>()._torso.GetComponent<Renderer>().bounds.size.y / 1.2f, 0));
        tmp.transform.GetChild(1).position = cam.WorldToScreenPoint(col.gameObject.transform.position - new Vector3(0, GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject.transform.GetChild(0).gameObject.GetComponent<ReferenceMainTorso>()._torso.GetComponent<Renderer>().bounds.size.y / 1.2f, 0));     
        if (!_floorManager._robotsSeen.Contains(col.gameObject)) {
          _floorManager._robotsSeen.Add(col.gameObject);
          _floorManager._robotData.Add(GenRobotData());
        }

        tmp.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "MODEL " + _floorManager._robotData[_floorManager._robotsSeen.IndexOf(col.gameObject)][1] + "- #" + _floorManager._robotData[_floorManager._robotsSeen.IndexOf(col.gameObject)][0];
        if (col.gameObject.transform.GetChild(1).gameObject.GetComponent<EnemyAI>()._targetingPlayer) {
          tmp.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "STATE: <color=#FF0000>ALERTED</color>";
        }
        else {
          tmp.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "STATE: <color=#00FF00>PASSIVE</color>";
        }
        tmp.transform.GetChild(0).localScale *= Mathf.Lerp(_maxScale, _minScale, _scaleCurve.Evaluate(Vector3.Distance(transform.position, col.gameObject.transform.position) / _maxRange));
        tmp.transform.GetChild(1).localScale *= Mathf.Lerp(_maxScale, _minScale, _scaleCurve.Evaluate(Vector3.Distance(transform.position, col.gameObject.transform.position) / _maxRange));
        
        tmp.transform.GetChild(2).position = cam.WorldToScreenPoint(col.gameObject.transform.position + new Vector3(0, GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject.transform.GetChild(0).gameObject.GetComponent<ReferenceMainTorso>()._torso.GetComponent<Renderer>().bounds.size.y / 2f, 0) + ((GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject.transform.GetChild(0).gameObject.GetComponent<ReferenceMainTorso>()._torso.GetComponent<Renderer>().bounds.size.x / 2) * Vector3.Cross(transform.forward, Vector3.up)));
        tmp.transform.GetChild(2).localScale *= Mathf.Lerp(_maxScale, _minScale, _scaleCurve.Evaluate(Vector3.Distance(transform.position, col.gameObject.transform.position) / _maxRange));
        tmp.transform.GetChild(3).position = cam.WorldToScreenPoint(col.gameObject.transform.position + new Vector3(0, GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject.transform.GetChild(0).gameObject.GetComponent<ReferenceMainTorso>()._torso.GetComponent<Renderer>().bounds.size.y / 2f, 0) - ((GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject.transform.GetChild(0).gameObject.GetComponent<ReferenceMainTorso>()._torso.GetComponent<Renderer>().bounds.size.x / 2) * Vector3.Cross(transform.forward, Vector3.up)));
        tmp.transform.GetChild(3).localScale *= Mathf.Lerp(_maxScale, _minScale, _scaleCurve.Evaluate(Vector3.Distance(transform.position, col.gameObject.transform.position) / _maxRange));
        tmp.transform.GetChild(4).position = cam.WorldToScreenPoint(col.gameObject.transform.position - new Vector3(0, GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject.transform.GetChild(0).gameObject.GetComponent<ReferenceMainTorso>()._torso.GetComponent<Renderer>().bounds.size.y / 2f, 0) + ((GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject.transform.GetChild(0).gameObject.GetComponent<ReferenceMainTorso>()._torso.GetComponent<Renderer>().bounds.size.x / 2) * Vector3.Cross(transform.forward, Vector3.up)));
        tmp.transform.GetChild(4).localScale *= Mathf.Lerp(_maxScale, _minScale, _scaleCurve.Evaluate(Vector3.Distance(transform.position, col.gameObject.transform.position) / _maxRange));
        tmp.transform.GetChild(5).position = cam.WorldToScreenPoint(col.gameObject.transform.position - new Vector3(0, GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject.transform.GetChild(0).gameObject.GetComponent<ReferenceMainTorso>()._torso.GetComponent<Renderer>().bounds.size.y / 2f, 0) - ((GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting.gameObject.transform.GetChild(0).gameObject.GetComponent<ReferenceMainTorso>()._torso.GetComponent<Renderer>().bounds.size.x / 2) * Vector3.Cross(transform.forward, Vector3.up)));
        tmp.transform.GetChild(5).localScale *= Mathf.Lerp(_maxScale, _minScale, _scaleCurve.Evaluate(Vector3.Distance(transform.position, col.gameObject.transform.position) / _maxRange));
      }
    }
  }
}