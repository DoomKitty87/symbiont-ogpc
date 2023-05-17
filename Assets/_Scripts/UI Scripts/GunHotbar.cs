using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GunHotbar : MonoBehaviour
{

  [SerializeField] private GameObject _gunSlot1, _gunSlot2;
  [SerializeField] private Sprite _emptySprite;

  public void LoadInfoForGuns(WeaponItem[] weapons, WeaponItem weapon) {
    if (weapons.Length == 2) {
      _gunSlot1.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = weapons[0].name.ToUpper();
      _gunSlot2.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = weapons[1].name.ToUpper();
      _gunSlot1.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = weapons[0].sprite;
      _gunSlot2.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = weapons[0].sprite;
    }
    else {
      _gunSlot1.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = weapons[0].name.ToUpper();
      _gunSlot2.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "NULL WEAPON";
      _gunSlot1.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = weapons[0].sprite;
      _gunSlot2.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = null;
      _gunSlot2.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
    }
  }

  public void SelectNewGun() {

  }
}