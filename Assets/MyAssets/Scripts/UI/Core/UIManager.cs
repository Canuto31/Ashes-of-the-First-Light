using UnityEngine;

/// <summary>
/// Coordinates uimanager state and operations for the game.
/// </summary>
public class UIManager : MonoBehaviour
{

    #region Fields and Configuration

    public static UIManager Instance { get; private set; }

    [Header("HUD")]
    [SerializeField] private LifeHUDController _lifeHUD;


    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Caches required dependencies and initializes this component before other Unity callbacks run.
    /// </summary>
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

    #endregion
}
