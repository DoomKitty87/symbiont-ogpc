using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunHotbar : MonoBehaviour
{

  [SerializeField] private GameObject _gunSlot1, _gunSlot2;

  public void LoadInfoForGuns(WeaponItem[] weapons, WeaponItem weapon) {
    if (weapons.Length == 2) {
      _gunSlot1.gameObject.GetComponent<TextMeshProUGUI>().text = weapons[0].name;
      _gunSlot2.gameObject.GetComponent<TextMeshProUGUI>().text = weapons[1].name;
    }
    else {
      _gunSlot1.GetComponent<TextMeshProUGUI>().text = weapons[0].name;
      _gunSlot2.GetComponent<TextMeshProUGUI>().text = "NULL WEAPON";
    }
  }

  public void SelectNewGun() {

  }
}