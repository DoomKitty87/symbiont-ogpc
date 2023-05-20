using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentItemStats : MonoBehaviour
{

  [SerializeField] private Image damageMult, damageOnLeave, maxHealthIncrease, healOnTeleport, dps, teleportSpeed;

  public void RefreshItems() {
    PlayerItems playerInv = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PlayerItems>();
    damageMult.fillAmount = playerInv.GetDamageMult() / 3;
    damageOnLeave.fillAmount = playerInv.GetLeaveDamage() / 50f;
    maxHealthIncrease.fillAmount = playerInv.GetMaxHealthIncrease() / 100f;
    healOnTeleport.fillAmount = playerInv.GetHealOnTeleport() / 50f;
    dps.fillAmount = playerInv.GetDPS() / 5f;
    teleportSpeed.fillAmount = 1f / playerInv.GetTeleportSpeed() / 3f;
  }
}