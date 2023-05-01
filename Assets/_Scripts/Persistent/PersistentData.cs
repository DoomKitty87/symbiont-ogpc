using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GunData;
using static ArmorData;

public class PersistentData : MonoBehaviour
{
  // TODO: Have these be the Weapon Scriptable Object, then have both gun controller and fire raycast take values from them
  public GunData selectedPrimary;
  public GunData selectedSecondary;
  public ArmorData selectedArmor;
  public BotData selectedBotStats;
  public List<Attachment> selectedAttachments = new();
  public List<int> unlockedAttachments = new();

  // Singletoninator
  void Awake() {
    if (GameObject.FindGameObjectsWithTag("Data").Length > 1) Destroy(gameObject);
    DontDestroyOnLoad(gameObject);
  }
}
