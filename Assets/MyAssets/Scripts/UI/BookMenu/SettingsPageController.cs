using UnityEngine;

/// <summary>
/// Coordinates settings page behavior and its Unity lifecycle.
/// </summary>
public class SettingsPageController : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] private UISelectableOption[] _options;

    private PlayerInputHandler _input;

    private int _currentOption;

    private void Start()
    {
        _input = FindFirstObjectByType<PlayerInputHandler>();

        UpdateVisuals();
    }

    private void Update()
    {
        if (_input == null || GameStateManager.Instance == null ||
            GameStateManager.Instance.GetState() != GameStateManager.GameState.BookMenu)
            return;

        HandleNavigation();
        HandleConfirm();
    }

    private void HandleNavigation()
    {
        if (_input.NavigateUpPressed)
            NavigateUp();
        else if (_input.NavigateDownPressed)
            NavigateDown();
    }

    private void NavigateUp()
    {
        if (_options.Length == 0)
            return;

        _currentOption = (_currentOption - 1 + _options.Length) % _options.Length;

        UpdateVisuals();
    }

    private void NavigateDown()
    {
        if (_options.Length == 0)
            return;

        _currentOption = (_currentOption + 1) % _options.Length;

        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        for (int i = 0; i < _options.Length; i++)
        {
            bool isSelected = i == _currentOption;
            
            _options[i].SetSelected(isSelected);
        }
    }

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

    private void ResumeGame()
    {
        BookMenuManager.Instance?.CloseBook();
    }

    private void ReturnToCheckpoint()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        CheckpointManager.Instance?.ReturnToCheckpoint(player);
        BookMenuManager.Instance?.CloseBook();
    }

    private void ExitGame()
    {
        Debug.Log("Exit Game");
    }
}
