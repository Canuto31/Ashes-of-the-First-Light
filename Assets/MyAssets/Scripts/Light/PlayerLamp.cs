using System;
using UnityEngine;

public class PlayerLamp : MonoBehaviour
{
    [SerializeField] private Transform _lightTransform;
    [SerializeField] private PlayerInputHandler _input;
    
    [Header("Light Settings")]
    [SerializeField] private float _minScale = 5f;
    [SerializeField] private float _maxScale = 10f;

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
        // 🔥 TEST usando tu input system
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
        _lightTransform.localScale = new Vector3(_currentScale, _currentScale, _currentScale);
    }

    public bool IsLightOn()
    {
        return _isLightOn;
    }
}
