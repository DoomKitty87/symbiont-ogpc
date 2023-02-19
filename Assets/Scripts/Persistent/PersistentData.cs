using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GunData;
using static ArmorData;

public class PersistentData : MonoBehaviour
{
  // Should store these in the PlayerPrefs -Matthew
  public GunData selectedPrimary;
  public GunData selectedSecondary;
  public ArmorData selectedArmor;
  public List<Attachment> selectedAttachments = new List<Attachment>();
  public List<int> unlockedAttachments = new List<int>();

  void Awake() {
    if (GameObject.FindGameObjectsWithTag("Data").Length > 1) Destroy(this.gameObject);
    DontDestroyOnLoad(this.gameObject);
  }
}
