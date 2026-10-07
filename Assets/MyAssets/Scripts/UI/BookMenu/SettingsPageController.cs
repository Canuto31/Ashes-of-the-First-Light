using UnityEngine;

/// <summary>
/// Coordinates settings page behavior and its Unity lifecycle.
/// </summary>
public class SettingsPageController : MonoBehaviour
{

    #region Fields and Configuration

    [Header("Options")]
    [SerializeField] private UISelectableOption[] _options;

    private PlayerInputHandler _input;

    private int _currentOption;


    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Initializes runtime state after all scene objects have completed their Awake phase.
    /// </summary>
    private void Start()
    {
        _input = FindFirstObjectByType<PlayerInputHandler>();

        UpdateVisuals();
    }

    /// <summary>
    /// Coordinates frame-based input and state updates for this component.
    /// </summary>
    private void Update()
    {
        if (_input == null || GameStateManager.Instance == null ||
            GameStateManager.Instance.GetState() != GameStateManager.GameState.BookMenu)
            return;

        HandleSettingsInput();
    }


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Processes settings input for this component.
    /// </summary>
    private void HandleSettingsInput()
    {
        HandleNavigation();
        HandleConfirm();
    }

    /// <summary>
    /// Processes navigation for this component.
    /// </summary>
    private void HandleNavigation()
    {
        if (_input.NavigateUpPressed)
            NavigateUp();
        else if (_input.NavigateDownPressed)
            NavigateDown();
    }

    /// <summary>
    /// Executes the navigate up operation for this component.
    /// </summary>
    private void NavigateUp()
    {
        if (_options.Length == 0)
            return;

        _currentOption = (_currentOption - 1 + _options.Length) % _options.Length;

        UpdateVisuals();
    }

    /// <summary>
    /// Executes the navigate down operation for this component.
    /// </summary>
    private void NavigateDown()
    {
        if (_options.Length == 0)
            return;

        _currentOption = (_currentOption + 1) % _options.Length;

        UpdateVisuals();
    }

    /// <summary>
    /// Refreshes visuals for this component.
    /// </summary>
    private void UpdateVisuals()
    {
        for (int i = 0; i < _options.Length; i++)
        {
            bool isSelected = i == _currentOption;
            
            _options[i].SetSelected(isSelected);
        }
    }

    /// <summary>
    /// Processes confirm for this component.
    /// </summary>
    private void HandleConfirm()
    {
        if (!_input.ConfirmPressed)
            return;

        switch (_currentOption)
        {
            case 0:
                ResumeGame();
                break;
            case 1:
                ReturnToCheckpoint();
                break;
            case 2:
                ExitGame();
                break;
        }
    }

    /// <summary>
    /// Executes the resume game operation for this component.
    /// </summary>
    private void ResumeGame()
    {
        BookMenuManager.Instance?.CloseBook();
    }

    /// <summary>
    /// Returns to checkpoint for this component.
    /// </summary>
    private void ReturnToCheckpoint()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        CheckpointManager.Instance?.ReturnToCheckpoint(player);
        BookMenuManager.Instance?.CloseBook();
    }

    /// <summary>
    /// Executes the exit game operation for this component.
    /// </summary>
    private void ExitGame()
    {
        Debug.Log("Exit Game");
    }

    #endregion
}
