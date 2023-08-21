using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

[System.Serializable]
public class InventorySlot 
{
  public PlayerItem item;
  public Image image;
  public ChangeOpacityElement opacityElement;
  public Button button;
  public bool hovered;
}

[RequireComponent(typeof(PlayerItemInteractions))]
public class ItemInventoryDisplay : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private PlayerItemInteractions _playerItemInteractions;
  [SerializeField] private DisplaySelectedItem _displaySelectedItem;
  [Header("Settings")]
  public List<InventorySlot> _inventorySlots;
  [SerializeField] private Sprite _emptySlotSprite;

  private bool _displayedItemAdded;

  public void Initalize() {
    if (_playerItemInteractions == null) {
      _playerItemInteractions = GetComponent<PlayerItemInteractions>();
    }

    List<PlayerItem> currentInventory = _playerItemInteractions.GetInventory().ToList();
    for (int i = 0; i < _inventorySlots.Count; i++) {
      try {
        _inventorySlots[i].item = currentInventory[i];
        _inventorySlots[i].image.sprite = currentInventory[i]._item._icon;
      }
      catch (System.ArgumentOutOfRangeException) {
        _inventorySlots[i].item = null;
        _inventorySlots[i].image.sprite = _emptySlotSprite;
      }
    }

    // for (int i = 0; i < _inventorySlots.Count; i++) {
    //   _inventorySlots[i].button.onClick.AddListener(() => SelectItem(i));
    // }
    _displayedItemAdded = false;
    UpdateInventory();
  }

  public void UpdateInventory() {
    List<PlayerItem> currentInventory = _playerItemInteractions.GetInventory().ToList();
    for (int i = 0; i < currentInventory.Count; i++) {
      _inventorySlots[i].item = currentInventory[i];
      _inventorySlots[i].image.sprite = currentInventory[i]._item._icon;
    }
  }
}
