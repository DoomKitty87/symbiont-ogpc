using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilOffset : MonoBehaviour
{
  [SerializeField] private float _cameraRecoilOffset;
  private float _upRecoil;
  private float _backRecoil;
  private float _recoilRecovery;
  private float _shotSpread;

  // Start is called before the first frame update
  void Start()
  {
        
  }

  // Update is called once per frame
  void Update()
  {
        
  }

  public void UpdateForNewValues(WeaponItem weaponItem) {
    _upRecoil = weaponItem.upRecoil;
    _backRecoil = weaponItem.backRecoil;
    _recoilRecovery = weaponItem.recoilRecovery;
    _shotSpread = weaponItem.shotSpread;
  }

  public void OnFire() {
    _cameraRecoilOffset = Mathf.Clamp(_cameraRecoilOffset + _upRecoil, 0, 1);
  }
}
