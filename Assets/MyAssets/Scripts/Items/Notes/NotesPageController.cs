using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Coordinates notes page behavior and its Unity lifecycle.
/// </summary>
public class NotesPageController : MonoBehaviour
{

    #region Fields and Configuration

    [Header("List")] 
    [SerializeField] private Transform _notesContainer;
    [SerializeField] private GameObject _notesOptionPrefab;
    
    [Header("Content")]
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _contentText;
    [SerializeField] private TextMeshProUGUI _pageIndicatorText;

    private PlayerInputHandler _input;

    private List<NoteData> _notes;

    private readonly List<UISelectableOption> _spawnOptions = new();

    private int _currentNoteIndex;
    private int _currentPageIndex;


    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Initializes runtime state after all scene objects have completed their Awake phase.
    /// </summary>
    private void Start()
    {
        _input = FindFirstObjectByType<PlayerInputHandler>();
        
        RefreshNotes();
    }

    /// <summary>
    /// Coordinates frame-based input and state updates for this component.
    /// </summary>
    private void Update()
    {
        if (_input == null || GameStateManager.Instance == null ||
            GameStateManager.Instance.GetState() != GameStateManager.GameState.BookMenu)
            return;

        HandleNavigationInput();
    }


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Processes navigation input for this component.
    /// </summary>
    private void HandleNavigationInput()
    {
        HandleNoteNavigation();
        HandlePageNavigation();
    }

    /// <summary>
    /// Rebuilds notes for this component.
    /// </summary>
    public void RefreshNotes()
    {
        if (_notesContainer == null || _notesOptionPrefab == null || NotesManager.Instance == null)
            return;

        ClearSpawnedOptions();
        PopulateNotes();
        SelectInitialNote();
    }

    /// <summary>
    /// Clears spawned options for this component.
    /// </summary>
    private void ClearSpawnedOptions()
    {
        foreach (Transform child in _notesContainer)
            Destroy(child.gameObject);

        _spawnOptions.Clear();
    }

    /// <summary>
    /// Executes the populate notes operation for this component.
    /// </summary>
    private void PopulateNotes()
    {
        _notes = NotesManager.Instance.GetNotes();

        foreach (NoteData note in _notes)
        {
            GameObject optionObj = Instantiate(_notesOptionPrefab, _notesContainer);
            
            UISelectableOption option = optionObj.GetComponent<UISelectableOption>();
            NoteOptionUI optionUI = optionObj.GetComponent<NoteOptionUI>();

            optionUI?.SetTitle(note.noteTitle);

            if (option != null)
                _spawnOptions.Add(option);
        }

    }

    /// <summary>
    /// Selects initial note for this component.
    /// </summary>
    private void SelectInitialNote()
    {
        if (_notes.Count > 0)
            SelectNote(0);
        else
            ClearContent();
    }

    /// <summary>
    /// Clears content for this component.
    /// </summary>
    private void ClearContent()
    {
        _titleText.text = "";
        _contentText.text = "";
        _pageIndicatorText.text = "";
    }

    /// <summary>
    /// Selects note for this component.
    /// </summary>
    private void SelectNote(int index)
    {
        if (_notes == null || index < 0 || index >= _notes.Count)
            return;

        _currentNoteIndex = index;
        
        _currentPageIndex = 0;

        UpdateVisuals();

        UpdateContent();
    }

    /// <summary>
    /// Refreshes visuals for this component.
    /// </summary>
    private void UpdateVisuals()
    {
        for (int i = 0; i < _spawnOptions.Count; i++)
        {
            bool selected = i == _currentNoteIndex;
            
            _spawnOptions[i].SetSelected(selected);
        }
    }

    /// <summary>
    /// Refreshes content for this component.
    /// </summary>
    private void UpdateContent()
    {
        if (_notes == null || _notes.Count == 0)
            return;
        
        NoteData note = _notes[_currentNoteIndex];

        if (note == null || note.pages == null || note.pages.Length == 0)
        {
            ClearContent();
            return;
        }
        
        _titleText.text = note.noteTitle;
        
        _contentText.text = note.pages[_currentPageIndex];
        
        _pageIndicatorText.text = (_currentPageIndex + 1) + " / " + note.pages.Length;
    }

    /// <summary>
    /// Processes note navigation for this component.
    /// </summary>
    private void HandleNoteNavigation()
    {
        if (_notes == null || _notes.Count == 0)
            return;

        if (_input.NavigateUpPressed)
        {
            SelectNote((_currentNoteIndex - 1 + _notes.Count) % _notes.Count);
        }
        else if (_input.NavigateDownPressed)
        {
            SelectNote((_currentNoteIndex + 1) % _notes.Count);
        }
    }

    /// <summary>
    /// Processes page navigation for this component.
    /// </summary>
    private void HandlePageNavigation()
    {
        if (_notes == null || _notes.Count == 0)
            return;
        
        NoteData note = _notes[_currentNoteIndex];

        if (note == null || note.pages == null || note.pages.Length == 0)
            return;

        if (_input.NextPagePressed)
        {
            if (_currentPageIndex < note.pages.Length - 1)
            {
                _currentPageIndex++;
            
                UpdateContent();
            }
        }
        else if (_input.PreviousPagePressed)
        {
            if (_currentPageIndex > 0)
            {
                _currentPageIndex--;
                
                UpdateContent();
            }
        }
    }

    /// <summary>
    /// Focuses last collected note for this component.
    /// </summary>
    public void FocusLastCollectedNote()
    {
        NoteData lastNote = NotesManager.Instance?.GetLastCollectedNote();

        if (lastNote == null || _notes == null)
            return;
        
        int index = _notes.IndexOf(lastNote);

        if (index >= 0)
        {
            SelectNote(index);
        }
    }

    #endregion
}
