using UnityEngine;

/// <summary>
/// Coordinates game state state and operations for the game.
/// </summary>
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public enum GameState
    {
        Playing,
        Tutorial,
        BookMenu
    }

    private GameState _currentState;

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

    private void Start()
    {
        _currentState = GameState.Playing;
    }

    public void SetState(GameState newState)
    {
        if (_currentState == newState)
            return;

        _currentState = newState;
        Debug.Log("Game state changed to: " + _currentState);
    }

    public GameState GetState()
    {
        return _currentState;
    }

    public bool IsPlaying()
    {
        return _currentState == GameState.Playing;
    }
}
