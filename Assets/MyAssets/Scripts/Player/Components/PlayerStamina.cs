using System;
using UnityEngine;

/// <summary>
/// Provides the runtime behavior and data owned by the player stamina component.
/// </summary>
public class PlayerStamina : MonoBehaviour
{

    #region Fields and Configuration

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
    
    public bool CanSprint => !_isExhausted;

    public event Action<float, float> OnStaminaChanged;


    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Initializes runtime state after all scene objects have completed their Awake phase.
    /// </summary>
    private void Start()
    {
        _input = FindFirstObjectByType<PlayerInputHandler>();
        
        _currentStamina = _maxstamina;

        NotifyStaminaChanged();
    }

    /// <summary>
    /// Coordinates frame-based input and state updates for this component.
    /// </summary>
    private void Update()
    {
        HandleStamina();
    }


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Processes stamina for this component.
    /// </summary>
    private void HandleStamina()
    {
        if (_input == null)
            return;

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

    /// <summary>
    /// Consumes stamina for this component.
    /// </summary>
    public void DrainStamina(float amount)
    {
        if (amount <= 0f)
            return;

        _currentStamina = Mathf.Max(0f, _currentStamina - amount);

        if (Mathf.Approximately(_currentStamina, 0f))
            _isExhausted = true;

        NotifyStaminaChanged();
    }

    /// <summary>
    /// Regenerates stamina for this component.
    /// </summary>
    public void RegenerateStamina(float amount)
    {
        if (amount <= 0f || Mathf.Approximately(_currentStamina, _maxstamina))
            return;

        _currentStamina = Mathf.Min(_maxstamina, _currentStamina + amount);

        if (_isExhausted)
        {
            float recoveryThreshold =
                _maxstamina * _exhaustRecoveryPercent;

            if (_currentStamina >= recoveryThreshold)
                _isExhausted = false;
        }

        NotifyStaminaChanged();
    }

    /// <summary>
    /// Notifies subscribers about stamina changed for this component.
    /// </summary>
    private void NotifyStaminaChanged()
    {
        OnStaminaChanged?.Invoke(_currentStamina, _maxstamina);
    }

    #endregion
}
