using System;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [Header("Stamina")]
    [SerializeField] private float _maxstamina = 100f;

    [SerializeField] private float _drainRate = 25f;
    [SerializeField] private float _regenRate = 20f;
    [SerializeField] private float _exhaustRecoveryPercent = 0.2f;

    private float _currentStamina;
    private bool _isExhausted;

    private PlayerInputHandler _input;
    
    public float CurrentStamina => _currentStamina;
    public float MaxStamina => _maxstamina;
    public bool IsExhausted => _isExhausted;
    
    public bool CanSprint =>
        !_isExhausted;
    
    
    public event Action<float, float> OnStaminaChanged;

    private void Start()
    {
        _input = FindFirstObjectByType<PlayerInputHandler>();
        
        _currentStamina = _maxstamina;

        NotifyStaminaChanged();
    }

    private void Update()
    {
        HandleStamina();
    }

    private void HandleStamina()
    {
        bool isRunning =
            _input.MoveInput != Vector2.zero &&
            _input.SprintHeld &&
            CanSprint;

        if (isRunning)
        {
            DrainStamina(_drainRate * Time.deltaTime);
        }
        else
        {
            RegenerateStamina(_regenRate * Time.deltaTime);
        }
    }

    public void DrainStamina(float amount)
    {
        _currentStamina -= amount;
        
        if (_currentStamina <= 0)
        {
            _currentStamina = 0;
            _isExhausted = true;
        }

        NotifyStaminaChanged();
    }

    public void RegenerateStamina(float amount)
    {
        _currentStamina += amount;
        
        if (_currentStamina > _maxstamina)
            _currentStamina = _maxstamina;

        if (_isExhausted)
        {
            float recoveryThreshold =
                _maxstamina * _exhaustRecoveryPercent;

            if (_currentStamina >= recoveryThreshold)
            {
                _isExhausted = false;
            }
        }
        
        NotifyStaminaChanged();
    }

    private void NotifyStaminaChanged()
    {
        OnStaminaChanged?.Invoke(_currentStamina, _maxstamina);
    }
}
