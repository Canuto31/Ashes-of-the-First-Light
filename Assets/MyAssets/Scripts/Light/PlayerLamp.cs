using UnityEngine;

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
        if (_input.ToggleLanternPressed)
        {
            ToggleLight();
        }

        // TEST (puedes quitar luego)
        if (_input.InteractPressed)
        {
            IncreaseLight(1f);
        }
    }

    private void ToggleLight()
    {
        _isLightOn = !_isLightOn;
        _lightTransform.gameObject.SetActive(_isLightOn);
    }

    public void IncreaseLight(float amount)
    {
        _currentScale += amount;
        _currentScale = Mathf.Clamp(_currentScale, _minScale, _maxScale);

        UpdateLight();
    }

    private void UpdateLight()
    {
        _lightTransform.localScale = new Vector3(_currentScale, _currentScale, 1f);
    }

    // 🔥 EXPONER DATOS PARA OTROS SISTEMAS

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
        return _lightTransform.position;
    }

    // 🔵 DEBUG VISUAL

    private void OnDrawGizmos()
    {
        if (_lightTransform == null) return;

        Gizmos.color = Color.yellow;

        // En editor, usa minScale si no ha iniciado
        float radius = Application.isPlaying ? _currentScale : _minScale;

        Gizmos.DrawWireSphere(_lightTransform.position, radius);
    }
}