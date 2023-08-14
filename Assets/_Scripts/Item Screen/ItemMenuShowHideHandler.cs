using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemMenuShowHideHandler : MonoBehaviour
{
  // Gets called by terminal handler to initiate the item menu
  // Precondition: The terminal handler has to be a child of the room
  // This script should be attached to the item menu
  // and a reference needs to be set up in the terminal handler
  // (Probably get gameObject by tag)
  // 
  // When this class is called, it should:
  // 1. Disable the player - DONE (ext)
  // 2. Pause the game - DONE (ext)
  // 3. Fade in the item menu 
  // 4. Call the needed scripts to set up the selection menu
  // 5. Once the selection menu has timed out, fade out the item menu
  // 6. Fade in the inventory menu
  // 7. Call the needed scripts to set up the inventory menu
  // 8. Once the inventory menu has timed out, fade out the inventory menu
  // 9. Switch to the next floor (ask ansel about this)
  [Header("Script References")]
  private PauseHandler _pauseHandler;

  [Header("UI References")]
  [SerializeField] private FadeElementInOut _itemMenuFade;
  [Header("Item Selection Menu References")]
  [SerializeField] private ItemSelectionInit _itemSelectionInit;
  [SerializeField] private CustomTimer _itemSelectionInitTimer;
  // [Header("Inventory References")]

  private bool _onItemScreen;

  private void Start() {
    _pauseHandler = GameObject.FindGameObjectWithTag("Handler").GetComponent<PauseHandler>();
    if (_pauseHandler == null) {
      Debug.LogError("ItemMenuOnFloorEnd: Cannot find the PauseHandler in the Handler Object!");
    }
    gameObject.GetComponent<Canvas>().enabled = false;
    _onItemScreen = false;
  }

  // If Update is still here after the menu is finished, remove it, it's only for testing.
  private void Update() {
    if (Input.GetKeyDown(KeyCode.Home) && _onItemScreen == false) {
      ClearedFloor();
    }
  }

  public void ClearedFloor() {
    _onItemScreen = true;
    _pauseHandler._disablePauseKeycodes = true;
    _pauseHandler.Pause(false, false);
    gameObject.GetComponent<Canvas>().enabled = true;
    _itemMenuFade.FadeIn(true);
    _itemSelectionInit.Initalize();
    _itemSelectionInitTimer.StartTimer();
  }

  private void OnItemSelectionCompleted() {
    gameObject.GetComponent<Canvas>().enabled = false;
    _onItemScreen = false;
    GameObject.FindGameObjectWithTag("Persistent").GetComponent<FloorManager>().ClearedFloor();
  }
}
