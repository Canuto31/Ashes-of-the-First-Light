using System;
using UnityEngine;

public class BookMenuManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _bookRoot;
    [SerializeField] private GameObject[] _pages;

    private PlayerInputHandler _input;

    private int _currentPage;
    private bool _justOpenedBook;

    private void Start()
    {
        _input = FindFirstObjectByType<PlayerInputHandler>();
        
        _currentPage = 0;
        ShowPage(_currentPage);
        
        _bookRoot.SetActive(false);
    }

    private void Update()
    {
        HandleBookToggle();

        if (GameStateManager.Instance.GetState() != GameStateManager.GameState.BookMenu) 
            return;

        HandlePageNavigation();
    }

    private void HandleBookToggle()
    {
        if (!_input.ConsumeToggleMenu()) return;

        if (GameStateManager.Instance.GetState() == GameStateManager.GameState.BookMenu)
        {
            CloseBook();
        }
        else if (GameStateManager.Instance.IsPlaying())
        {
            OpenBook();
        }
    }

    private void HandlePageNavigation()
    {
        if (_justOpenedBook)
        {
            _justOpenedBook = false;
            return;
        }
        
        if (_input.NextBookPagePressed)
        {
            NextPage();
        }
        else if (_input.PreviousBookPagePressed)
        {
            PreviousPage();
        }
    }

    private void OpenBook()
    {
        GameStateManager.Instance.SetState(GameStateManager.GameState.BookMenu);
        
        _currentPage = 0;
        
        _bookRoot.SetActive(true);
        
        ShowPage(_currentPage);

        _justOpenedBook = true;
        
        UI_Interaction.Instance.Hide();
        InteractionUIManager.Instance.HideVisual();
    }

    private void CloseBook()
    {
        GameStateManager.Instance.SetState(GameStateManager.GameState.Playing);
        
        _bookRoot.SetActive(false);
    }

    private void NextPage()
    {
        _currentPage++;

        if (_currentPage >= _pages.Length)
            _currentPage = 0;

        ShowPage(_currentPage);
    }

    private void PreviousPage()
    {
        _currentPage--;
        
        if (_currentPage < 0)
            _currentPage = _pages.Length - 1;
        
        ShowPage(_currentPage);
    }

    private void ShowPage(int index)
    {
        for (int i = 0; i < _pages.Length; i++)
        {
            _pages[i].SetActive(i == index);
        }
    }
}
