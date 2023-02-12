using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentData : MonoBehaviour
{

  public string selectedGun;

  void Awake() {
    DontDestroyOnLoad(this.gameObject);
  }
}
