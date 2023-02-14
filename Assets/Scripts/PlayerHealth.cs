using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

  private float health;

  void Start()
  {
    health = 10;
  }

  void Update()
  {
        
  }

  public void DamagePlayer(float damage) {
    health -= damage;
  }
}
