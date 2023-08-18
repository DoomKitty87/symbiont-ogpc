using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemInteractions : MonoBehaviour
{
  //Needs to be integrated with UI to be fully functional
  private PlayerItems playerItems;
  private PlayerTracker playerTracker;

  private void Start() {
    playerItems = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PlayerItems>();
    playerTracker = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PlayerTracker>();
  }

  public PlayerItem[] GenerateOfferedItems() {
    return playerItems.GetChoiceItems(3);
  }

  public void TryChooseItem(PlayerItem itm) {
    if (playerTracker.GetPoints() < playerItems.CalculatePrice(itm)) return;
    AddItem(itm);
  }

  public void UpgradeItem(PlayerItem itm) {
    if (playerTracker.GetPoints() < playerItems.CalculateUpgradePrice(itm)) return;
    playerItems.UpgradeItem(itm);
    playerTracker.SpendPoints(playerItems.CalculateUpgradePrice(itm));
  }

  public void ReplaceItem(PlayerItem toReplace, PlayerItem newItem) {
    playerItems.ReplaceItem(toReplace, newItem);
  }

  public void AddItem(PlayerItem itm) {
    playerItems.ChooseNewItem(itm);
  }

  public PlayerItem[] GetInventory() {
    return playerItems.GetPlayerItems();
  }

  public void RemoveItemByType(PlayerItem itm) {
    playerItems._invItems.Remove(itm);
  }

  public void RemoveItemAtIndex(int index) {
    playerItems._invItems.RemoveAt(index);
  }

  public void AddItemAtIndex(PlayerItem itm, int index) {
    playerItems._invItems.Insert(index, itm);
  }
}