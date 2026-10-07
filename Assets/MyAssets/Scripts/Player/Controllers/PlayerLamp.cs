using UnityEngine;

/// <summary>
/// Provides the runtime behavior and data owned by the player lamp component.
/// </summary>
public class PlayerLamp : MonoBehaviour
{

    #region Fields and Configuration

    [SerializeField] private Transform _lightTransform;
    [SerializeField] private PlayerInputHandler _input;

    [Header("Light Settings")]
    [SerializeField] private float _minScale = 15f;
    [SerializeField] private float _maxScale = 30f;

    private float _currentScale;
    private bool _isLightOn = true;


    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Initializes runtime state after all scene objects have completed their Awake phase.
    /// </summary>
    private void Start()
    {
        _currentScale = _minScale;
        UpdateLight();
    }

    /// <summary>
    /// Coordinates frame-based input and state updates for this component.
    /// </summary>
    private void Update()
    {
        HandleInput();
    }


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Processes input for this component.
    /// </summary>
    private void HandleInput()
    {
        if (_input == null)
            return;

        if (_input.ToggleLanternPressed)
            ToggleLight();

        // Temporary progression test: interaction increases the light radius.
        if (_input.InteractPressed)
            IncreaseLight(1f);
    }

    /// <summary>
    /// Executes the toggle light operation for this component.
    /// </summary>
    private void ToggleLight()
    {
        _isLightOn = !_isLightOn;

        if (_lightTransform != null)
            _lightTransform.gameObject.SetActive(_isLightOn);
    }

    /// <summary>
    /// Executes the increase light operation for this component.
    /// </summary>
    public void IncreaseLight(float amount)
    {
        _currentScale = Mathf.Clamp(_currentScale + amount, _minScale, _maxScale);
        UpdateLight();
    }

    /// <summary>
    /// Refreshes light for this component.
    /// </summary>
    private void UpdateLight()
    {
        if (_lightTransform != null)
            _lightTransform.localScale = new Vector3(_currentScale, _currentScale, 1f);
    }

    /// <summary>
    /// Returns whether light on is currently true.
    /// </summary>
    public bool IsLightOn()
    {
        return _isLightOn;
    }

    /// <summary>
    /// Returns the current light radius for this component.
    /// </summary>
    public float GetLightRadius()
    {
        return _currentScale;
    }

    /// <summary>
    /// Returns the current light position for this component.
    /// </summary>
    public Vector3 GetLightPosition()
    {
        return _lightTransform != null ? _lightTransform.position : transform.position;
    }


    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Draws editor-only gizmos that visualize this component's configured range.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (_lightTransform == null)
            return;

        Gizmos.color = Color.yellow;

        float radius = Application.isPlaying ? _currentScale : _minScale;
        Gizmos.DrawWireSphere(_lightTransform.position, radius);
    }

    #endregion
}
