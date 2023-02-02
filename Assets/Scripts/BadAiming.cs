using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BadAiming : MonoBehaviour
{
    private CinemachineVirtualCamera vcam;
    private CinemachinePOV pov;

    void Start()
    {
      vcam = GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;
      pov = vcam.GetCinemachineComponent<CinemachinePOV>();
    }

    void Update()
    {
      if (Input.GetKey(KeyCode.LeftArrow)) {
        pov.m_HorizontalAxis.Value -= 20 * Time.deltaTime;
      }
      else {
        pov.m_HorizontalAxis.Value += 20 * Time.deltaTime;
      }
      if (Input.GetKey(KeyCode.UpArrow)) {
        pov.m_VerticalAxis.Value -= 20 * Time.deltaTime;
      }
      else {
        pov.m_VerticalAxis.Value += 20 * Time.deltaTime;
      }
    }
}
