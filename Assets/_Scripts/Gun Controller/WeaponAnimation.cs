using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class WeaponAnimation : MonoBehaviour
{
  // TODO: Split this up into two scripts that handle  
  // 1: switching the overrides
  // 2: animation triggers
  
  [Header("Referemces")]
  [SerializeField] private Animator _animator;
  [SerializeField] private AnimatorController _baseAnimatorController;
  [SerializeField] private AnimatorOverrideController _unarmedAnimatorOverride;
  [SerializeField] bool _IsArmed;

  // Used so we don't have to search for the reload animation every time we want to play it,
  // only when we first call reload on a new weapon
  private AnimationClip _referencedReloadAnimation;
   
  // Start is called before the first frame update
  void Start() {
    if (_animator == null) {
      Debug.LogError("WeaponAnimation: Animator is null!");
    }
    if (_baseAnimatorController == null) {
      Debug.LogError("WeaponAnimation: BaseAnimatorController is null!");
    }
    if (_unarmedAnimatorOverride == null) {
      Debug.LogError("WeaponAnimation: UnarmedAnimatorOverride is null!");
    }
    _animator.runtimeAnimatorController = _unarmedAnimatorOverride;
    StartDrawAnimation();
  }

  // Ammo count will be used to determine if we should play the empty reload animation
  public void UpdateForNewValues(WeaponItem weaponItem, int ammoCount) {
    if (weaponItem.animatorOverride == null) {
      _animator.runtimeAnimatorController = _unarmedAnimatorOverride;
      _IsArmed = false;
      return;
    }
    _animator.runtimeAnimatorController = weaponItem.animatorOverride;
    _referencedReloadAnimation = null;
    StartDrawAnimation();
    print("WeaponAnimation: UpdateForNewValues: ammoCount: " + ammoCount);
  }
  public void StartReloadAnimation(float reloadTime) {
    if (_IsArmed == false) {
      return;
    }
    if (_referencedReloadAnimation != null) {
      float _reloadScale = reloadTime / _referencedReloadAnimation.length;
      _animator.SetFloat("ReloadSpeed", _reloadScale);
      _animator.SetTrigger("OnReload");
      return;
    }
    // Get the duration of the reload animation at 1x speed, and then scale it to the reload time
    AnimationClip[] _animationClips = _animator.runtimeAnimatorController.animationClips;
    for (int i = 0; i < _animationClips.Length; i++) {
      if (_animationClips[i].name.Contains("Reload")) {
        _referencedReloadAnimation = _animationClips[i];
        float _reloadScale = reloadTime / _referencedReloadAnimation.length;
        _animator.SetFloat("ReloadSpeed", _reloadScale);
        _animator.SetTrigger("OnReload");
        return;
      }
    }
  }
  public void StartDrawAnimation() {
    _animator.SetTrigger("OnDraw");
  }
  public void StartFireAnimation() {
    if (_IsArmed == false) {
      return;
    }
    _animator.SetTrigger("OnFire");
  }
}
