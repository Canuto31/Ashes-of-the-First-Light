using System;
using TMPro;
using UnityEngine;

public class NotesUIManager : MonoBehaviour
{
    public static NotesUIManager Instance;
    
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
        Instance = this;
        _panel.SetActive(false);
    }

    private void Update()
    {
        if (GameStateManager.Instance.GetState() != GameStateManager.GameState.ReadingMenu)
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
        if (_input == null) return;

        if (_input.NextPagePressed)
            NextPage();

        if (_input.PreviousPagePressed)
            PreviousPage();

        if (_input.InteractPressed)
            CloseNote();
    }

    public void OpenNote(NoteData note)
    {
        _currentNote = note;
        _currentPage = 0;

        UpdateUI();
        
        _panel.SetActive(true);
        
        GameStateManager.Instance.SetState(GameStateManager.GameState.ReadingMenu);
        
        _ignoreInputThisFrame = true;
        
        UI_Interaction.Instance.Hide();
        InteractionUIManager.Instance.HideVisual();
    }

    private void CloseNote()
    {
        _panel.SetActive(false);
        
        GameStateManager.Instance.SetState(GameStateManager.GameState.Playing);
    }

    private void NextPage()
    {
        if (_currentPage < _currentNote.pages.Length - 1)
        {
            _currentPage++;
            UpdateUI();
        }
    }

    private void PreviousPage()
    {
        if (_currentPage > 0)
        {
            _currentPage--;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        _titleText.text = _currentNote.noteTitle;
        _contentText.text = _currentNote.pages[_currentPage];
        _pageIndicator.text = (_currentPage + 1) + "/" + _currentNote.pages.Length;
    }
}
