using UnityEngine;

/// <summary>
/// Provides the runtime behavior and data owned by the player lamp component.
/// </summary>
public class PlayerLamp : MonoBehaviour
{
    [SerializeField] private Transform _lightTransform;
    [SerializeField] private PlayerInputHandler _input;

    [Header("Light Settings")]
    [SerializeField] private float _minScale = 15f;
    [SerializeField] private float _maxScale = 30f;

    private float _currentScale;
    private bool _isLightOn = true;

    private void Start()
    {
        _currentScale = _minScale;
        UpdateLight();
    }

    private void Update()
    {
        if (_input == null)
            return;

        if (_input.ToggleLanternPressed)
            ToggleLight();

        // Temporary progression test: interaction increases the light radius.
        if (_input.InteractPressed)
            IncreaseLight(1f);
    }

    private void ToggleLight()
    {
        _isLightOn = !_isLightOn;

        if (_lightTransform != null)
            _lightTransform.gameObject.SetActive(_isLightOn);
    }

    public void IncreaseLight(float amount)
    {
        _currentScale = Mathf.Clamp(_currentScale + amount, _minScale, _maxScale);
        UpdateLight();
    }

    private void UpdateLight()
    {
        if (_lightTransform != null)
            _lightTransform.localScale = new Vector3(_currentScale, _currentScale, 1f);
    }

    public bool IsLightOn()
    {
        return _isLightOn;
    }

    public float GetLightRadius()
    {
        return _currentScale;
    }

    public Vector3 GetLightPosition()
    {
        return _lightTransform != null ? _lightTransform.position : transform.position;
    }

    private void OnDrawGizmos()
    {
        if (_lightTransform == null)
            return;

        Gizmos.color = Color.yellow;

        float radius = Application.isPlaying ? _currentScale : _minScale;
        Gizmos.DrawWireSphere(_lightTransform.position, radius);
    }
}
