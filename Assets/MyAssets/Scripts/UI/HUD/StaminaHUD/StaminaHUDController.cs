using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Coordinates stamina hudcontroller behavior and its Unity lifecycle.
/// </summary>
public class StaminaHUDController : BaseHUDModule
{

    #region Fields and Configuration

    [SerializeField] private Image _staminaFill;

    [SerializeField] private float smoothSpeed = 10f;

    private float _targetFill;

    private PlayerStamina _playerStamina;


    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Caches required dependencies and initializes this component before other Unity callbacks run.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        _playerStamina = FindFirstObjectByType<PlayerStamina>();
    }

    /// <summary>
    /// Initializes runtime state after all scene objects have completed their Awake phase.
    /// </summary>
    private void Start()
    {
        if (_playerStamina != null)
        {
            _playerStamina.OnStaminaChanged += UpdateStamina;

            UpdateStamina(_playerStamina.CurrentStamina, _playerStamina.MaxStamina);
        }
    }

    /// <summary>
    /// Coordinates frame-based input and state updates for this component.
    /// </summary>
    private void Update()
    {
        if (_staminaFill == null)
            return;

        _staminaFill.fillAmount = Mathf.Lerp(
            _staminaFill.fillAmount,
            _targetFill,
            smoothSpeed * Time.deltaTime
        );
    }

    /// <summary>
    /// Releases owned resources and event subscriptions before this component is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        if (_playerStamina != null)
            _playerStamina.OnStaminaChanged -= UpdateStamina;
    }


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Refreshes stamina for this component.
    /// </summary>
    private void UpdateStamina(float current, float max)
    {
        _targetFill = max > 0f ? Mathf.Clamp01(current / max) : 0f;
    }

    #endregion
}
