using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedWeapon
{
  public GameObject instantiatedWeapon;
  public WeaponItem weaponItem;

  public SpawnedWeapon(GameObject instantiatedWeapon, WeaponItem weaponItem) {
    this.instantiatedWeapon = instantiatedWeapon;
    this.weaponItem = weaponItem;
  }
}

public class WeaponRenderer : MonoBehaviour
{
  [SerializeField] private Transform _weaponContainer;
  [SerializeField] private GameObject _currentlyViewedWeapon;

  private List<SpawnedWeapon> _spawnedWeapons = new();

  // Start is called before the first frame update
  void Start() {
    _spawnedWeapons.Clear();
  }

  public void InstantiateEquippedWeapons(WeaponItem[] equippedWeapons) {
    foreach (WeaponItem weaponItem in equippedWeapons)
    {
      InstantiateWeapon(weaponItem);
    }
  }
  private void InstantiateWeapon(WeaponItem weaponItem) {
    SpawnedWeapon spawningWeapon = new SpawnedWeapon(Instantiate(weaponItem.gunPrefab, _weaponContainer.position + weaponItem.gunOffset, Quaternion.identity, _weaponContainer), weaponItem);
    _spawnedWeapons.Add(spawningWeapon);
  }

  public void UpdateForNewValues(WeaponItem weaponItem, int ammoCount) {
    ShowNewWeapon(weaponItem);
  }
  private void ShowNewWeapon(WeaponItem weaponItem) {
    bool weaponItemInList = false;
    if (weaponItem == null) return;
    foreach (SpawnedWeapon spawnedWeapon in _spawnedWeapons) {
      if (spawnedWeapon.weaponItem == weaponItem) {
        spawnedWeapon.instantiatedWeapon.SetActive(true);
        _currentlyViewedWeapon = spawnedWeapon.instantiatedWeapon;
        weaponItemInList = true;
      } 
      else {
        spawnedWeapon.instantiatedWeapon.SetActive(false);
      }
    }
    // If the weaponItem is not in the list of spawned weapons, instantiate it
    if (weaponItemInList == false) {
      InstantiateWeapon(weaponItem); 
      foreach (SpawnedWeapon spawnedWeapon in _spawnedWeapons) {
        if (spawnedWeapon.weaponItem == weaponItem) {
          spawnedWeapon.instantiatedWeapon.SetActive(true);
          _currentlyViewedWeapon = spawnedWeapon.instantiatedWeapon;
        } 
        else {
          spawnedWeapon.instantiatedWeapon.SetActive(false);
        }
      } 
    }
  }
  // private void iterateSpawnedWeapons() {
}
