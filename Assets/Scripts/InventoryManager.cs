using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

  private Transform inventory;
  private Transform gunHolder;
  private Transform attachmentHolder;

  [SerializeField] private GameObject HUD;

  void Start()
  {
    inventory = transform.GetChild(3).GetChild(1);
    gunHolder = inventory.GetChild(0).GetChild(0);
    attachmentHolder = inventory.GetChild(0).GetChild(2);
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Tab)) ToggleInventory();
  }

  public void ToggleInventory() {
    inventory.gameObject.SetActive(inventory.gameObject.activeInHierarchy ^ true);
    HUD.SetActive(HUD.activeInHierarchy ^ true);
    if (!inventory.gameObject.activeInHierarchy) {
      Time.timeScale = 1f;
      return;
    }
    Time.timeScale = 0f;
    gunHolder.GetChild(0).gameObject.SetActive(false);
    gunHolder.GetChild(1).gameObject.SetActive(false);
    gunHolder.GetChild(2).gameObject.SetActive(false);
    attachmentHolder.GetChild(0).gameObject.SetActive(false);
    gunHolder.GetChild(GetComponent<GunController>().GetActiveGun()).gameObject.SetActive(true);
    for (int i = 0; i < GetComponent<GunController>().GetActiveAttachments().Count; i++) {
      attachmentHolder.GetChild(i).gameObject.SetActive(true);
    }
  }
}
