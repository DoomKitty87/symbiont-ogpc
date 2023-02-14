using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{

  private Transform inventory;
  private Transform gunHolder;
  private Transform attachmentHolder;
  private GameObject notifContainer;

  [SerializeField] private GameObject HUD;
  [SerializeField] private GameObject itemNotification;

  void Start()
  {
    inventory = transform.GetChild(3).GetChild(1);
    gunHolder = inventory.GetChild(0).GetChild(0);
    attachmentHolder = inventory.GetChild(0).GetChild(2);
    notifContainer = HUD.transform.GetChild(8).gameObject;
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Tab)) ToggleInventory();
  }

  public void PickUpItem(string itemName, float itemAmount) {
    GameObject tmp = Instantiate(itemNotification, notifContainer.transform);
    tmp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+" + itemAmount + " " + itemName;
    tmp.transform.position += new Vector3(0, 60 * notifContainer.transform.childCount - 1, 0);
    StartCoroutine(Notification(tmp));
  }

  private IEnumerator Notification(GameObject tmp) {
    Vector3 init = tmp.transform.localPosition;
    float timer = 0f;
    yield return new WaitForSeconds(1.75f);
    while (timer < 0.25f) {
      tmp.transform.localPosition = Vector3.Lerp(init, new Vector3(init.x + 200, init.y, 0), timer / 0.5f);
      timer += Time.deltaTime;
      yield return null;
    }
    Destroy(tmp);
  }

  public void ToggleInventory() {
    inventory.gameObject.SetActive(inventory.gameObject.activeInHierarchy ^ true);
    HUD.SetActive(HUD.activeInHierarchy ^ true);
    if (!inventory.gameObject.activeInHierarchy) {
      Time.timeScale = 1f;
      return;
    }
    Time.timeScale = 0f;
    for (int i = 0; i < gunHolder.childCount; i++) {
      gunHolder.GetChild(i).gameObject.SetActive(false);
    }
    for (int i = 0; i < attachmentHolder.childCount; i++) {
      attachmentHolder.GetChild(i).gameObject.SetActive(false);
    }
    gunHolder.GetChild(GetComponent<GunController>().GetActiveGun()).gameObject.SetActive(true);
    for (int i = 0; i < GetComponent<GunController>().GetActiveAttachments().Count; i++) {
      attachmentHolder.GetChild(i).gameObject.SetActive(true);
      attachmentHolder.GetChild(i).localPosition += new Vector3(250 * i, 0, 0);
    }
  }
}
