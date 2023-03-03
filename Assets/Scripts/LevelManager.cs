using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelManager : MonoBehaviour
{ 
  [Header("This (without assignment) requires a GameObject with" + "\n" + "tag 'Handler' containing ScoreTracker")]
  [SerializeField] private ScoreTracker _scoreTracker;
  [SerializeField] public float moveRate;
  
  private CinemachineVirtualCamera vcam;
  private CinemachineTrackedDolly dolly;

  

  private void Start() {
    if (_scoreTracker == null) {
      _scoreTracker = GameObject.FindGameObjectWithTag("Handler").GetComponent<ScoreTracker>();
    }
    vcam = GameObject.FindGameObjectWithTag("VCam").GetComponent<CinemachineVirtualCamera>();
    dolly = vcam.GetCinemachineComponent<CinemachineTrackedDolly>();
  }

  private void Update() {
    dolly.m_PathPosition += 5f * Time.deltaTime * (_scoreTracker.GetCombo() / 4 + 1);
  }
}
