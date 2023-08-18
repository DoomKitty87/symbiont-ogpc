using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ItemPoolSelectionManager : MonoBehaviour
{
  // This absolutely needs to be broken down and fixed before we can continue working.
  
  [Header("Handler References")]
  [SerializeField] private PlayerItemInteractions _playerItemInteractions;

  [Header("References")]
  [SerializeField] private ItemPoolSelectionSlotManager _itemSlotManager;
  
  // Separate this into an "item selection info"
  [Header("Text References")]
  [SerializeField] private TextMeshProUGUI _nameWithModifierText;
  [SerializeField] private TextMeshProUGUI _flavorText;
  [SerializeField] private TextMeshProUGUI _rarityText;
  [SerializeField] private TextMeshProUGUI _itemDescription;
  [SerializeField] private TextMeshProUGUI _debuffHeader;
  [SerializeField] private TextMeshProUGUI _debuffDescription;

  // This is fine to keep here, since this is "item pool selection manager" then it should handle item generation + reference
  [Header("Items")]
  public List<PlayerItem> _items = new();
  public PlayerItem _selectedItem;

  [Header("Settings")]
  // Used because TMPro needs the full font name in an HTML-like tag
  // to display different fonts in the same text object,
  [SerializeField] private string _debuffFontName;
  
  // This should be removed
  [SerializeField] private List<string> _rarityStrings;
  [SerializeField] private List<Color> _rarityColors;

  private void OnValidate() {
    if (_playerItemInteractions == null) {
      _playerItemInteractions = GameObject.FindWithTag("Handler").GetComponent<PlayerItemInteractions>();
    }
  }

  public void Initalize() {
    _items = _playerItemInteractions.GenerateOfferedItems().ToList();
    SetItems(_items[0], _items[1], _items[2]);
    _itemSlotManager.Initalize();
  }

  public void SetItems(PlayerItem item1, PlayerItem item2, PlayerItem item3) {
    _items[0] = item1;
    _itemSlotManager.SetSlot1(item1);
    SetTextInfo(item1);
    _items[1] = item2;
    _itemSlotManager.SetSlot2(item2);
    _items[2] = item3;
    _itemSlotManager.SetSlot3(item3);
  }

  public void SelectItem1() {
    print("Selecting item 1");
    _selectedItem = _items[0];
    SetTextInfo(_items[0]);
    _itemSlotManager.SelectItem1();
  }
  public void SelectItem2() {
    print("Selecting item 2");
    _selectedItem = _items[1];
    SetTextInfo(_items[1]);
    _itemSlotManager.SelectItem2();
  }
  public void SelectItem3() {
    print("Selecting item 3");
    _selectedItem = _items[2];
    SetTextInfo(_items[2]);
    _itemSlotManager.SelectItem3();
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
