using UnityEngine;
using UnityEngine.UI;

public class StaminaHUDController : BaseHUDModule
{
    [SerializeField] private Image _staminaFill;

    [SerializeField] private float smoothSpeed = 10f;

    private float _targetFill;

    private PlayerStamina _playerStamina;

    protected override void Awake()
    {
        base.Awake();

        _playerStamina = FindFirstObjectByType<PlayerStamina>();
    }

    private void Start()
    {
        if (_playerStamina != null)
        {
            _playerStamina.OnStaminaChanged += UpdateStamina;

            UpdateStamina(_playerStamina.CurrentStamina, _playerStamina.MaxStamina);
        }
    }

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

    private void OnDestroy()
    {
        if (_playerStamina != null)
            _playerStamina.OnStaminaChanged -= UpdateStamina;
    }

    private void UpdateStamina(float current, float max)
    {
        _targetFill = max > 0f ? Mathf.Clamp01(current / max) : 0f;
    }
}
