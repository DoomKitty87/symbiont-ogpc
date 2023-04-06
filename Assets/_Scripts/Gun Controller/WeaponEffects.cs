using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEffects : MonoBehaviour
{
  // References aren't through _currentWeaponItem because it would hide changes made by the item; we can fix this later easily,
  // but we should probably do it in all the other scripts to keep it consistent.

  [Header("Weapon Item")]
  [SerializeField] WeaponItem _currentWeaponItem;
  [Header("References")]
  [SerializeField] GameObject _laserBeamPrefab;
  [Header("Settings (WeaponItem)")]
  [SerializeField][ColorUsage(true, true)] Color _shotColor; 
  [SerializeField] GameObject _muzzleFlashPrefab;

  // ammoCount isn't used, but is needed to show up in inspector
  public void UpdateForNewValues(WeaponItem weaponItem, int ammoCount) {
    _currentWeaponItem = weaponItem;
    _shotColor = weaponItem.shotColor;
    if (_muzzleFlashPrefab != null) _muzzleFlashPrefab = weaponItem.muzzleFlashEffectPrefab;
    else Debug.LogWarning($"WeaponEffects: WeaponItem: '{weaponItem.name}' doesn't contain MuzzleFlashPrefab!");
  }


  // TODO: Get hitposition from FireGunLogic, and get muzzle position from prefab somehows
  public void StartEffect(Vector3 muzzlePosition, Vector3 hitPosition) {
    // shootSound.Play();
    // Will do custom effects like these through a WeaponAnimator + animations
    // if (activeGun == heavyRifle) StartCoroutine(ReactorGlow());
    // if (activeGun == assaultRifle) StartCoroutine(ChamberCharge());
    StartCoroutine(LaserFX(muzzlePosition, hitPosition));
    if (_muzzleFlashPrefab == null) return;
    _muzzleFlashPrefab.GetComponent<ParticleSystem>().Play();
  }

  // ansel code
  private IEnumerator LaserFX(Vector3 startPoint, Vector3 endPoint) {
    float timer = 0f;
    float durationIn = 0.08f;
    float durationOut = 0.1f;
    Color colorIn = Color.white;
    Color colorOut = Color.clear;
    LineRenderer laserLineRenderer = Instantiate(_laserBeamPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform).GetComponent<LineRenderer>();
    Renderer laserRenderer = laserLineRenderer.gameObject.GetComponent<Renderer>();
    laserLineRenderer.SetPosition(0, startPoint);
    laserLineRenderer.SetPosition(1, endPoint);
    laserLineRenderer.material.SetColor("_EmissionColor", _shotColor);
    laserLineRenderer.enabled = true;
    laserRenderer.material.color = colorOut;
    laserLineRenderer.startWidth = 0f;
    laserLineRenderer.endWidth = laserLineRenderer.startWidth;
    while (timer < durationIn) {
      laserLineRenderer.startWidth = Mathf.Lerp(0f, 0.25f, timer / durationIn);
      laserLineRenderer.endWidth = laserLineRenderer.startWidth;
      laserRenderer.material.color = Color.Lerp(colorOut, colorIn, timer / durationIn);
      timer += Time.deltaTime;
      yield return null;
    }
    timer = 0f;
    laserRenderer.material.color = colorIn;
    laserLineRenderer.startWidth = 0.25f;
    laserLineRenderer.endWidth = laserLineRenderer.startWidth;
    while (timer < durationOut) {
      laserLineRenderer.startWidth = Mathf.Lerp(0.25f, 0f, timer / durationOut);
      laserLineRenderer.endWidth = laserLineRenderer.startWidth;
      laserRenderer.material.color = Color.Lerp(colorIn, colorOut, timer / durationOut);
      timer += Time.deltaTime;
      yield return null;
    }
    laserLineRenderer.startWidth = 0f;
    laserLineRenderer.endWidth = laserLineRenderer.startWidth;
    laserRenderer.material.color = colorOut;
    laserLineRenderer.enabled = false;
    Destroy(laserLineRenderer.gameObject);
  }
}
