using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

[RequireComponent(typeof(PlayerItemInteractions))]
public class ItemSelectionInit : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private PlayerItemInteractions _playerItemInteractions;

  [Header("Slot 1")]
  [SerializeField] private ChangeOpacityElement _fade1;
  [SerializeField] private Image _image1;
  [SerializeField] private TextMeshProUGUI _cost1;
  [SerializeField] private Button _button1;
  [Header("Slot 2")]
  [SerializeField] private ChangeOpacityElement _fade2;
  [SerializeField] private Image _image2;
  [SerializeField] private TextMeshProUGUI _cost2;
  [SerializeField] private Button _button2;
  [Header("Slot 3")]
  [SerializeField] private ChangeOpacityElement _fade3;
  [SerializeField] private Image _image3;
  [SerializeField] private TextMeshProUGUI _cost3;
  [SerializeField] private Button _button3;

  
  [Header("Text References")]
  [SerializeField] private TextMeshProUGUI _nameWithModifierText;
  [SerializeField] private TextMeshProUGUI _flavorText;
  [SerializeField] private TextMeshProUGUI _rarityText;
  [SerializeField] private TextMeshProUGUI _itemDescription;
  [SerializeField] private TextMeshProUGUI _debuffHeader;
  [SerializeField] private TextMeshProUGUI _debuffDescription;

  [Header("Items")]
  [SerializeField] private PlayerItem[] _items = new PlayerItem[3];
  public PlayerItem _selectedItem;

  [Header("Settings")]
  [SerializeField] private string _debuffFontName;
  [SerializeField] private List<string> _rarityStrings;
  [SerializeField] private List<Color> _rarityColors;

  public void Initalize() {
    if (_playerItemInteractions == null) {
      _playerItemInteractions = GetComponent<PlayerItemInteractions>();
    }
    _playerItemInteractions.ShopScreenTrigger();
    _items = _playerItemInteractions.RollOfferedItems();
    SetItems(_items[0], _items[1], _items[2]);
    _fade1.OpacityIn(true);
    _fade2.OpacityOut(true);
    _fade3.OpacityOut(true);
  }

  public void SetItems(PlayerItem item1, PlayerItem item2, PlayerItem item3) {
    _items[0] = item1;
    SetSlot1(item1);
    SetTextInfo(item1);
    _items[1] = item2;
    SetSlot2(item2);
    _items[2] = item3;
    SetSlot3(item3);
  }

  public void SelectItem1() {
    print("Selecting item 1");
    _selectedItem = _items[0];
    _fade1.OpacityIn(false);
    _fade2.OpacityOut(false);
    _fade3.OpacityOut(false);
    SetTextInfo(_items[0]);
  }
  public void SelectItem2() {
    print("Selecting item 2");
    _selectedItem = _items[1];
    _fade1.OpacityOut(false);
    _fade2.OpacityIn(false);
    _fade3.OpacityOut(false);
    SetTextInfo(_items[1]);
  }
  public void SelectItem3() {
    print("Selecting item 3");
    _selectedItem = _items[2];
    _fade1.OpacityOut(false);
    _fade2.OpacityOut(false);
    _fade3.OpacityIn(false);
    SetTextInfo(_items[2]);
  }

  private void SetSlot1(PlayerItem playerItem) {
    _image1.sprite = playerItem.item.icon;
    _cost1.text = playerItem.item.cost.ToString();
  }
  private void SetSlot2(PlayerItem playerItem) {
    _image2.sprite = playerItem.item.icon;
    _cost2.text = playerItem.item.cost.ToString();
  }
  private void SetSlot3(PlayerItem playerItem) {
    _image3.sprite = playerItem.item.icon;
    _cost3.text = playerItem.item.cost.ToString();
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
