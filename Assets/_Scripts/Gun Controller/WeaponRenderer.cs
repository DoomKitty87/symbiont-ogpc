using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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

  [SerializeField] private List<SpawnedWeapon> _spawnedWeapons = new();

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
    if (weaponItem.gunPrefab == null) {
      Debug.LogError($"WeaponRenderer: WeaponItem '{weaponItem.name}' doesn't contain a gun prefab to instantiate!");
      return;
    }
    SpawnedWeapon spawningWeapon = new SpawnedWeapon(Instantiate(weaponItem.gunPrefab, _weaponContainer.position + weaponItem.gunOffset, Quaternion.identity, _weaponContainer), weaponItem);
    _spawnedWeapons.Add(spawningWeapon);
  }

  public void UpdateForNewValues(WeaponItem weaponItem, int ammoCount) {
    ShowNewWeapon(weaponItem);
  }
  private void ShowNewWeapon(WeaponItem weaponItem) {
    (bool inList, int index) results = WeaponItemIsInList(weaponItem);
    if (results.inList) {
      ViewSpawnedWeaponAtIndex(results.index);
    }
    else {
      InstantiateWeapon(weaponItem);
      // The spawnedWeapon has just been added to the list, so it's index would be -1 from the list.Count
      ViewSpawnedWeaponAtIndex(-1);
    }
  }


  private void ViewSpawnedWeaponAtIndex(int spawnedWeaponListIndex) {
    foreach (SpawnedWeapon spawnedWeapon in _spawnedWeapons) {
      spawnedWeapon.instantiatedWeapon.SetActive(false);
    }
    _spawnedWeapons[spawnedWeaponListIndex].instantiatedWeapon.SetActive(true);
    _currentlyViewedWeapon = _spawnedWeapons[spawnedWeaponListIndex].instantiatedWeapon;
  }
  private (bool inList, int index) WeaponItemIsInList(WeaponItem weaponItem) {
    for (int i = 0; i < _spawnedWeapons.Count; i++) {
      if (_spawnedWeapons[i].weaponItem == weaponItem) {
        return (true, i);
      } 
    }
    return (false, -1);
  }
}
