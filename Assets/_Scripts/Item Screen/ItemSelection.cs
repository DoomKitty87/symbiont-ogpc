using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ItemInstance
{
  PlayerItem
    [Header("Text References")]
  [SerializeField] private TextMeshProUGUI _nameWithModifierText;
  [SerializeField] private TextMeshProUGUI _flavorText;
  [SerializeField] private TextMeshProUGUI _rarityText;
  [SerializeField] private TextMeshProUGUI _itemDescription;
  [SerializeField] private TextMeshProUGUI _debuffDescription;
}


public class ItemSelection : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private GameObject _parentItemLister;

  [Header("Text References")]
  [SerializeField] private TextMeshProUGUI _nameWithModifierText;
  [SerializeField] private TextMeshProUGUI _flavorText;
  [SerializeField] private TextMeshProUGUI _rarityText;
  [SerializeField] private TextMeshProUGUI _itemDescription;
  [SerializeField] private TextMeshProUGUI _debuffDescription;

  [Header("Item List")]
  [SerializeField] private List<PlayerItem> _items = new();
  [SerializeField] private int _selectedItemIndex;

  [Header("Settings")]
  [SerializeField] private string _debuffFontName;
  [SerializeField] private List<string> _rarityStrings;
  [SerializeField] private List<Color> _rarityColors;

  private void Start() {
    if (_parentItemLister == null) {
      Debug.LogError("ItemSelection: Parent Item Lister is null! Please assign it in the inspector!");
    }
  }

  // Helper Functions
  public void SetItems(PlayerItem[] playerItems) {
    _items.Clear();
    foreach (PlayerItem item in playerItems) {
      _items.Add(item);
    }
  }

  public void SelectItem(Button button) {
    _selectedItemIndex = 

    string debuffName = selectedItem.debuff.name.ToUpper();
    string itemName = selectedItem.item.name.ToUpper();

    _nameWithModifierText.text = $"<font={_debuffFontName}>{debuffName}</font>{itemName}";
    _flavorText.text = selectedItem.item.flavorText;

    string[] _rarityStrings = { "Common", "Uncommon", "Rare", "Legendary" };

    _rarityText.text = _rarityStrings[selectedItem.item.rarity].ToUpper();
    _rarityText.color = _rarityColors[selectedItem.item.rarity];

    _itemDescription.text = selectedItem.item.description;
    _debuffDescription.text = selectedItem.debuff.description;
  }

  private GameObject GetItemPrefabThroughButton(Button button) {
    List<GameObject> itemObjects = new();
    for (int i = 0; i < _parentItemLister.transform.childCount; i++) {
      itemObjects.Add(_parentItemLister.transform.GetChild(i).gameObject);
    }
    int j = 0;
    foreach (GameObject itemPrefab in itemObjects) {
      Button prefabButton = itemPrefab.GetComponent<Button>();
      if (prefabButton == button) {
        return itemPrefab;
      }
      j++;
    }
    return null;
  }
}
