using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelManager : MonoBehaviour
{
  
  [SerializeField] public float moveRate;
    
  private CinemachineVirtualCamera vcam;
  private CinemachineTrackedDolly dolly;
  private PointTracker pointTracker;

  private void Start() {
    vcam = GameObject.FindGameObjectWithTag("VCam").GetComponent<CinemachineVirtualCamera>();
    dolly = vcam.GetCinemachineComponent<CinemachineTrackedDolly>();
    pointTracker = GetComponent<PointTracker>();
  }

  private void Update() {
    dolly.m_PathPosition += 5f * Time.deltaTime * (pointTracker.GetCombo() / 4 + 1);
  }
}
