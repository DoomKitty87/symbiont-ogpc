using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnNewCurrentWeapon : UnityEvent<WeaponItem> {}

public class WeaponInventory : MonoBehaviour
{
  // This should store all of the nessecary values to the neccessary scripts when a weapon is selected.
  // This should be directly referenced by the other scripts, so we need to add [RequireComponent()] into
  // them.

  // This could be used by a larger inventory script to have a one stop shop for assigning weapons;
  // All the inventory script would have to do is assign a weapon item to this, and all other scripts 
  // handling gun logic would update.

  // This needs to handle the weapon switching, and assigning those values to the needed scripts.

  public WeaponItem _currentWeapon;
  public List<WeaponItem> _equippedWeapons = new();

  public OnNewCurrentWeapon _onNewCurrentWeapon;

  // Start is called before the first frame update
  private void Start() {
    if (_equippedWeapons.Count >= 1) {
      _currentWeapon = _equippedWeapons[0];
    }
  }

  // Update is called once per frame
  private void Update() {
      
  }
}
