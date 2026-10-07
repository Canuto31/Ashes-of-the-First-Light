using UnityEngine;

/// <summary>
/// Coordinates uimanager state and operations for the game.
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("HUD")]
    [SerializeField] private LifeHUDController _lifeHUD;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }
    
    public LifeHUDController LifeHUD => _lifeHUD;
}
