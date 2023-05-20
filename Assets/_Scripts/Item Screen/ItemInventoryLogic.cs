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
  public bool selected;
}

[RequireComponent(typeof(PlayerItemInteractions))]
public class ItemInventoryLogic : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private PlayerItemInteractions _playerItemInteractions;
  [SerializeField] private DisplaySelectedItem _displaySelectedItem;
  [Header("Text References")]
  [SerializeField] private TextMeshProUGUI _nameWithModifierText;
  [SerializeField] private TextMeshProUGUI _flavorText;
  [SerializeField] private TextMeshProUGUI _rarityText;
  [SerializeField] private TextMeshProUGUI _itemDescription;
  [SerializeField] private TextMeshProUGUI _debuffHeader;
  [SerializeField] private TextMeshProUGUI _debuffDescription;
  [SerializeField] private string _debuffFontName;
  [SerializeField] private List<string> _rarityStrings;
  [SerializeField] private List<Color> _rarityColors;
  [Header("Settings")]
  public List<InventorySlot> _inventorySlots;
  [SerializeField] private Sprite _emptySlotSprite;

  public void Initalize() {
    if (_playerItemInteractions == null) {
      _playerItemInteractions = GetComponent<PlayerItemInteractions>();
    }

    List<PlayerItem> currentInventory = _playerItemInteractions.GetInventory().ToList();
    for (int i = 0; i < _inventorySlots.Count; i++) {
      _inventorySlots[i].item = currentInventory[i];
      _inventorySlots[i].image.sprite = _emptySlotSprite;
    }

    for (int i = 0; i < _inventorySlots.Count; i++) {
      _inventorySlots[i].button.onClick.AddListener(() => SelectItem(i));
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

  public void SelectItem(int slot) {
    _inventorySlots[slot].opacityElement.OpacityIn(false);
    _inventorySlots[slot].selected = true;
    for (int i = 0; i < _inventorySlots.Count; i++) {
      if (i != slot) {
        _inventorySlots[i].opacityElement.OpacityOut(false);
        _inventorySlots[i].selected = false;
      }
    }
  }

  private void SetTextInfo(PlayerItem playerItem) {
    if (playerItem.debuff == null) {
      _nameWithModifierText.text = playerItem.item.name.ToUpper();
      _flavorText.text = playerItem.item.flavorText;

      _rarityText.text = _rarityStrings[playerItem.item.rarity].ToUpper();
      _rarityText.color = _rarityColors[playerItem.item.rarity];

      _itemDescription.text = playerItem.item.description;
      _debuffDescription.text = "";
      _debuffHeader.text = "";
      return;
    }
    else {
      string debuffName = playerItem.debuff.name.ToUpper();
      string itemName = playerItem.item.name.ToUpper();

      _nameWithModifierText.text = $"<font={_debuffFontName}>{debuffName}</font> {itemName}";
      _flavorText.text = playerItem.item.flavorText;

      _rarityText.text = _rarityStrings[playerItem.item.rarity].ToUpper();
      _rarityText.color = _rarityColors[playerItem.item.rarity];

      _itemDescription.text = playerItem.item.description;
      _debuffHeader.text = "BUT";
      _debuffDescription.text = playerItem.debuff.description;
    }
  }
}
