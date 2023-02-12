using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GunData;

public class PersistentData : MonoBehaviour
{

  public GunData selectedGun;

  void Awake() {
    if (GameObject.FindGameObjectsWithTag("Data").Length > 1) Destroy(this.gameObject);
    DontDestroyOnLoad(this.gameObject);
  }
}
