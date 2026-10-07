using TMPro;
using UnityEngine;

/// <summary>
/// Coordinates notes uimanager state and operations for the game.
/// </summary>
public class NotesUIManager : MonoBehaviour
{

    #region Fields and Configuration

    public static NotesUIManager Instance { get; private set; }
    
    [SerializeField] private PlayerInputHandler _input;
    
    [Header("UI")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _contentText;
    [SerializeField] private TextMeshProUGUI _pageIndicator;

    private NoteData _currentNote;
    private int _currentPage;

    private bool _ignoreInputThisFrame;


    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Caches required dependencies and initializes this component before other Unity callbacks run.
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _panel.SetActive(false);
    }

    /// <summary>
    /// Coordinates frame-based input and state updates for this component.
    /// </summary>
    private void Update()
    {
        if (GameStateManager.Instance == null ||
            GameStateManager.Instance.GetState() != GameStateManager.GameState.BookMenu)
            return;

        if (_ignoreInputThisFrame)
        {
            _ignoreInputThisFrame = false;
            return;
        }

        HandleInput();
    }


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Processes input for this component.
    /// </summary>
    private void HandleInput()
    {
        if (_input == null)
            return;

        if (_input.NextPagePressed)
            NextPage();

        if (_input.PreviousPagePressed)
            PreviousPage();

        if (_input.InteractPressed)
            CloseNote();
    }

    /// <summary>
    /// Opens note for this component.
    /// </summary>
    public void OpenNote(NoteData note)
    {
        if (note == null || note.pages == null || note.pages.Length == 0)
            return;

        _currentNote = note;
        _currentPage = 0;

        UpdateUI();
        
        _panel.SetActive(true);
        
        GameStateManager.Instance.SetState(GameStateManager.GameState.BookMenu);
        
        _ignoreInputThisFrame = true;
        
        UI_Interaction.Instance?.Hide();
        InteractionUIManager.Instance?.HideVisual();
    }

    /// <summary>
    /// Closes note for this component.
    /// </summary>
    private void CloseNote()
    {
        _panel.SetActive(false);
        
        GameStateManager.Instance.SetState(GameStateManager.GameState.Playing);
    }

    /// <summary>
    /// Executes the next page operation for this component.
    /// </summary>
    private void NextPage()
    {
        if (_currentNote == null || _currentNote.pages == null)
            return;

        if (_currentPage < _currentNote.pages.Length - 1)
        {
            _currentPage++;
            UpdateUI();
        }
    }

    /// <summary>
    /// Executes the previous page operation for this component.
    /// </summary>
    private void PreviousPage()
    {
        if (_currentNote == null)
            return;

        if (_currentPage > 0)
        {
            _currentPage--;
            UpdateUI();
        }
    }

    /// <summary>
    /// Refreshes ui for this component.
    /// </summary>
    private void UpdateUI()
    {
        if (_currentNote == null || _currentNote.pages == null || _currentNote.pages.Length == 0)
            return;

        _titleText.text = _currentNote.noteTitle;
        _contentText.text = _currentNote.pages[_currentPage];
        _pageIndicator.text = (_currentPage + 1) + "/" + _currentNote.pages.Length;
    }

    #endregion
}
