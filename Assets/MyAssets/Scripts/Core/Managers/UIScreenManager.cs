using UnityEngine;

/// <summary>
/// Coordinates uiscreen state and operations for the game.
/// </summary>
public class UIScreenManager : MonoBehaviour
{

    #region Fields and Configuration

    [SerializeField] private GameObject _pauseMenu;


    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Initializes runtime state after all scene objects have completed their Awake phase.
    /// </summary>
    private void Start()
    {
        if (_pauseMenu != null)
            _pauseMenu.SetActive(false);
    }

    /// <summary>
    /// Coordinates frame-based input and state updates for this component.
    /// </summary>
    private void Update()
    {
        if (GameStateManager.Instance == null || _pauseMenu == null)
            return;

        UpdatePauseMenuVisibility();
    }


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Refreshes pause menu visibility for this component.
    /// </summary>
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

    /// <summary>
    /// Returns whether book menu open is currently true.
    /// </summary>
    private static bool IsBookMenuOpen()
    {
        return GameStateManager.Instance.GetState() == GameStateManager.GameState.BookMenu;
    }

    /// <summary>
    /// Displays pause menu for this component.
    /// </summary>
    private void ShowPauseMenu()
    {
        if (_pauseMenu.activeSelf)
            return;

        _pauseMenu.SetActive(true);
        UI_Interaction.Instance?.Hide();
        InteractionUIManager.Instance?.HideVisual();
    }

    #endregion
}
