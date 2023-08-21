using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentItemStats : MonoBehaviour
{

  [SerializeField] private Image damageMult, damageOnLeave, maxHealthIncrease, healOnTeleport, dps, teleportSpeed;
  [SerializeField] private AnimationCurve _easeCurve;

  public void RefreshItems() {
    StopAllCoroutines();
    StartCoroutine(LerpItemValues());
  }

  private IEnumerator LerpItemValues() {
    PlayerItemsHandler playerInv = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PlayerItemsHandler>();
    float dmgm = playerInv.GetDamageMult() / 3;
    float dmgl = playerInv.GetLeaveDamage() / 50f;
    float mxhlt = playerInv.GetMaxHealthIncrease() / 100f;
    float hlontp = playerInv.GetHealOnTeleport() / 50f;
    float dpsv = playerInv.GetDPS() / 5f;
    float tpspd = 1f / playerInv.GetTeleportSpeed() / 3f;

    float initdmgm = damageMult.fillAmount;
    float initdmgl = damageOnLeave.fillAmount;
    float initmxhlt = maxHealthIncrease.fillAmount;
    float inithlontp = healOnTeleport.fillAmount;
    float initdpsv = dps.fillAmount;
    float inittpspd = teleportSpeed.fillAmount;

    float duration = 0.3f;

    float timeElapsed = 0;
    while (timeElapsed <= duration) {
      damageMult.fillAmount = Mathf.Lerp(initdmgm, dmgm, _easeCurve.Evaluate(timeElapsed / duration));
      damageOnLeave.fillAmount = Mathf.Lerp(initdmgl, dmgl, _easeCurve.Evaluate(timeElapsed / duration));
      maxHealthIncrease.fillAmount = Mathf.Lerp(initmxhlt, mxhlt, _easeCurve.Evaluate(timeElapsed / duration));
      healOnTeleport.fillAmount = Mathf.Lerp(inithlontp, hlontp, _easeCurve.Evaluate(timeElapsed / duration));
      dps.fillAmount = Mathf.Lerp(initdpsv, dpsv, _easeCurve.Evaluate(timeElapsed / duration));
      teleportSpeed.fillAmount = Mathf.Lerp(inittpspd, tpspd, _easeCurve.Evaluate(timeElapsed / duration));
      timeElapsed += Time.deltaTime;
      yield return null;
    }
  }
}