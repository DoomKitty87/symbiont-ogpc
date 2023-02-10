using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelManager : MonoBehaviour
{
    
    [SerializeField]
    public float moveRate;
    
    private CinemachineVirtualCamera vcam;
    private CinemachineTrackedDolly dolly;
    private PointTracker pointtracker;

    void Start()
    {
        vcam = GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;
        dolly = vcam.GetCinemachineComponent<CinemachineTrackedDolly>();
        pointtracker = GetComponent<PointTracker>();
    }

    void Update()
    {
        dolly.m_PathPosition += 5f * Time.deltaTime * (pointtracker.GetCombo() + 1);
    }
}
