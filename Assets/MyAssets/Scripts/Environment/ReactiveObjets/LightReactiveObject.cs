using UnityEngine;

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
        if (_playerLamp == null || _targets == null || _targets.Length == 0) return;

        bool shouldBeActive;

        // Si la luz está apagada → todo es oscuridad
        if (!_playerLamp.IsLightOn())
        {
            shouldBeActive = !_activeInLight;
        }
        else
        {
            bool anyInLight = IsAnyTargetInLight();
            shouldBeActive = _activeInLight ? anyInLight : !anyInLight;
        }

        SetTargetsActive(shouldBeActive);
    }

    private bool IsAnyTargetInLight()
    {
        Vector3 lightPos = _playerLamp.GetLightPosition();
        float radius = _playerLamp.GetLightRadius();

        foreach (var target in _targets)
        {
            if (target == null) continue;

            float distance = Vector2.Distance(
                target.transform.position,
                lightPos
            );

            if (distance <= radius)
                return true; // 🔥 ya con uno basta
        }

        return false;
    }

    private void SetTargetsActive(bool state)
    {
        foreach (var target in _targets)
        {
            if (target != null && target.activeSelf != state)
            {
                target.SetActive(state);
            }
        }
    }

    // 🔵 DEBUG VISUAL
    private void OnDrawGizmos()
    {
        if (_targets == null) return;

        Gizmos.color = _activeInLight ? Color.green : Color.red;

        foreach (var target in _targets)
        {
            if (target != null)
            {
                Gizmos.DrawWireSphere(target.transform.position, 0.3f);
            }
        }
    }
}