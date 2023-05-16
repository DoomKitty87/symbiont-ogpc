
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerItem
{

  public int level;
  public Debuff debuff;
  public Item item;
}

[System.Serializable]
public class Debuff
{

  public string name;
  public int type;
  public float[] scaleRarities;
}

[System.Serializable]
public class Item
{

  public string name;
  public int type;
  public int rarity;
  public string flavorText;
  public string description;
  public string category;
}

public class PlayerItems : MonoBehaviour
{

  [HideInInspector] public List<PlayerItem> _invItems = new List<PlayerItem>();

  [SerializeField] private Debuff[] _debuffs;
  [SerializeField] private Item[] _items;
  [SerializeField] private int[] _rarityPrices;
  [SerializeField] private int[] _rarityUpgradePrices;

  public PlayerItem[] GetChoiceItems(int amt) {
    PlayerItem[] toReturn = new PlayerItem[amt];
    for (int i = 0; i < amt; i++) {
      PlayerItem itm = new PlayerItem();
      itm.level = 1;
      itm.debuff = _debuffs[Random.Range(0, _debuffs.Length)];
      itm.item = _items[Random.Range(0, _items.Length)];
      while (_invItems.Where(checkItm => checkItm.item == itm.item).ToList().Count > 0) {
        itm.item = _items[Random.Range(0, _items.Length)];
      }
      toReturn[i] = itm;
    }
    return toReturn;
  }

  public void ChooseNewItem(PlayerItem itm) {
    if (_invItems.Count < 5) _invItems.Add(itm);
  }

  public void ReplaceItem(PlayerItem itm, PlayerItem newItem) {
    _invItems.Remove(itm);
    _invItems.Add(newItem);
  }

  public void UpgradeItem(PlayerItem itm) {
    if (!_invItems.Contains(itm)) return;
    if (itm.debuff.type == 1) {
      if (itm.level == itm.debuff.scaleRarities[itm.item.rarity]) return;
    }
    _invItems[_invItems.IndexOf(itm)].level++;
  }

  public PlayerItem[] GetPlayerItems() {
    return _invItems.ToArray();
  }

  public int CalculatePrice(PlayerItem itm) {
    if (itm.debuff.type == 2) return (int)(_rarityPrices[itm.item.rarity] * 0.5f);
    return _rarityPrices[itm.item.rarity];
  }

  public int CalculateUpgradePrice(PlayerItem itm) {
    int price = _rarityUpgradePrices[itm.item.rarity] * itm.level;
    if (itm.debuff.type == 2) {
      price *= (int)itm.debuff.scaleRarities[itm.item.rarity];
    }
    return price;
  }

  public float GetDamageMult() {
    float damageMult = 1;
    foreach (PlayerItem itm in _invItems) {
      if (itm.item.type != 3) continue;
      if (itm.debuff.type == 0) damageMult *= 1.1f + ((itm.level - itm.debuff.scaleRarities[itm.item.rarity]) * .05f);
      else if (itm.debuff.type == 4) damageMult *= 1 + (0.1f / itm.debuff.scaleRarities[itm.item.rarity]);
      else damageMult *= 1.1f + (itm.level * .05f);
    }
    return damageMult;
  }

  public float GetHealOnTeleport() {
    float healValue = 0;
    foreach (PlayerItem itm in _invItems) {
      if (itm.item.type != 0) continue;
      if (itm.debuff.type == 0) healValue += 10 + ((itm.level - itm.debuff.scaleRarities[itm.item.rarity]) * 2);
      else if (itm.debuff.type == 4) healValue += 10 / itm.debuff.scaleRarities[itm.item.rarity];
      else healValue += 10 + (itm.level * 2);
    }
    return healValue;
  }

  public float GetMaxHealthIncrease() {
    float maxHealthIncrease = 0;
    foreach (PlayerItem itm in _invItems) {
      if (itm.item.type != 1) continue;
      if (itm.debuff.type == 0) maxHealthIncrease += 20 + ((itm.level - itm.debuff.scaleRarities[itm.item.rarity]) * 5);
      else if (itm.debuff.type == 4) maxHealthIncrease += 20 / itm.debuff.scaleRarities[itm.item.rarity];
      else maxHealthIncrease += 20 + (itm.level * 5);
    }
    return maxHealthIncrease;
  }

  public float GetLeaveDamage() {
    float onLeaveDamage = 0;
    foreach (PlayerItem itm in _invItems) {
      if (itm.item.type != 2) continue;
      if (itm.debuff.type == 0) onLeaveDamage += 10 + ((itm.level - itm.debuff.scaleRarities[itm.item.rarity]) * 3);
      else if (itm.debuff.type == 4) onLeaveDamage += 10 / itm.debuff.scaleRarities[itm.item.rarity];
      else onLeaveDamage += 10 + (itm.level * 3);
    }
    return onLeaveDamage;
  }

  public float GetDPS() {
    float damagePerSecond = 0;
    foreach (PlayerItem itm in _invItems) {
      if (itm.item.type != 4) continue;
      if (itm.debuff.type == 0) damagePerSecond += 1 + ((itm.level - itm.debuff.scaleRarities[itm.item.rarity]) * 3);
      else if (itm.debuff.type == 4) damagePerSecond += 1 / itm.debuff.scaleRarities[itm.item.rarity];
      else damagePerSecond += 1 + itm.level;
    }
    return damagePerSecond;
  }

  public float GetTeleportSpeed() {
    float teleportSpeed = 1;
    foreach (PlayerItem itm in _invItems) {
      if (itm.item.type != 5) continue;
      if (itm.debuff.type == 0) teleportSpeed *= 0.1f + ((itm.level - itm.debuff.scaleRarities[itm.item.rarity]) * 0.05f);
      else if (itm.debuff.type == 4) teleportSpeed *= 0.1f / itm.debuff.scaleRarities[itm.item.rarity];
      else teleportSpeed *= 0.1f + (itm.level * 0.05f);
    }
    return 1 / teleportSpeed;
  }
}