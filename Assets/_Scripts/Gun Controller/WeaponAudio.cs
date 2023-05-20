using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WeaponAudio : MonoBehaviour
{
  [Header("Weapon Item")]
  [SerializeField] private WeaponItem _currentWeaponItem;
  [SerializeField] private int _ammoCount;

  [Header("Audio Clips")]
  [SerializeField] private AudioSource _audioSource;
  [SerializeField] private AudioClip _shotSound;
  [SerializeField] private AudioClip _shotSoundNearEmpty;
  [SerializeField] private AudioClip _emptyFireSound;
  [SerializeField] private AudioClip _reloadStartSound;
  [SerializeField] private AudioClip _reloadEndSound;
  [SerializeField] private AudioClip _equipSound;

  private float _initVol;
  private float _lastAudioVol;

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
    _initVol = _audioSource.volume;
  }

  private void Update() {
    if (PlayerPrefs.GetFloat("SOUND_VOLUME_EFFECTS") / 100 != _lastAudioVol) {
      _lastAudioVol = PlayerPrefs.GetFloat("SOUND_VOLUME_EFFECTS") / 100;
      _audioSource.volume = _initVol * _lastAudioVol;
    }
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
    _ammoCount = _currentWeaponItem.magSize;
  }

}
