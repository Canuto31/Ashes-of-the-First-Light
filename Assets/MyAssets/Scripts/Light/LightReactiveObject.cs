using System;
using UnityEngine;

public class LightReactiveObject : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _lightTransform;

    [Header("Behavior")]
    [SerializeField] private GameObject _target;
    [SerializeField] private bool _activeInLight = true;
    
    [SerializeField] private PlayerLamp _playerLamp;

    private void Update()
    {
        bool isLightOn = _playerLamp.IsLightOn();
        
        float distance = Vector2.Distance(transform.position, _player.position);

        float lightRadius = _lightTransform.localScale.x;
        bool isInLight = isLightOn && distance <= lightRadius;
        
        if (_activeInLight)
            _target.SetActive(isInLight);
        else
            _target.SetActive(!isInLight);
    }
}
