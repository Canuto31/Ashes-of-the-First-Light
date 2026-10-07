using System;
using UnityEngine;

/// <summary>
/// Provides the runtime behavior and data owned by the player health component.
/// </summary>
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
        if (amount <= 0f || Mathf.Approximately(_currentHealth, 0f))
            return;

        _currentHealth = Mathf.Max(0f, _currentHealth - amount);

        NotifyHealthChanged();
        OnDamageTaken?.Invoke();

        if (Mathf.Approximately(_currentHealth, 0f))
            Die();
    }

    public void Heal(float amount)
    {
        if (amount <= 0f)
            return;

        _currentHealth = Mathf.Min(_maxHealth, _currentHealth + amount);

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
