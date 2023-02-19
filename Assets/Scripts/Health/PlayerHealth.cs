using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{

  private float health;

  [SerializeField] private GameObject healthDisplay;

  void Start()
  {
    health = 25;
    healthDisplay.GetComponent<TextMeshProUGUI>().text = health.ToString();
  }

  void Update()
  {
        
  }

  public void DamagePlayer(float damage) {
    health -= damage;
    healthDisplay.GetComponent<TextMeshProUGUI>().text = health.ToString();
    if (health <= 0) GetComponent<SubmitScores>().EndLevel();
  }
}
