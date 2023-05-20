using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentItemStats : MonoBehaviour
{

  public void RefreshItems() {
    PlayerItems playerInv = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PlayerItems>();
  }
}