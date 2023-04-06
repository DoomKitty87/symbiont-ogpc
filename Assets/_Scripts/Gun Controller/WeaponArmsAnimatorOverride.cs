using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponArmsAnimatorOverride : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private Animator _animatorToOverride;
  [SerializeField] private WeaponItem _currentWeaponItemAnimated;
  [Header("Settings")]
  [SerializeField] private AnimatorOverrideController _defaultOverride;
  [Header("Events")]
  public UnityEvent _animationOverrideIsReady;

  // Start is called before the first frame update
  void Start() {
    _animatorToOverride.runtimeAnimatorController = _defaultOverride;
  }

  public void UpdateForNewValues(WeaponItem weaponItem, int ammoCount) {
    if (weaponItem.animatorOverride == null) {
      Debug.LogError("AnimationOverride: WeaponItem does not contain an override to use!");
      return;
    }
    _animatorToOverride.runtimeAnimatorController = weaponItem.animatorOverride;
    _currentWeaponItemAnimated = weaponItem;

    float reloadAnimationLength = GetReloadAnimationLength();
    SetReloadTimeScale(weaponItem.reloadTimeSeconds, reloadAnimationLength);
  }
  private float GetReloadAnimationLength() {
    foreach (AnimationClip animationClip in _animatorToOverride.runtimeAnimatorController.animationClips) {
      if (animationClip.name.Contains("reload") || animationClip.name.Contains("Reload")) {
        return animationClip.length;
      }
    }
    return float.NaN;
  }
  private void SetReloadTimeScale(float reloadLength, float reloadAnimationLength) {
    _animatorToOverride.SetFloat("ReloadAnimationSpeed", reloadAnimationLength / reloadLength);
  }
}
