using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GunData;

public class PersistentData : MonoBehaviour
{

  public GunData selectedGun;

  void Awake() {
    DontDestroyOnLoad(this.gameObject);
  }
}
