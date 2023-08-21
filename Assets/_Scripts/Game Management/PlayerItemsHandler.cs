using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayerItem
{
  public int _level;
  public Debuff _debuff;
  public Item _item;
  // Doubles as upgrade cost and acquire cost
  public int _initCost => CalculatePrice();
  public int _upgradeCost => CalculateUpgradePrice();

  private int CalculatePrice() {
    PlayerItemsHandler itemHandler = PlayerItemsHandler.instance;
    return _debuff._type switch {
      2 => (int)(itemHandler._rarityPrices[_item._rarity] * 0.5f),
      4 => 0,
      _ => itemHandler._rarityPrices[_item._rarity]
    };
  }

  private int CalculateUpgradePrice() {
    PlayerItemsHandler itemHandler = PlayerItemsHandler.instance;
    int price = itemHandler._rarityUpgradePrices[_item._rarity] * _level;
    if (_debuff._type == 2) {
      price *= (int)_debuff._scaleRarities[_item._rarity];
    }
    if (!IsUpgradeable()) {
      return -1;
    }
    return price;
  }

  private bool IsUpgradeable() {
    PlayerItemsHandler itemHandler = PlayerItemsHandler.instance;
    switch (_debuff._type) {
      // ReSharper disable once CompareOfFloatsByEqualityOperator
      case 1 when _level == _debuff._scaleRarities[_item._rarity]:
      case 4:
        return false;
      default:
        return true;
    }
  }
}
[System.Serializable]
public class Debuff
{
  [FormerlySerializedAs("name")] public string _name;
  [FormerlySerializedAs("type")] public int _type;
  [FormerlySerializedAs("description")] public string _description;
  [FormerlySerializedAs("scaleRarities")] public float[] _scaleRarities;
}
[System.Serializable]
public class Item
{
  [FormerlySerializedAs("name")] public string _name;
  [FormerlySerializedAs("type")] public int id;
  [FormerlySerializedAs("rarity")] public int _rarity;
  [FormerlySerializedAs("flavorText")] public string _flavorText;
  [FormerlySerializedAs("description")] public string _description;
  [FormerlySerializedAs("category")] public string _category;
  [FormerlySerializedAs("icon")] public Sprite _icon;
  [FormerlySerializedAs("equation")] public string _equation;
}

// singleton time
public class PlayerItemsHandler : MonoBehaviour
{
  // SINGLETON ==============================
  public static PlayerItemsHandler instance { get; private set; }
  private void Awake() {
    if (instance != null && instance != this) {
      Destroy(this);
    }
    else {
      instance = this;
    }
  }
  // =========================================

  [HideInInspector] public List<PlayerItem> _itemInventory = new();

  [SerializeField] private Debuff[] _debuffs;
  [SerializeField] private Item[] _items;
  [SerializeField] public int[] _rarityPrices;
  [SerializeField] public int[] _rarityUpgradePrices;

  public PlayerItem[] GenerateItems(int amt) {
    PlayerItem[] toReturn = new PlayerItem[amt];
    for (int i = 0; i < amt; i++) {
      PlayerItem newItem = new();
      // No duplicate items
      while (_itemInventory.Where(checkItm => checkItm == newItem).ToList().Count > 0) {
        newItem._level = 1;
        newItem._debuff = _debuffs[Random.Range(0, _debuffs.Length)];
        newItem._item = _items[Random.Range(0, _items.Length)];
      }
      toReturn[i] = newItem;
    }
    return toReturn;
  }

  public void ReplaceItemAtIndex(int replacementIndex, PlayerItem newItem) {
    _itemInventory[replacementIndex] = newItem;
  }
  
  public void UpgradeItem(PlayerItem item) {
    if (!_itemInventory.Contains(item)) return;
    _itemInventory[_itemInventory.IndexOf(item)]._level++;
  }

  public PlayerItem[] GetPlayerItems() {
    return _itemInventory.ToArray();
  }

  public float GetDamageMult() {
    float damageMult = 1;
    foreach (PlayerItem itm in _itemInventory) {
      if (itm._item.id != 3) continue;
      if (itm._debuff._type == 0) damageMult *= 1.1f + ((itm._level - itm._debuff._scaleRarities[itm._item._rarity]) * .05f);
      //else if (itm.debuff.type == 4) damageMult *= 1 + (0.1f / itm.debuff.scaleRarities[itm.item.rarity]);
      else damageMult *= 1.1f + (itm._level * .05f);
    }
    return damageMult;
  }

  public float GetHealOnTeleport() {
    float healValue = 0;
    foreach (PlayerItem itm in _itemInventory) {
      if (itm._item.id != 0) continue;
      if (itm._debuff._type == 0) healValue += 10 + ((itm._level - itm._debuff._scaleRarities[itm._item._rarity]) * 2);
      //else if (itm.debuff.type == 4) healValue += 10 / itm.debuff.scaleRarities[itm.item.rarity];
      else healValue += 10 + (itm._level * 2);
    }
    return healValue;
  }

  public float GetMaxHealthIncrease() {
    float maxHealthIncrease = 0;
    foreach (PlayerItem itm in _itemInventory) {
      if (itm._item.id != 1) continue;
      if (itm._debuff._type == 0) maxHealthIncrease += 20 + ((itm._level - itm._debuff._scaleRarities[itm._item._rarity]) * 5);
      //else if (itm.debuff.type == 4) maxHealthIncrease += 20 / itm.debuff.scaleRarities[itm.item.rarity];
      else maxHealthIncrease += 20 + (itm._level * 5);
    }
    return maxHealthIncrease;
  }

  public float GetLeaveDamage() {
    float onLeaveDamage = 0;
    foreach (PlayerItem itm in _itemInventory) {
      if (itm._item.id != 2) continue;
      if (itm._debuff._type == 0) onLeaveDamage += 10 + ((itm._level - itm._debuff._scaleRarities[itm._item._rarity]) * 3);
      //else if (itm.debuff.type == 4) onLeaveDamage += 10 / itm.debuff.scaleRarities[itm.item.rarity];
      else onLeaveDamage += 10 + (itm._level * 3);
    }
    return onLeaveDamage;
  }

  public float GetDPS() {
    float damagePerSecond = 0;
    foreach (PlayerItem itm in _itemInventory) {
      if (itm._item.id != 4) continue;
      if (itm._debuff._type == 0) damagePerSecond += 1 + ((itm._level - itm._debuff._scaleRarities[itm._item._rarity]) * 3);
      //else if (itm.debuff.type == 4) damagePerSecond += 1 / itm.debuff.scaleRarities[itm.item.rarity];
      else damagePerSecond += 1 + itm._level;
    }
    return damagePerSecond;
  }

  public float GetTeleportSpeed() {
    float teleportSpeed = 1;
    foreach (PlayerItem itm in _itemInventory) {
      if (itm._item.id != 5) continue;
      if (itm._debuff._type == 0) teleportSpeed *= 0.1f + ((itm._level - itm._debuff._scaleRarities[itm._item._rarity]) * 0.05f);
      //else if (itm.debuff.type == 4) teleportSpeed *= 0.1f / itm.debuff.scaleRarities[itm.item.rarity];
      else teleportSpeed *= 0.1f + (itm._level * 0.05f);
    }
    return 1 / teleportSpeed;
  }
}