using UnityEngine;

public class RecoilOffset : MonoBehaviour
{
  private Vector3 _currentRotation;
  private Vector3 _targetRotation;

  [Header("Recoil Values")]
  [SerializeField] private float _verticalRecoil;
  [SerializeField] private float _horizontalRecoil;
  [SerializeField] private float _recoilRecoverySpeed;
  [SerializeField] private float _toTargetCameraRotationSpeed;

  [Header("Effects")]
  [SerializeField] private float _zCameraShake;

  private void Start() {
    _currentRotation = transform.rotation.eulerAngles;
    _targetRotation = transform.rotation.eulerAngles;    
  }

  // Current ammo isn't used in this script, but it's used in the WeaponInventory script, so
  // I think it has to be for it to show up in the UnityEvent in the WeaponInventory script.
  public void UpdateForNewValues(WeaponItem weaponItem, int currentAmmo) {
    _verticalRecoil = weaponItem.verticalRecoil;
    _horizontalRecoil = weaponItem.horizontalRecoilDeviation;
    _recoilRecoverySpeed = weaponItem.recoilRecoverySpeed;
    _toTargetCameraRotationSpeed = weaponItem.recoilSnapiness;
    _zCameraShake = weaponItem.zCameraShake;
  }

  private void Update() {
    // _targetRotation is always lerped towards 0, 0, 0, since thats when theres no recoil offset. 
    _targetRotation = Vector3.Lerp(_targetRotation, new Vector3(0, 0, 0), _recoilRecoverySpeed * Time.deltaTime);
    // Uses slerp because apparently it's "better" for rotations. No idea why.
    _currentRotation = Vector3.Slerp(_currentRotation, _targetRotation, _toTargetCameraRotationSpeed * Time.deltaTime);

    transform.localRotation = Quaternion.Euler(_currentRotation);
  }

  public void OnFire() {
    // Adds a random value to the horizontal recoil, so that the recoil isn't always the same.
    _targetRotation -= new Vector3(_verticalRecoil, Random.Range(-_horizontalRecoil, _horizontalRecoil), Random.Range(-_zCameraShake, _zCameraShake));
  }
}
