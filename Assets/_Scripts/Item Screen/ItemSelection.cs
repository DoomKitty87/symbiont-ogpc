using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ItemSelection : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private GameObject _parentItemLister;
  [SerializeField] private GameObject _itemPrefab;

  [Header("Text References")]
  [SerializeField] private TextMeshProUGUI _nameWithModifierText;
  [SerializeField] private TextMeshProUGUI _flavorText;
  [SerializeField] private TextMeshProUGUI _rarityText;
  [SerializeField] private TextMeshProUGUI _itemDescription;
  [SerializeField] private TextMeshProUGUI _debuffDescription;

  [Header("Item List")]
  [SerializeField] private List<ItemInstance> _items = new();
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
    for (int i = 0; i < _parentItemLister.transform.childCount; i++) {
      Destroy(_parentItemLister.transform.GetChild(0).gameObject);
    }
    _items.Clear();
    for (int i = 0; i < _items.Count; i++) {
      _items[i]._itemReference = playerItems[i];
      _items[i]._itemImage.sprite = playerItems[i].item.icon;
      _items[i]._itemCostText.text = playerItems[i].item.cost.ToString();
    }
  }

  public void SelectItemByButton(Button button) {

    
  }

  private void SetTextInfo(PlayerItem playerItem) {
    string debuffName = playerItem.debuff.name.ToUpper();
    string itemName = playerItem.item.name.ToUpper();

    _nameWithModifierText.text = $"<font={_debuffFontName}>{debuffName}</font>{itemName}";
    _flavorText.text = playerItem.item.flavorText;

    string[] _rarityStrings = { "Common", "Uncommon", "Rare", "Legendary" };

    _rarityText.text = _rarityStrings[playerItem.item.rarity].ToUpper();
    _rarityText.color = _rarityColors[playerItem.item.rarity];

    _itemDescription.text = playerItem.item.description;
    _debuffDescription.text = playerItem.debuff.description;
  }

  // private GameObject GetItemPrefabThroughButton(Button button) {
  //   List<GameObject> itemObjects = new();
  //   for (int i = 0; i < _parentItemLister.transform.childCount; i++) {
  //     itemObjects.Add(_parentItemLister.transform.GetChild(i).gameObject);
  //   }
  //   int j = 0;
  //   foreach (GameObject itemPrefab in itemObjects) {
  //     Button prefabButton = itemPrefab.GetComponent<Button>();
  //     if (prefabButton == button) {
  //       return itemPrefab;
  //     }
  //     j++;
  //   }
  //   return null;
  // }
}
