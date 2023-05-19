using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateItemInfo : MonoBehaviour
{

    [SerializeField] private PlayerItems _playerItems;
    [SerializeField] private GameObject _slotParent;
    // [SerializeField] private ItemSelection _itemSelection;


    private void UpdateInventory() {
        PlayerItem[] inventory = _playerItems.GetPlayerItems();
        for (int i = 0; i < 5; i++) {
            _slotParent.transform.GetChild(i).GetComponent<Image>().sprite = inventory[0].item.icon;
        }
    }
}
