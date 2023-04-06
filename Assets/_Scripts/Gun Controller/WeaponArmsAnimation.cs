using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class WeaponArmsAnimation : MonoBehaviour
{
  // This requires WeaponAnimationOverride to function properly
  
  [Header("Referemces")]
  [SerializeField] private Animator _animator;
  [SerializeField] bool _CanReload;
   
  // Start is called before the first frame update
  void Start() {
    if (_animator == null) {
      Debug.LogError("WeaponAnimation: Animator is null!");
    }
    StartDrawAnimation();
  } 
   
  public void UpdateForNewValues(WeaponItem weaponItem, int ammoCount) {
    if (weaponItem.animatorOverride == null) {
      Debug.LogError("AnimationOverride: WeaponItem does not contain an override to use!");
      return;
    }
    StartDrawAnimation();
    print("WeaponAnimation: UpdateForNewValues: ammoCount: " + ammoCount);
  }

  public void StartReloadAnimation() {
    if (_CanReload == false) return;
    _animator.SetTrigger("OnReload");
    _animator.SetBool("OnFire", false);
  }

  public void StartDrawAnimation() {
    _animator.SetTrigger("OnDraw");
  }

  public void StartFireAnimation() {
    _animator.SetTrigger("OnFire");
  }
}
