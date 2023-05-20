using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySpeedCharge : MonoBehaviour
{

  [SerializeField] private Image _rechargeBar;
  
  private SpeedUpDisplay _speedHandler;

  private void Start() {
    _speedHandler = GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<SpeedUpDisplay>();
  }

  private void Update() {
    _rechargeBar.fillAmount = _speedHandler._rechargeAmt / _speedHandler._timeToCharge;
  }
}