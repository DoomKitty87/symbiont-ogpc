using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

// Should probably handle the selection
[Serializable]
public class ItemPoolSelectionSlot
{
  public ChangeOpacityElement _fadeScript;
  public Image _itemIconDisplayImage;
  public TextMeshProUGUI _itemCostText;
  public Button _selectionButton;
}

public class ItemPoolSlotDisplay : MonoBehaviour
{
  [SerializeField] private List<ItemPoolSelectionSlot> _itemSlots = new();
  
  public void Initalize() {
    foreach (ItemPoolSelectionSlot slot in _itemSlots) {
      slot._fadeScript.OpacityOut(false);      
    }
  }

  public int GetSlotCount() {
    return _itemSlots.Count;
  }
  
  public void SetSlotItemTo(int slotIndex, PlayerItem playerItem) {
    if (slotIndex >= _itemSlots.Count) {
      Debug.LogWarning("ItemPoolSlotDisplay: SetSlot slotIndex is out of range! Check _itemSlots to make sure it has enough slots.");
    }
    else {
      _itemSlots[slotIndex]._itemIconDisplayImage.sprite = playerItem._item._icon;
      _itemSlots[slotIndex]._itemCostText.text = playerItem._initCost.ToString();
    }
  }
  public void HighlightItem(int slotIndex) {
    if (slotIndex >= _itemSlots.Count) {
      Debug.LogError("ItemPoolSlotDisplay: SelectItem slotIndex is out of range! Check _itemSlots to make sure it has enough slots.");
      return;
    }
    for (int i = 0; i < _itemSlots.Count; i++) {
      ItemPoolSelectionSlot slot = _itemSlots[i];
      if (i != slotIndex) {
        slot._fadeScript.OpacityOut(false);
        continue;
      }
      else {
        slot._fadeScript.OpacityIn(false);
      }
    }
  }
}
