using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemInteractions : MonoBehaviour
{
  //Needs to be integrated with UI to be fully functional
  private PlayerItemsHandler _itemHandler = PlayerItemsHandler.instance;
  private PlayerTracker _playerTracker;

  private void Start() {
    _itemHandler = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PlayerItemsHandler>();
    _playerTracker = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PlayerTracker>();
  }

  public PlayerItem[] GenerateOfferedItems(int amount) {
    return _itemHandler.GenerateItems(amount);
  }

  public bool CanAddItem(PlayerItem itm) {
    if (_playerTracker.GetPoints() < itm._initCost) return false;
    return true;
  }

  public void UpgradeItem(PlayerItem itm) {
    if (_playerTracker.GetPoints() < itm._upgradeCost) return;
    _itemHandler.UpgradeItem(itm);
    _playerTracker.SpendPoints(itm._upgradeCost);
  }

  public void ReplaceItemAtIndex(int index, PlayerItem newItem) {
    _itemHandler.ReplaceItemAtIndex(index, newItem);
  }

  public PlayerItem[] GetInventory() {
    return _itemHandler.GetPlayerItems();
  }

  public void RemoveItemByType(PlayerItem itm) {
    _itemHandler._itemInventory.Remove(itm);
  }
  public void AddItemAtIndex(PlayerItem itm, int index) {
    _itemHandler._itemInventory.Insert(index, itm);
  }

  public void RemoveItemAtIndex(int index) {
    _itemHandler._itemInventory.RemoveAt(index);
  }

}