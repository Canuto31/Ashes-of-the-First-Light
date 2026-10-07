using System;
using UnityEngine;

/// <summary>
/// Provides the runtime behavior and data owned by the player health component.
/// </summary>
public class PlayerHealth : MonoBehaviour
{

    #region Fields and Configuration

    [Header("Health")]
    [SerializeField] private float _maxHealth = 100f;
    
    private PlayerInputHandler _input;
    
    private float _currentHealth;

    public float CurrentHealth => _currentHealth;
    public float MaxHealth => _maxHealth;
    
    public event Action<float, float> OnHealthChanged;
    public event Action OnDeath;
    public event Action OnDamageTaken;


    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Initializes runtime state after all scene objects have completed their Awake phase.
    /// </summary>
    private void Start()
    {
        _input = FindFirstObjectByType<PlayerInputHandler>();
        
        _currentHealth = _maxHealth;

        NotifyHealthChanged();
    }

    /// <summary>
    /// Coordinates frame-based input and state updates for this component.
    /// </summary>
    private void Update()
    {
        HandleDebugInput();
    }


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Processes debug input for this component.
    /// </summary>
    private void HandleDebugInput()
    {
        if (_input == null)
            return;

        if (_input.IncreaseHealthPressed)
            Heal(10);

        if (_input.DecreaseHealthPressed)
            TakeDamage(10);
    }

    /// <summary>
    /// Executes the take damage operation for this component.
    /// </summary>
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

    /// <summary>
    /// Executes the heal operation for this component.
    /// </summary>
    public void Heal(float amount)
    {
        if (amount <= 0f)
            return;

        _currentHealth = Mathf.Min(_maxHealth, _currentHealth + amount);

        NotifyHealthChanged();
    }

    /// <summary>
    /// Notifies subscribers about health changed for this component.
    /// </summary>
    private void NotifyHealthChanged()
    {
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }

    /// <summary>
    /// Executes the die operation for this component.
    /// </summary>
    private void Die()
    {
        Debug.Log("Player Dead");

        OnDeath?.Invoke();
    }

    #endregion
}
