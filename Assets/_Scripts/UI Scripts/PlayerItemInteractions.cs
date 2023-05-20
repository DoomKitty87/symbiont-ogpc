using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemInteractions : MonoBehaviour
{
  //Needs to be integrated with UI to be fully functional
  private PlayerItems playerItems;
  private PlayerTracker playerTracker;
  private int actionsLeft;

  private void Start() {
    playerItems = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PlayerItems>();
    playerTracker = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PlayerTracker>();
  }

  public void ShopScreenTrigger() {
    actionsLeft = 3;
  }

  public PlayerItem[] RollOfferedItems() {
    return playerItems.GetChoiceItems(3);
  }

  public void TryChooseItem(PlayerItem itm) {
    if (playerTracker.GetPoints() < playerItems.CalculatePrice(itm)) return;
    AddItem(itm);
    actionsLeft--;
  }

  public void UpgradeItem(PlayerItem itm) {
    if (playerTracker.GetPoints() < playerItems.CalculateUpgradePrice(itm)) return;
    playerItems.UpgradeItem(itm);
    playerTracker.SpendPoints(playerItems.CalculateUpgradePrice(itm));
  }

  private void ReplaceItem(PlayerItem toReplace, PlayerItem newItem) {
    playerItems.ReplaceItem(toReplace, newItem);
    playerTracker.SpendPoints(playerItems.CalculatePrice(newItem));
  }

  private void AddItem(PlayerItem itm) {
    playerItems.ChooseNewItem(itm);
    playerTracker.SpendPoints(playerItems.CalculatePrice(itm));
  }

  public PlayerItem[] GetInventory() {
    return playerItems.GetPlayerItems();
  }
}