using System;
using UnityEngine;
using UnityEngine.UI;

public class LifeHUDController : BaseHUDModule
{
    [Header("References")]
    [SerializeField] private Image _lifeBarFill;
    [SerializeField] private Image _damageFlash;

    [Header("Damage Flash")] 
    [SerializeField] private float _flashFadeSpeed = 5f;
    [SerializeField] private float _flashAlpha = 0.7f;
    
    private PlayerHealth _playerHealth;
    
    private bool _isFlashing;

    protected override void Awake()
    {
        base.Awake();
        
        _playerHealth = FindFirstObjectByType<PlayerHealth>();
    }
    
    private void Start()
    {
        if (_damageFlash != null)
        {
            Color color = _damageFlash.color;
            color.a = 0;
            _damageFlash.color = color;
        }
        
        if (_playerHealth != null)
        {
            _playerHealth.OnHealthChanged += UpdateLife;
            _playerHealth.OnDamageTaken += ShowDamageFlash;
            
            UpdateLife(_playerHealth.CurrentHealth, _playerHealth.MaxHealth);
        }
    }

    private void Update()
    {
        HandleDamageFlashFade();
    }

    private void OnDestroy()
    {
        if (_playerHealth != null)
        {
            _playerHealth.OnHealthChanged -= UpdateLife;
            _playerHealth.OnDamageTaken -= ShowDamageFlash;
        }
    }

    public void UpdateLife(float currentLife, float maxLife)
    {
        _lifeBarFill.fillAmount = currentLife / maxLife;
    }

    private void ShowDamageFlash()
    {
        if (_damageFlash == null)
            return;
        
        Color color = _damageFlash.color;
        color.a = _flashAlpha;
        
        _damageFlash.color = color;
        
        _isFlashing = true;
    }

    private void HandleDamageFlashFade()
    {
        if (!_isFlashing || _damageFlash == null)
            return;
        
        Color color = _damageFlash.color;

        color.a = Mathf.Lerp(color.a, 0f, _flashFadeSpeed * Time.deltaTime);
        
        _damageFlash.color = color;

        if (color.a <= 0.01f)
        {
            color.a = 0f;
            _damageFlash.color = color;
            
            _isFlashing = false;
        }
    }
}
