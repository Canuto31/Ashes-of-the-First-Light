using UnityEngine;

/// <summary>
/// Coordinates game menu behavior and its Unity lifecycle.
/// </summary>
public class GameMenuController : MonoBehaviour
{

    #region Fields and Configuration

    [SerializeField] private PlayerInputHandler _input;


    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Coordinates frame-based input and state updates for this component.
    /// </summary>
    private void Update()
    {
        HandleMenuInput();
    }


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Processes menu input for this component.
    /// </summary>
    private void HandleMenuInput()
    {
        if (_input == null || !_input.ToggleMenuPressed)
            return;

        HandleMenu();
    }

    /// <summary>
    /// Processes menu for this component.
    /// </summary>
    private void HandleMenu()
    {
        if (GameStateManager.Instance == null)
            return;

        GameStateManager.GameState currentState = GameStateManager.Instance.GetState();

        if (currentState == GameStateManager.GameState.Playing)
        {
            GameStateManager.Instance.SetState(GameStateManager.GameState.BookMenu);
        }
        else if (currentState == GameStateManager.GameState.BookMenu)
        {
            GameStateManager.Instance.SetState(GameStateManager.GameState.Playing);
        }
    }

    #endregion
}
