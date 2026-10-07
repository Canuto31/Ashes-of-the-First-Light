using UnityEngine;

/// <summary>
/// Provides the runtime behavior and data owned by the light reactive object component.
/// </summary>
public class LightReactiveObject : MonoBehaviour
{

    #region Fields and Configuration

    [Header("References")]
    [SerializeField] private PlayerLamp _playerLamp;

    [Header("Targets")]
    [SerializeField] private GameObject[] _targets;

    [Header("Behavior")]
    [SerializeField] private bool _activeInLight = true;


    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Coordinates frame-based input and state updates for this component.
    /// </summary>
    private void Update()
    {
        RefreshTargetStates();
    }


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Rebuilds target states for this component.
    /// </summary>
    private void RefreshTargetStates()
    {
        if (!HasValidConfiguration())
            return;

        SetTargetsActive(ShouldTargetsBeActive());
    }

    /// <summary>
    /// Returns whether valid configuration is currently true.
    /// </summary>
    private bool HasValidConfiguration()
    {
        return _playerLamp != null && _targets != null && _targets.Length > 0;
    }

    /// <summary>
    /// Executes the should targets be active operation for this component.
    /// </summary>
    private bool ShouldTargetsBeActive()
    {
        if (!_playerLamp.IsLightOn())
            return !_activeInLight;

        bool anyTargetInLight = IsAnyTargetInLight();
        return _activeInLight ? anyTargetInLight : !anyTargetInLight;
    }

    /// <summary>
    /// Returns whether any target in light is currently true.
    /// </summary>
    private bool IsAnyTargetInLight()
    {
        Vector3 lightPosition = _playerLamp.GetLightPosition();
        float radius = _playerLamp.GetLightRadius();

        foreach (GameObject target in _targets)
        {
            if (target == null)
                continue;

            float distance = Vector2.Distance(target.transform.position, lightPosition);
            if (distance <= radius)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Updates targets active for this component.
    /// </summary>
    private void SetTargetsActive(bool state)
    {
        foreach (GameObject target in _targets)
        {
            if (target != null && target.activeSelf != state)
                target.SetActive(state);
        }
    }


    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Draws editor-only gizmos that visualize this component's configured range.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (_targets == null)
            return;

        Gizmos.color = _activeInLight ? Color.green : Color.red;

        foreach (GameObject target in _targets)
        {
            if (target != null)
                Gizmos.DrawWireSphere(target.transform.position, 0.3f);
        }
    }

    #endregion
}
