using UnityEngine;

/// <summary>
/// Provides the runtime behavior and data owned by the light reactive object component.
/// </summary>
public class LightReactiveObject : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerLamp _playerLamp;

    [Header("Targets")]
    [SerializeField] private GameObject[] _targets;

    [Header("Behavior")]
    [SerializeField] private bool _activeInLight = true;

    private void Update()
    {
        RefreshTargetStates();
    }

    private void RefreshTargetStates()
    {
        if (!HasValidConfiguration())
            return;

        SetTargetsActive(ShouldTargetsBeActive());
    }

    private bool HasValidConfiguration()
    {
        return _playerLamp != null && _targets != null && _targets.Length > 0;
    }

    private bool ShouldTargetsBeActive()
    {
        if (!_playerLamp.IsLightOn())
            return !_activeInLight;

        bool anyTargetInLight = IsAnyTargetInLight();
        return _activeInLight ? anyTargetInLight : !anyTargetInLight;
    }

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

    private void SetTargetsActive(bool state)
    {
        foreach (GameObject target in _targets)
        {
            if (target != null && target.activeSelf != state)
                target.SetActive(state);
        }
    }

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
}
