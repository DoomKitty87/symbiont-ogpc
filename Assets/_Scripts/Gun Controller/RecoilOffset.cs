using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RecoilOffset : MonoBehaviour
{
  [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
  private CinemachinePOV _cameraPov; 
  [SerializeField] private float _cameraRecoilOffset;
  private float _verticalRecoil;
  private float _backRecoil;
  private float _recoilRecovery;

  // If the recoil has not already been reset, start resetting it to the 

  // Start is called before the first frame update
  void Start()
  {
    if (_cinemachineVirtualCamera == null) {
      _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
      Debug.LogWarning("RecoilOffset: No Cinemachine Virtual Camera found on recoil offset script, attempting to find one. Did you forget to assign it?");
    }
    _cameraPov = _cinemachineVirtualCamera.m_LookAt.GetComponent<CinemachinePOV>();
    if (_cameraPov == null) {
      Debug.LogError("RecoilOffset: No Cinemachine POV found on recoil offset script, please assign one!");
    }
  }

  // Update is called once per frame
  void Update() {

  }

  public void UpdateForNewValues(WeaponItem weaponItem) {
    _verticalRecoil = weaponItem.verticalRecoilDegrees;
    _backRecoil = weaponItem.backRecoil;
    _recoilRecovery = weaponItem.verticalRecoilRecovery;
  }

  public void OnFire() {
    _cameraRecoilOffset = Mathf.Clamp(_cameraRecoilOffset + _verticalRecoil, _cameraPov.m_VerticalAxis.m_MinValue, _cameraPov.m_VerticalAxis.m_MaxValue);
  }
}
