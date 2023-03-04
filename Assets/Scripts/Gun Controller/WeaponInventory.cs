using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
  // This should store all of the nessecary values to the neccessary scripts when a weapon is selected.
  // This should be directly referenced by the other scripts, so we need to add [RequireComponent()] into
  // them.

  // Maybe make into a list so we can modify how many guns the player can hold later?

  // This could be used by a larger inventory script to have a one stop shop for assigning weapons;
  // All the inventory script would have to do is assign a weapon item to this, and all other scripts 
  // handling gun logic would update.

  // What kind of logic does this need to handle? Does it need to handle any at all?
  // This is definitely better than assigning the weapon item to each individual script though.


  public List<WeaponItem> _weaponList = new();

  // Start is called before the first frame update
  private void Start() {
      
  }

  // Update is called once per frame
  private void Update() {
      
  }
}
