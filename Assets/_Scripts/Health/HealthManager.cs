using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnHealthInitializeEvent : UnityEvent<float> {}
[System.Serializable]
public class OnHealthChangedEvent : UnityEvent<float, float, float> {}
public class HealthManager : MonoBehaviour
{
  public float _currentHealth;
  [SerializeField] public float _maxHealth;
  [Tooltip("OnHealthInitialize(_maxHealth)")] public OnHealthInitializeEvent _onHealthInitialize;
  [Tooltip("OnHealthZero()")] public UnityEvent _onHealthZero;
  [Tooltip("OnHealthChanged(healthBeforeDamage, _currentHealth, _maxHealth) Doesn't fire when health changes to a value below zero.")] public OnHealthChangedEvent _onHealthChanged;

  // Start is called before the first frame update
  void Start() {
    _maxHealth = GameObject.FindGameObjectWithTag("Persistent").GetComponent<FloorManager>().GetRandEnemyHealth();
    _currentHealth = _maxHealth;
    _onHealthInitialize?.Invoke(_maxHealth);
  }
  public void Damage(float damagePoints) {
    if (_currentHealth - damagePoints <= 0) {
      _onHealthZero?.Invoke();
      if (gameObject == GameObject.FindGameObjectWithTag("PlayerHolder").GetComponent<ViewSwitcher>()._currentObjectInhabiting) GameObject.FindGameObjectWithTag("Persistent").GetComponent<FloorManager>().LoseState();
      this.enabled = false;
    }
    else {
      float initialHealth = _currentHealth;
      _currentHealth -= damagePoints;
      _onHealthChanged?.Invoke(initialHealth, _currentHealth, _maxHealth);
    }
  }
}
