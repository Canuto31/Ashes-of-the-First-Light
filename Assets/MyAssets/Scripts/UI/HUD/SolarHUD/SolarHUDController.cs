using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Coordinates solar hudcontroller behavior and its Unity lifecycle.
/// </summary>
public class SolarHUDController : MonoBehaviour
{

    #region Fields and Configuration

    [Header("Solar Fragments")]
    [SerializeField] private Image[] _solarFragments;

    [Header("Colors")]
    [SerializeField] private Color _fullColor = Color.yellow;
    [SerializeField] private Color _partialColor = new Color(1f, 0.5f, 0f);
    [SerializeField] private Color _emptyColor = Color.grey;

    [Header("Debug")]
    [SerializeField] private float _currentSolarEnergy = 3.5f;

    private PlayerInputHandler _input;

    private const int MaxFragments = 6;


    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Initializes runtime state after all scene objects have completed their Awake phase.
    /// </summary>
    private void Start()
    {
        _input = FindFirstObjectByType<PlayerInputHandler>();
        UpdateSolarHUD();
    }

    /// <summary>
    /// Coordinates frame-based input and state updates for this component.
    /// </summary>
    private void Update()
    {
        HandleDebugControls();
    }


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Processes debug controls for this component.
    /// </summary>
    private void HandleDebugControls()
    {
        if (_input == null)
            return;

        if (_input.ConsumeSolarEnergyPressed)
        {
            ConsumeSolarEnergy(0.5f);
        }

        if (_input.RestoreSolarEnergyPressed)
        {
            RestoreSolarEnergy(0.5f);
        }
    }

    /// <summary>
    /// Executes the consume solar energy operation for this component.
    /// </summary>
    private void ConsumeSolarEnergy(float amount)
    {
        _currentSolarEnergy = Mathf.Clamp(_currentSolarEnergy - amount, 0f, MaxFragments);

        UpdateSolarHUD();
    }

    /// <summary>
    /// Executes the restore solar energy operation for this component.
    /// </summary>
    private void RestoreSolarEnergy(float amount)
    {
        _currentSolarEnergy = Mathf.Clamp(_currentSolarEnergy + amount, 0f, MaxFragments);

        UpdateSolarHUD();
    }

    /// <summary>
    /// Refreshes solar hud for this component.
    /// </summary>
    private void UpdateSolarHUD()
    {
        for (int i = 0; i < _solarFragments.Length; i++)
        {
            float fragmentValue = _currentSolarEnergy - i;

            if (fragmentValue >= 1)
            {
                _solarFragments[i].color = _fullColor;
            }
            else if (fragmentValue >= 0.5f)
            {
                _solarFragments[i].color = _partialColor;
            }
            else
            {
                _solarFragments[i].color = _emptyColor;
            }
        }
    }

    #endregion
}
