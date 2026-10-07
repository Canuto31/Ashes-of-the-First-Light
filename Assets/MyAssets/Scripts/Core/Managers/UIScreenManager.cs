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

        UpdatePauseMenuVisibility();
    }

    private void UpdatePauseMenuVisibility()
    {
        bool shouldShowPauseMenu = IsBookMenuOpen();

        if (shouldShowPauseMenu)
        {
            ShowPauseMenu();
        }
        else if (_pauseMenu.activeSelf)
        {
            _pauseMenu.SetActive(false);
        }
    }

    private static bool IsBookMenuOpen()
    {
        return GameStateManager.Instance.GetState() == GameStateManager.GameState.BookMenu;
    }

    private void ShowPauseMenu()
    {
        if (_pauseMenu.activeSelf)
            return;

        _pauseMenu.SetActive(true);
        UI_Interaction.Instance?.Hide();
        InteractionUIManager.Instance?.HideVisual();
    }
}
