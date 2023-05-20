using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponArmsAnimation : MonoBehaviour
{
  // This requires WeaponAnimationOverride to function properly
  
  [Header("Referemces")]
  [SerializeField] private Animator _animator;
  [SerializeField] bool _CanReload;
   
  // Start is called before the first frame update
  void Start() {
    if (_animator == null) {
      Debug.LogError("WeaponArmsAnimation: Animator is null!");
    }
    StartDrawAnimation();
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
