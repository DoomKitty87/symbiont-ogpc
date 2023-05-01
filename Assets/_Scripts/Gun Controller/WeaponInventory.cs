using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// This should store all of the nessecary values to the neccessary scripts when a weapon is selected.

// This could be used by a larger inventory script to have a one stop shop for assigning weapons;
// All the inventory script would have to do is assign a weapon item to this, and all other scripts 
// handling gun logic should update.

// I'm not sure about this and the regular inventory being seperate, but I think it's a good idea.
// Should it only contain the current weapon, or should it contain all equipped weapons?
// What input should be used to switch weapons? Should it be a button, or a scroll wheel?
// Whats the most scaleable way to implement that input in the UI?

[System.Serializable]
public class OnNewCurrentWeapon : UnityEvent<WeaponItem, int> {}
[System.Serializable]
public class OnInventoryInitalize : UnityEvent<WeaponItem[], WeaponItem> {}

[System.Serializable]
public class EquippedWeapon {
  public WeaponItem _weaponItem;
  public int _ammoLeft;

  public EquippedWeapon(WeaponItem weaponItem, int ammoLeft) {
    _weaponItem = weaponItem;
    _ammoLeft = ammoLeft;
  }
}

public class WeaponInventory : MonoBehaviour
{
  public EquippedWeapon _currentWeapon;
  public List<EquippedWeapon> _equippedWeapons = new();

  public OnNewCurrentWeapon _onNewCurrentWeapon;
  public OnInventoryInitalize _onInventoryInitialize;

  private void Start() {
    if (_equippedWeapons.Count >= 1) {
      _currentWeapon = _equippedWeapons[0];
    }
    WeaponItem[] weaponItems = new WeaponItem[_equippedWeapons.Count];
    for (int i = 0; i < _equippedWeapons.Count; i++) {
      _equippedWeapons[i]._ammoLeft = _equippedWeapons[i]._weaponItem.magSize;
      weaponItems[i] = _equippedWeapons[i]._weaponItem;
    }
    _onInventoryInitialize?.Invoke(weaponItems, _currentWeapon._weaponItem);
    StartCoroutine(StartAfterFrame());
  }
  private IEnumerator StartAfterFrame() {
    yield return null;
    _onNewCurrentWeapon?.Invoke(_currentWeapon._weaponItem, _currentWeapon._ammoLeft);
  }
  public void AddWeapon(WeaponItem weapon) {
    _equippedWeapons.Add(new EquippedWeapon(weapon, weapon.magSize));
  }
  public void RemoveWeapon(WeaponItem weapon) {
    foreach (EquippedWeapon equippedWeapon in _equippedWeapons) {
      if (equippedWeapon._weaponItem == weapon) {
        _equippedWeapons.Remove(equippedWeapon);
        if (_currentWeapon == equippedWeapon) {
          _currentWeapon = null;
        }
        return;
      }
    }
  }
  public void SetCurrentWeaponByWeaponItem(WeaponItem weapon) {
    foreach (EquippedWeapon equippedWeapon in _equippedWeapons) {
      if (equippedWeapon._weaponItem == weapon) {
        _currentWeapon = equippedWeapon;
        _onNewCurrentWeapon.Invoke(_currentWeapon._weaponItem, _currentWeapon._ammoLeft);
        return;
      }
    }
    return;
  }
  public void SetCurrentWeaponByIndex(int index) {
    try {
      if (_currentWeapon == _equippedWeapons[index]) return;
      _currentWeapon = _equippedWeapons[index];
      _onNewCurrentWeapon.Invoke(_currentWeapon._weaponItem, _currentWeapon._ammoLeft);
      return;
    } catch (System.IndexOutOfRangeException) {
      Debug.Log($"WeaponInventory: No weapon equipped at index: {index}");
      return;
    }
  }
  public void DecrementAmmoLeft(int amount) {
    _currentWeapon._ammoLeft -= amount;
  }
  public void ResetAmmoToMax() {
    _currentWeapon._ammoLeft = _currentWeapon._weaponItem.magSize;
  }
}
