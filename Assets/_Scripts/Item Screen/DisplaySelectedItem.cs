using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

[RequireComponent(typeof(PlayerItemInteractions))]
public class DisplaySelectedItem : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private PlayerItemInteractions _playerItemInteractions;
  [SerializeField] private ItemSelection3 _itemSelection3;
  [Header("UI")]
  [SerializeField] private Image _image1;
  
  [Header("Text References")]
  [SerializeField] private TextMeshProUGUI _nameWithModifierText;
  [SerializeField] private TextMeshProUGUI _flavorText;
  [SerializeField] private TextMeshProUGUI _rarityText;
  [SerializeField] private TextMeshProUGUI _itemDescription;
  [SerializeField] private TextMeshProUGUI _debuffHeader;
  [SerializeField] private TextMeshProUGUI _debuffDescription;

  [Header("Item")]
  public PlayerItem _selectedItem;

  [Header("Settings")]
  [SerializeField] private string _debuffFontName;
  [SerializeField] private List<string> _rarityStrings;
  [SerializeField] private List<Color> _rarityColors;

  public void Initalize() {
    if (_playerItemInteractions == null) {
      _playerItemInteractions = GetComponent<PlayerItemInteractions>();
    }
    _selectedItem = _itemSelection3._selectedItem;
    SetSlot1(_selectedItem);
    SetTextInfo(_selectedItem);
  }

  private void SetSlot1(PlayerItem playerItem) {
    _image1.sprite = playerItem.item.icon;
    // _cost1.text = playerItem.item.cost.ToString();
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
