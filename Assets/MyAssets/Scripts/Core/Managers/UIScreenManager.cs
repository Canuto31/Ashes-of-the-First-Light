using UnityEngine;

/// <summary>
/// Coordinates uiscreen state and operations for the game.
/// </summary>
public class UIScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;

    private void Start()
    {
        if (_pauseMenu != null)
            _pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (GameStateManager.Instance == null || _pauseMenu == null)
            return;

        bool shouldShowPauseMenu =
            GameStateManager.Instance.GetState() == GameStateManager.GameState.BookMenu;

        if (shouldShowPauseMenu)
        {
            if (!_pauseMenu.activeSelf)
            {
                _pauseMenu.SetActive(true);
                UI_Interaction.Instance?.Hide();
                InteractionUIManager.Instance?.HideVisual();
            }
        }
        else if (_pauseMenu.activeSelf)
            _pauseMenu.SetActive(false);
    }
}
