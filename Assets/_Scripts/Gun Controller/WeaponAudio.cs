using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WeaponAudio : MonoBehaviour
{
  [SerializeField] private WeaponItem _currentWeaponItem;
  [SerializeField] private int _ammoCount;

  [SerializeField] private AudioSource _audioSource;
  [SerializeField] private AudioClip _shotSound;
  [SerializeField] private AudioClip _shotSoundNearEmpty;
  [SerializeField] private AudioClip _emptyFireSound;
  [SerializeField] private AudioClip _reloadStartSound;
  [SerializeField] private AudioClip _reloadEndSound;
  [SerializeField] private AudioClip _equipSound;

  public void UpdateForNewValues(WeaponItem weaponItem, int ammoCount) {
    _currentWeaponItem = weaponItem;
    _ammoCount = ammoCount;
    
    _shotSound = weaponItem.shotSound;
    _shotSoundNearEmpty = weaponItem.shotSoundNearEmpty;
    _emptyFireSound = weaponItem.emptyFireSound;
    _reloadStartSound = weaponItem.reloadStartSound;
    _reloadEndSound = weaponItem.reloadEndSound;
    _equipSound = weaponItem.equipSound;

    OnEquip();
  }
  // Start is called before the first frame update
  private void Start() {
    _audioSource = gameObject.GetComponent<AudioSource>();
  }

  public void OnEquip() {
    _audioSource.PlayOneShot(_equipSound);
  }
  public void OnFire() {
    _ammoCount--;
    if (_ammoCount <= 0) {
      _audioSource.PlayOneShot(_emptyFireSound);
    } else if (_ammoCount <= _currentWeaponItem.magSize / 4) {
      _audioSource.PlayOneShot(_shotSoundNearEmpty);
    } else {
      _audioSource.PlayOneShot(_shotSound);
    }
  }
  public void OnReloadStart() {
    _audioSource.PlayOneShot(_reloadStartSound);
  }
  public void OnReloadEnd() {
    _audioSource.PlayOneShot(_reloadEndSound);
  }

}
