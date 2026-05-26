using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotesPageController : MonoBehaviour
{
    [Header("List")] 
    [SerializeField] private Transform _notesContainer;
    [SerializeField] private GameObject _notesOptionPrefab;
    
    [Header("Content")]
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _contentText;
    [SerializeField] private TextMeshProUGUI _pageIndicatorText;

    private PlayerInputHandler _input;

    private List<NoteData> _notes;

    private List<UISelectableOption> _spawnOptions = new List<UISelectableOption>();

    private int _currentNoteIndex;
    private int _currentPageIndex;

    private void Start()
    {
        _input = FindFirstObjectByType<PlayerInputHandler>();
        
        RefreshNotes();
    }

    private void Update()
    {
        if (GameStateManager.Instance.GetState() != GameStateManager.GameState.BookMenu)
            return;

        HandleNoteNavigation();

        HandlePageNavigation();
    }

    public void RefreshNotes()
    {
        foreach (Transform child in _notesContainer)
        {
            Destroy(child.gameObject);
        }
        
        _spawnOptions.Clear();
        
        _notes = NotesManager.Instance.GetNotes();

        foreach (NoteData note in _notes)
        {
            GameObject optionObj = Instantiate(_notesOptionPrefab, _notesContainer);
            
            UISelectableOption option= optionObj.GetComponent<UISelectableOption>();
            
            NoteOptionUI optionUI = optionObj.GetComponent<NoteOptionUI>();

            optionUI.SetTitle(note.noteTitle);
            
            _spawnOptions.Add(option);
        }

        if (_notes.Count > 0)
        {
            SelectNote(0);
        }
        else
        {
            ClearContent();
        }
    }

    private void ClearContent()
    {
        _titleText.text = "";
        _contentText.text = "";
        _pageIndicatorText.text = "";
    }

    private void SelectNote(int index)
    {
        _currentNoteIndex = index;
        
        _currentPageIndex = 0;

        UpdateVisuals();

        UpdateContent();
    }

    private void UpdateVisuals()
    {
        for (int i = 0; i < _spawnOptions.Count; i++)
        {
            bool selected = i == _currentNoteIndex;
            
            _spawnOptions[i].SetSelected(selected);
        }
    }

    private void UpdateContent()
    {
        if (_notes.Count == 0) return;
        
        NoteData note = _notes[_currentNoteIndex];
        
        _titleText.text = note.noteTitle;
        
        _contentText.text = note.pages[_currentPageIndex];
        
        _pageIndicatorText.text = (_currentPageIndex + 1) + " / " + note.pages.Length;
    }

    private void HandleNoteNavigation()
    {
        if (_notes == null || _notes.Count == 0) return;

        if (_input.NavigateUpPressed)
        {
            _currentNoteIndex--;
            
            if (_currentNoteIndex < 0) 
                _currentNoteIndex = _notes.Count - 1;
            
            SelectNote(_currentNoteIndex);
        } else if (_input.NavigateDownPressed)
        {
            _currentNoteIndex++;
            
            if (_currentNoteIndex >= _notes.Count)
                _currentNoteIndex = 0;
            
            SelectNote(_currentNoteIndex);
        }
    }

    private void HandlePageNavigation()
    {
        if (_notes == null || _notes.Count == 0) return;
        
        NoteData note = _notes[_currentNoteIndex];

        if (_input.NextPagePressed)
        {
            if (_currentPageIndex < note.pages.Length - 1)
            {
                _currentPageIndex++;
            
                UpdateContent();
            }
        } else if (_input.PreviousPagePressed)
        {
            if (_currentPageIndex > 0)
            {
                _currentPageIndex--;
                
                UpdateContent();
            }
        }
    }

    public void FocusLastCollectedNote()
    {
        NoteData lastNote = NotesManager.Instance.GetLastCollectedNote();

        if (lastNote == null) return;
        
        int index = _notes.IndexOf(lastNote);

        if (index >= 0)
        {
            SelectNote(index);
        }
    }
}
