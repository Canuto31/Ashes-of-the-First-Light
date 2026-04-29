using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public enum GameState
    {
        Playing,
        Tutorial,
        ReadingMenu,
        Menu
    }

    private GameState _currentState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _currentState = GameState.Playing;
    }

    public void SetState(GameState newState)
    {
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
