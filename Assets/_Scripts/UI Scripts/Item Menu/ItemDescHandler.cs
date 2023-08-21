using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemDescHandler : MonoBehaviour
{
  public PlayerItem _describedItem;

  [Header("Text References")]
  [SerializeField] private TextMeshProUGUI _nameWithModifierText;

  [SerializeField] private TextMeshProUGUI _flavorText;
  [SerializeField] private TextMeshProUGUI _rarityText;
  [SerializeField] private TextMeshProUGUI _itemDescription;
  [SerializeField] private TextMeshProUGUI _debuffHeader;
  [SerializeField] private TextMeshProUGUI _debuffDescription;

  [Header("Settings")]
  // Used because TMPro needs the full font name in an HTML-like tag
  // to display different fonts in the same text object,
  [SerializeField] private string _debuffFontName;

  private List<string> _rarityStrings = new() {"Common", "Uncommon", "Rare", "Legendary"};
  private List<Color> _rarityColors = new() {new Color(255, 255, 255), new Color(128, 255, 102), new Color(102, 204, 255), new Color(255, 230, 102)};

  public void SetDescribedItemTo(PlayerItem playerItem) {
    _describedItem = playerItem;
    SetInfo();
  }
  private void SetInfo() {
    if (_describedItem._debuff == null) {
      _nameWithModifierText.text = _describedItem._item._name.ToUpper();
      _flavorText.text = _describedItem._item._flavorText;

      _rarityText.text = _rarityStrings[_describedItem._item._rarity].ToUpper();
      _rarityText.color = _rarityColors[_describedItem._item._rarity];

      _itemDescription.text = _describedItem._item._description;
      _debuffDescription.text = "";
      _debuffHeader.text = "";
      return;
    }
    else {
      string debuffName = _describedItem._debuff._name.ToUpper();
      string itemName = _describedItem._item._name.ToUpper();

      _nameWithModifierText.text = $"<font={_debuffFontName}>{debuffName}</font> {itemName}";
      _flavorText.text = _describedItem._item._flavorText;

      _rarityText.text = _rarityStrings[_describedItem._item._rarity].ToUpper();
      _rarityText.color = _rarityColors[_describedItem._item._rarity];

      _itemDescription.text = _describedItem._item._description;
      _debuffHeader.text = "BUT";
      _debuffDescription.text = _describedItem._debuff._description;
    }
  }
}