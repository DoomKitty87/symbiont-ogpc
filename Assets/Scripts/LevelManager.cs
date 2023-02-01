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

    void Start()
    {
        vcam = GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;
        dolly = vcam.GetCinemachineComponent<CinemachineTrackedDolly>();
    }

    void Update()
    {
        dolly.m_PathPosition += 0.05f * Time.deltaTime;
    }
}
