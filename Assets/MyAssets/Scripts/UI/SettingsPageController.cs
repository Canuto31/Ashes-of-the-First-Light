using UnityEngine;

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
        if (GameStateManager.Instance.GetState() != GameStateManager.GameState.BookMenu)
            return;

        HandleNavigation();
        HandleConfirm();
    }

    private void HandleNavigation()
    {
        if (_input.NavigateUpPressed)
        {
            NavigateUp();
        }
        else if (_input.NavigateDownPressed)
        {
            NavigateDown();
        }
    }

    private void NavigateUp()
    {
        _currentOption--;
        
        if (_currentOption < 0)
            _currentOption = _options.Length - 1;

        UpdateVisuals();
    }

    private void NavigateDown()
    {
        _currentOption++;
        
        if (_currentOption >= _options.Length)
            _currentOption = 0;

        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        for (int i = 0; i < _options.Length; i++)
        {
            bool isSelected = (i == _currentOption);
            
            _options[i].SetSelected(isSelected);
        }
    }

    private void HandleConfirm()
    {
        if (!_input.ConfirmPressed) return;

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
        FindFirstObjectByType<BookMenuManager>().SendMessage("CloseBook");
    }

    private void ReturnToCheckpoint()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        CheckpointManager.Instance.ReturnToCheckpoint(player);
        
        FindFirstObjectByType<BookMenuManager>().SendMessage("CloseBook");
    }

    private void ExitGame()
    {
        Debug.Log("Exit Game");
    }
}
