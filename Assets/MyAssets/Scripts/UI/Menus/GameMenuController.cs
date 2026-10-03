using UnityEngine;

public class GameMenuController : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler _input;

    private void Update()
    {
        if (_input != null && _input.ToggleMenuPressed)
            HandleMenu();
    }

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
}
