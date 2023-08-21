using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemPoolSelectionManager : MonoBehaviour
{
  [Header("Handler References")]
  [SerializeField] private PlayerItemInteractions _playerItemInteractions;

  [FormerlySerializedAs("_itemPoolSelectionSlotManager")]
  [FormerlySerializedAs("_itemPoolSlotManager")]
  [FormerlySerializedAs("_itemSlotManager")]
  [Header("References")]
  [SerializeField] private ItemPoolSlotDisplay _itemPoolSlotDisplay;
  [SerializeField] private ItemDescHandler _itemDescHandler;
  
  [Header("Items")]
  public List<PlayerItem> _generatedItems = new();
  public PlayerItem _selectedItem;

  private void OnValidate() {
    if (_playerItemInteractions == null) {
      _playerItemInteractions = GameObject.FindWithTag("Handler").GetComponent<PlayerItemInteractions>();
    }
  }

  public void Initalize() {
    _generatedItems = _playerItemInteractions.GenerateOfferedItems(_itemPoolSlotDisplay.GetSlotCount()).ToList();
    for (int i = 0; i < _generatedItems.Count; i++) {
      _itemPoolSlotDisplay.SetSlotItemTo(i, _generatedItems[i]);
    }
    _itemPoolSlotDisplay.Initalize();
  }

  public void SelectItem(int slotIndex) {
    _selectedItem = _generatedItems[slotIndex];
    _itemDescHandler.SetDescribedItemTo(_selectedItem);
    _itemPoolSlotDisplay.HighlightItem(slotIndex);
  }
}
