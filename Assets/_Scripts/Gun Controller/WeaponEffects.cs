using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour {
  public GameObject _particlePrefab;

  public enum EffectType {
    Muzzle,
    Hitpoint,
    Shot,
  }
  
  public EffectType _effectType;  

  public void StartEffect() {

  }
}

public class WeaponEffects : MonoBehaviour
{
  // Create a base Effect class
  // Base class contains a StartEffect(), particle, and position
  // Simplest effect, plays a particle at that position until it's over
  // 
  // Have new effects derive from that class
  //

  // Priority 1: Create laser/bullet effect coming from the barrel to the hitpoint (in FPS view only)
  // Priority 2: Extra weapon effects? Muzzle flash? Explosion at hitpoint?
  
  [SerializeField] private List<Effect> _effectsList = new(); 
  
  // ammoCount isn't used, but is needed to show up in inspector
  public void UpdateForNewValues(WeaponItem weaponItem, int ammoCount) {

  }
}
