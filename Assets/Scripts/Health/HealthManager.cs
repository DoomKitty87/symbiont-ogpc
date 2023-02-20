using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnHealthChangedEvent : UnityEvent<float, float> {}
public class HealthManager : MonoBehaviour
{
    public float _currentHealth;
    [SerializeField] private float _maxHealth;
    public UnityEvent _onHealthInitialize;
    public UnityEvent _onHealthZero;
    public OnHealthChangedEvent _onHealthChanged;

    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = _maxHealth;
        _onHealthInitialize?.Invoke();
    }
    public void Damage(float damagePoints)
    {
        if (_currentHealth - damagePoints <= 0)
        {
            _onHealthZero?.Invoke();
            _currentHealth -= damagePoints;
            _onHealthChanged?.Invoke(_currentHealth, _maxHealth);
            this.enabled = false;
        }
        else
        {
            _currentHealth -= damagePoints;
            _onHealthChanged?.Invoke(_currentHealth, _maxHealth);
        }
    }
}
