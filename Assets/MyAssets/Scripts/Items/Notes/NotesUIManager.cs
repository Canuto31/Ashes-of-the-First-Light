using TMPro;
using UnityEngine;

public class NotesUIManager : MonoBehaviour
{
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

    private void CloseNote()
    {
        _panel.SetActive(false);
        
        GameStateManager.Instance.SetState(GameStateManager.GameState.Playing);
    }

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

    private void UpdateUI()
    {
        if (_currentNote == null || _currentNote.pages == null || _currentNote.pages.Length == 0)
            return;

        _titleText.text = _currentNote.noteTitle;
        _contentText.text = _currentNote.pages[_currentPage];
        _pageIndicator.text = (_currentPage + 1) + "/" + _currentNote.pages.Length;
    }
}
