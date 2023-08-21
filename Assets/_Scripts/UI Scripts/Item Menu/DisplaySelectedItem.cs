using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using UnityEngine.Serialization;

[RequireComponent(typeof(PlayerItemInteractions))]
public class DisplaySelectedItem : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private PlayerItemInteractions _playerItemInteractions;
  [FormerlySerializedAs("_itemSelectionInit")] [FormerlySerializedAs("_itemSelection3")] [SerializeField] private ItemPoolSelectionManager _itemPoolSelectionManager;
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
    _selectedItem = _itemPoolSelectionManager._selectedItem;
    SetSlot1(_selectedItem);
    SetTextInfo(_selectedItem);
  }

  private void SetSlot1(PlayerItem playerItem) {
    _image1.sprite = playerItem._item._icon;
    // _cost1.text = playerItem.item.cost.ToString();
  }

  private void SetTextInfo(PlayerItem playerItem) {
    if (playerItem._debuff == null) {
      _nameWithModifierText.text = playerItem._item._name.ToUpper();
      _flavorText.text = playerItem._item._flavorText;

      _rarityText.text = _rarityStrings[playerItem._item._rarity].ToUpper();
      _rarityText.color = _rarityColors[playerItem._item._rarity];

      _itemDescription.text = playerItem._item._description;
      _debuffDescription.text = "";
      _debuffHeader.text = "";
      return;
    }
    else {
      string debuffName = playerItem._debuff._name.ToUpper();
      string itemName = playerItem._item._name.ToUpper();

      _nameWithModifierText.text = $"<font={_debuffFontName}>{debuffName}</font> {itemName}";
      _flavorText.text = playerItem._item._flavorText;

      _rarityText.text = _rarityStrings[playerItem._item._rarity].ToUpper();
      _rarityText.color = _rarityColors[playerItem._item._rarity];

      _itemDescription.text = playerItem._item._description;
      _debuffHeader.text = "BUT";
      _debuffDescription.text = playerItem._debuff._description;
    }
  }
}
