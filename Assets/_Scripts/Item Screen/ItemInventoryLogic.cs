using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class InventorySlot 
{
  public PlayerItem item;
  public Image image;
}

[RequireComponent(typeof(PlayerItemInteractions))]
public class ItemInventoryLogic : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private PlayerItemInteractions _playerItemInteractions;
  [SerializeField] private DisplaySelectedItem _displaySelectedItem;
  [Header("Settings")]
  public List<InventorySlot> _inventorySlots;
  [SerializeField] private Sprite _emptySlotSprite;

  public void Initalize() {
    if (_playerItemInteractions == null) {
      _playerItemInteractions = GetComponent<PlayerItemInteractions>();
    }
    _inventorySlots = new List<InventorySlot>();
    List<PlayerItem> currentInventory = _playerItemInteractions.GetInventory().ToList();

    for (int i = 0; i < currentInventory.Count; i++) {
      InventorySlot inventorySlot = new InventorySlot();
      inventorySlot.item = currentInventory[i];
      inventorySlot.image.sprite = _emptySlotSprite;
      _inventorySlots.Add(inventorySlot);
    }

    UpdateInventory();
  }

  public void UpdateInventory() {
    List<PlayerItem> currentInventory = _playerItemInteractions.GetInventory().ToList();
    for (int i = 0; i < currentInventory.Count; i++) {
      _inventorySlots[i].item = currentInventory[i];
      _inventorySlots[i].image.sprite = currentInventory[i].item.icon;
    }
  }
}
