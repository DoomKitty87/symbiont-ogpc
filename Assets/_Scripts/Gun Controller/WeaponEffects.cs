using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponRenderer))]
public class WeaponEffects : MonoBehaviour
{
  // References aren't through _currentWeaponItem because it would hide changes made by the item; we can fix this later easily,
  // but we should probably do it in all the other scripts to keep it consistent.

  [Header("Weapon Item")]
  [SerializeField] private WeaponItem _currentWeaponItem;
  [Header("References")]
  [Header("Requires a WeaponRenderer to get the instanced weapon.")]
  [SerializeField] private WeaponRenderer _weaponRenderer;
  [SerializeField] private GameObject _weaponInstance;
  private GameObject _weaponInstanceMuzzleObject;
  [SerializeField] private Vector3 _weaponInstanceMuzzlePosition;
  [Header("Shot Effect")]
  [SerializeField] private GameObject _laserBeamPrefab;
  [SerializeField] private Vector3 _effectPositionOffset;
  [SerializeField][ColorUsage(true, true)] private Color _laserScaleUpColor; 
  [SerializeField][ColorUsage(true, true)] private Color _laserScaleDownColor; 
  [SerializeField][ColorUsage(true, true)] private Color _laserEmissionColor; 
  [SerializeField] private float _durationScaleUp;
  [SerializeField] private float _durationScaleDown;

  [Header("Other Effects")]
  [SerializeField] GameObject _muzzleFlashPrefab;

  private bool _hadFirstUpdateForNewValues = false;

  private void Start() {
    _weaponRenderer = gameObject.GetComponent<WeaponRenderer>();
  }

  private void Update() {
    if (!_hadFirstUpdateForNewValues) return;
    _weaponInstanceMuzzlePosition = _weaponInstanceMuzzleObject.transform.position;
  }

  // ammoCount isn't used, but is needed to show up in inspector
  public void UpdateForNewValues(WeaponItem weaponItem, int ammoCount) {
    if (!_hadFirstUpdateForNewValues) _hadFirstUpdateForNewValues = true;

    _currentWeaponItem = weaponItem;
    _effectPositionOffset = weaponItem.effectPositionOffset;
    _laserScaleUpColor = weaponItem.laserScaleUpColor;
    _laserScaleDownColor = weaponItem.laserScaleDownColor;
    _laserEmissionColor = weaponItem.laserEmissionColor;
    _durationScaleUp = weaponItem.durationLaserScaleUp;
    _durationScaleDown = weaponItem.durationLaserScaleDown;
    
    _weaponInstance = _weaponRenderer.GetWeaponInstance(weaponItem);
    if (_weaponInstance == null) {
      Debug.LogError($"WeaponEffects: WeaponInstance was not found for weaponItem '{weaponItem.name}'");
      return;
    }
    _weaponInstanceMuzzleObject = _weaponInstance.transform.Find("MuzzlePosition").gameObject;
    
    if (_weaponInstanceMuzzleObject != null) { _weaponInstanceMuzzlePosition = _weaponInstance.transform.Find("MuzzlePosition").position; }
    else { Debug.LogError($"WeaponEffects: MuzzlePosition object not found in weaponItemPrefab for '{weaponItem.name}'"); }
    if (_muzzleFlashPrefab != null) { _muzzleFlashPrefab = weaponItem.muzzleFlashEffectPrefab; }
    else { Debug.LogWarning($"WeaponEffects: WeaponItem: '{weaponItem.name}' doesn't contain MuzzleFlashPrefab!"); }
  }

  public void StartEffect(Vector3 hitPosition) {
    // Will do custom effects like these through a WeaponAnimator + animations
    // if (activeGun == heavyRifle) StartCoroutine(ReactorGlow());
    // if (activeGun == assaultRifle) StartCoroutine(ChamberCharge());
    StartCoroutine(LaserFX(_weaponInstanceMuzzlePosition, hitPosition));
    if (_muzzleFlashPrefab == null) return;
    StartCoroutine(MuzzleFlashFX(_weaponInstanceMuzzlePosition));
  }
  private IEnumerator MuzzleFlashFX(Vector3 muzzlePosition) {
    GameObject muzzleFlashInstance = Instantiate(_muzzleFlashPrefab, _effectPositionOffset, Quaternion.identity, _weaponInstanceMuzzleObject.transform);
    ParticleSystem muzzleFlashParticleSystem = muzzleFlashInstance.GetComponent<ParticleSystem>();
    muzzleFlashParticleSystem.Play();
    yield return new WaitForSeconds(muzzleFlashParticleSystem.main.duration);
    Destroy(muzzleFlashInstance);
  }

  // Changed to code to spawn the laser inside of the muzzle position gameobject
  private IEnumerator LaserFX(Vector3 startPoint, Vector3 endPoint) {
    float timer = 0f;
    _laserScaleDownColor = Color.white;
    _laserScaleUpColor = Color.clear;

    LineRenderer laserLineRenderer = Instantiate(_laserBeamPrefab, _effectPositionOffset, Quaternion.identity, _weaponInstanceMuzzleObject.transform).GetComponent<LineRenderer>();
    Renderer laserRenderer = laserLineRenderer.gameObject.GetComponent<Renderer>();
    laserLineRenderer.SetPosition(0, startPoint + _effectPositionOffset);
    laserLineRenderer.SetPosition(1, endPoint + _effectPositionOffset);
    laserLineRenderer.material.SetColor("_EmissionColor", _laserEmissionColor);
    laserLineRenderer.enabled = true;

    laserRenderer.material.color = _laserScaleUpColor;

    laserLineRenderer.startWidth = 0f;
    laserLineRenderer.endWidth = laserLineRenderer.startWidth;

    while (timer < _durationScaleUp) {
      laserLineRenderer.startWidth = Mathf.Lerp(0f, 0.25f, timer / _durationScaleUp);
      laserLineRenderer.endWidth = laserLineRenderer.startWidth;
      laserRenderer.material.color = Color.Lerp(_laserScaleDownColor, _laserScaleUpColor, timer / _durationScaleUp);
      timer += Time.deltaTime;
      yield return null;
    }

    timer = 0f;
    laserRenderer.material.color = _laserScaleUpColor;
    laserLineRenderer.startWidth = 0.25f;
    laserLineRenderer.endWidth = laserLineRenderer.startWidth;

    while (timer < _durationScaleDown) {
      laserLineRenderer.startWidth = Mathf.Lerp(0.25f, 0f, timer / _durationScaleDown);
      laserLineRenderer.endWidth = laserLineRenderer.startWidth;
      laserRenderer.material.color = Color.Lerp(_laserScaleUpColor, _laserScaleDownColor, timer / _durationScaleDown);
      timer += Time.deltaTime;
      yield return null;
    }

    laserLineRenderer.startWidth = 0f;
    laserLineRenderer.endWidth = laserLineRenderer.startWidth;
    laserRenderer.material.color = _laserScaleUpColor;
    laserLineRenderer.enabled = false;
    Destroy(laserLineRenderer.gameObject);
  }
}
