using UnityEngine;

/// <summary>
/// Coordinates game state state and operations for the game.
/// </summary>
public class GameStateManager : MonoBehaviour
{

    #region Fields and Configuration

    public static GameStateManager Instance { get; private set; }

    public enum GameState
    {
        Playing,
        Tutorial,
        BookMenu
    }

    private GameState _currentState;


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
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Initializes runtime state after all scene objects have completed their Awake phase.
    /// </summary>
    private void Start()
    {
        _currentState = GameState.Playing;
    }


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Updates state for this component.
    /// </summary>
    public void SetState(GameState newState)
    {
        if (_currentState == newState)
            return;

        _currentState = newState;
        Debug.Log("Game state changed to: " + _currentState);
    }

    /// <summary>
    /// Returns the current state for this component.
    /// </summary>
    public GameState GetState()
    {
        return _currentState;
    }

    /// <summary>
    /// Returns whether playing is currently true.
    /// </summary>
    public bool IsPlaying()
    {
        return _currentState == GameState.Playing;
    }

    #endregion
}
