using System;
using UnityEngine;
using UnityEngine.UI;

public class SolarHUDController : MonoBehaviour
{
    [Header("Solar Fragments")]
    [SerializeField] private Image[] _solarFragments;

    [Header("Colors")]
    [SerializeField] private Color _fullColor = Color.yellow;
    [SerializeField] private Color _partialColor = new Color(1f, 0.5f, 0f);
    [SerializeField] private Color _emptyColor = Color.grey;

    [Header("Debug")]
    [SerializeField] private float _currentSolarEnergy = 3.5f;

    private PlayerInputHandler _input;

    private const int MAX_FRAGMENTS = 6;

    private void Start()
    {
        _input = FindFirstObjectByType<PlayerInputHandler>();
        UpdateSolarHUD();
    }

    private void Update()
    {
        DebudControls();
    }

    private void DebudControls()
    {
        if (_input.ConsumeSolarEnergyPressed)
        {
            ConsumeSolarEnergy(0.5f);
        }

        if (_input.RestoreSolarEnergyPressed)
        {
            RestoreSolarEnergy(0.5f);
        }
    }

    private void ConsumeSolarEnergy(float amount)
    {
        _currentSolarEnergy -= amount;
        
        _currentSolarEnergy = Mathf.Clamp(_currentSolarEnergy, 0f, MAX_FRAGMENTS);

        UpdateSolarHUD();
    }

    private void RestoreSolarEnergy(float amount)
    {
        _currentSolarEnergy += amount;
        
        _currentSolarEnergy = Mathf.Clamp(_currentSolarEnergy, 0, MAX_FRAGMENTS);

        UpdateSolarHUD();
    }

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
}
