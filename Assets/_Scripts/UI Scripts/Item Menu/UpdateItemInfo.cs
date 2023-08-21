using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UpdateItemInfo : MonoBehaviour
{

    [FormerlySerializedAs("_playerItems")] [SerializeField] private PlayerItemsHandler _playerItemsHandler;
    [SerializeField] private GameObject _slotParent;
    // [SerializeField] private ItemSelection _itemSelection;


    private void UpdateInventory() {
        PlayerItem[] inventory = _playerItemsHandler.GetPlayerItems();
        for (int i = 0; i < 5; i++) {
            _slotParent.transform.GetChild(i).GetComponent<Image>().sprite = inventory[0]._item._icon;
        }
    }
}
