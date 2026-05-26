using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float _maxHealth = 100f;
    
    private PlayerInputHandler _input;
    
    private float _currentHealth;
    
    public float CurrentHealth => _currentHealth;
    public float MaxHealth => _maxHealth;
    
    public event Action<float, float> OnHealthChanged;
    public event Action OnDeath;
    public event Action OnDamageTaken;

    private void Start()
    {
        _input = FindFirstObjectByType<PlayerInputHandler>();
        
        _currentHealth = _maxHealth;

        NotifyHealthChanged();
    }

    private void Update()
    {
        if (_input == null)
            return;
        
        if (_input.IncreaseHealthPressed)
            Heal(10);
        
        if (_input.DecreaseHealthPressed)
            TakeDamage(10);
    }

    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;
        
        if (_currentHealth < 0f)
            _currentHealth = 0f;

        NotifyHealthChanged();
        OnDamageTaken?.Invoke();

        if (_currentHealth <= 0f)
            Die();
    }

    public void Heal(float amount)
    {
        _currentHealth += amount;
        
        if (_currentHealth > _maxHealth)
            _currentHealth = _maxHealth;

        NotifyHealthChanged();
    }

    private void NotifyHealthChanged()
    {
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }

    private void Die()
    {
        Debug.Log("Player Dead");
        
        OnDeath?.Invoke();
    }
}
