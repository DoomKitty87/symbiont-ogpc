using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPoolSelectionSlotManager : MonoBehaviour
{
  [Header("Slot 1")]
  [SerializeField] private ChangeOpacityElement _fade1;
  [SerializeField] private Image _image1;
  [SerializeField] private TextMeshProUGUI _cost1;
  [SerializeField] private Button _button1;
  [Header("Slot 2")]
  [SerializeField] private ChangeOpacityElement _fade2;
  [SerializeField] private Image _image2;
  [SerializeField] private TextMeshProUGUI _cost2;
  [SerializeField] private Button _button2;
  [Header("Slot 3")]
  [SerializeField] private ChangeOpacityElement _fade3;
  [SerializeField] private Image _image3;
  [SerializeField] private TextMeshProUGUI _cost3;
  [SerializeField] private Button _button3;
  
  public void Initalize() {
    _fade1.OpacityIn(true);
    _fade2.OpacityOut(true);
    _fade3.OpacityOut(true);
  }
  
  public void SetSlot1(PlayerItem playerItem) {
    _image1.sprite = playerItem.item.icon;
    _cost1.text = playerItem.item.cost.ToString();
  }
  public void SetSlot2(PlayerItem playerItem) {
    _image2.sprite = playerItem.item.icon;
    _cost2.text = playerItem.item.cost.ToString();
  }
  public void SetSlot3(PlayerItem playerItem) {
    _image3.sprite = playerItem.item.icon;
    _cost3.text = playerItem.item.cost.ToString();
  }
  
  public void SelectItem1() {
    _fade1.OpacityIn(false);
    _fade2.OpacityOut(false);
    _fade3.OpacityOut(false);
  }
  public void SelectItem2() {
    _fade1.OpacityOut(false);
    _fade2.OpacityIn(false);
    _fade3.OpacityOut(false);
  }
  public void SelectItem3() {
    _fade1.OpacityOut(false);
    _fade2.OpacityOut(false);
    _fade3.OpacityIn(false);
  }
}
