using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedWeapon
{
  // Reference to the spawned gun and its respective weapon item
}

public class WeaponRenderer : MonoBehaviour
{
  [SerializeField] private Transform _weaponContainer;
  [SerializeField] private GameObject _currentlyViewedWeapon;

  private List<GameObject> _spawnedWeapons = new();

  // Start is called before the first frame update
  void Start() {
    _spawnedWeapons.Clear();
  }

  public void InstantiateEquippedWeapons(WeaponItem[] equippedWeapons) {
    foreach (WeaponItem weaponItem in equippedWeapons)
    {
      GameObject spawnedWeapon = Instantiate(weaponItem.gunPrefab, _weaponContainer.position + weaponItem.gunOffset, Quaternion.identity, _weaponContainer);
      _spawnedWeapons.Add(spawnedWeapon);
    }
  }

  public void UpdateForNewValues(WeaponItem weaponItem, int ammoCount) {
    // Show the weapon being updated
  }

  public void ShowNewWeapon() {
    // If the weapon is in the list, show it. Otherwise, instantiate and add it to the list,
    // and show.
  }

  // Update is called once per frame
  void Update() {
        
  }
}
