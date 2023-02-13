using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GunData;

public class PersistentData : MonoBehaviour
{

  public GunData selectedGun;
  public List<Attachment> selectedAttachments = new List<Attachment>();
  public List<int> unlockedAttachments = new List<int>();

  void Awake() {
    if (GameObject.FindGameObjectsWithTag("Data").Length > 1) Destroy(this.gameObject);
    DontDestroyOnLoad(this.gameObject);
  }
}
