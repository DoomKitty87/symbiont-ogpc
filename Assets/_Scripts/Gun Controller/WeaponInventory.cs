using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnNewCurrentWeapon : UnityEvent<WeaponItem> {}

public class WeaponInventory : MonoBehaviour
{
  // This should store all of the nessecary values to the neccessary scripts when a weapon is selected.

  // This could be used by a larger inventory script to have a one stop shop for assigning weapons;
  // All the inventory script would have to do is assign a weapon item to this, and all other scripts 
  // handling gun logic should update.

  // I'm not sure about this and the regular inventory being seperate, but I think it's a good idea.
  // Should it only contain the current weapon, or should it contain all equipped weapons?
  // What input should be used to switch weapons? Should it be a button, or a scroll wheel?
  // Whats the most scaleable way to implement that input in the UI?

  // TODO: Change this to be an index of _equippedWeapons, and have _equippedWeapons also contain the amount of ammo left in
  // each weapon.
  public WeaponItem _currentWeapon;
  public List<WeaponItem> _equippedWeapons = new();

  public OnNewCurrentWeapon _onNewCurrentWeapon;

  // Start is called before the first frame update
  private void Start() {
    if (_equippedWeapons.Count >= 1) {
      _currentWeapon = _equippedWeapons[0];
    }
  }
  public void AddWeapon(WeaponItem weapon) {
    _equippedWeapons.Add(weapon);
  }
  public void RemoveWeapon(WeaponItem weapon) {
    _equippedWeapons.Remove(weapon);
  }
  public void SetCurrentWeaponByWeaponItem(WeaponItem weapon) {
    foreach (WeaponItem weaponItem in _equippedWeapons) {
      if (weaponItem == weapon) {
        _currentWeapon = weaponItem;
        _onNewCurrentWeapon.Invoke(_currentWeapon);
        return;
      }
    }
    return;
  }
  public void SetCurrentWeaponByIndex(int index) {
    try {
      _currentWeapon = _equippedWeapons[index];
      _onNewCurrentWeapon.Invoke(_currentWeapon);
      return;
    } catch (System.IndexOutOfRangeException) {
      Debug.Log($"WeaponInventory: No weapon equipped at index: {index}");
      return;
    }
  }
}
