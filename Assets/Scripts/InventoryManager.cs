using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

  private Transform inventory;

  [SerializeField] private GameObject HUD;

  void Start()
  {
    inventory = transform.GetChild(3).GetChild(1);
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Tab)) ToggleInventory();
  }

  public void ToggleInventory() {
    inventory.gameObject.SetActive(inventory.gameObject.activeInHierarchy ^ true);
    HUD.SetActive(HUD.activeInHierarchy ^ true);
    if (!inventory.gameObject.activeInHierarchy) return;
    inventory.GetChild(0).GetChild(0).gameObject.SetActive(false);
    inventory.GetChild(0).GetChild(1).gameObject.SetActive(false);
    inventory.GetChild(0).GetChild(2).gameObject.SetActive(false);
    inventory.GetChild(0).GetChild(GetComponent<GunController>().GetActiveGun()).gameObject.SetActive(true);
  }
}
