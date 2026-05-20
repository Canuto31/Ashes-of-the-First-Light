using UnityEngine;

public class BookMenuManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _bookRoot;
    [SerializeField] private GameObject[] _pages;
    [SerializeField] private NotesPageController _notesPageController;

    private PlayerInputHandler _input;

    private int _currentPage;
    private bool _justOpenedBook;

    public static BookMenuManager Instance;

    // Context Page
    private bool _hasPendingContextPage;
    private BookPage _pendingPage;
    private float _contextPageTimer;

    public enum BookPage
    {
        Notes,
        Tutorials,
        Settings
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _input = FindFirstObjectByType<PlayerInputHandler>();

        _currentPage = 0;

        ShowPage(_currentPage);

        _bookRoot.SetActive(false);
    }

    private void Update()
    {
        UpdateContextPageTimer();

        HandleBookToggle();

        HandleContextPageOpen();

        if (GameStateManager.Instance.GetState() != GameStateManager.GameState.BookMenu)
            return;

        HandlePageNavigation();
    }

    private void HandleBookToggle()
    {
        if (!_input.ConsumeToggleMenu())
            return;

        if (GameStateManager.Instance.GetState() == GameStateManager.GameState.BookMenu)
        {
            CloseBook();
        }
        else if (GameStateManager.Instance.IsPlaying())
        {
            OpenBookAtPage(BookPage.Notes);
        }
    }

    private void HandleContextPageOpen()
    {
        if (!_input.OpenItemPressed)
            return;

        if (!_hasPendingContextPage)
            return;

        _currentPage = GetPageIndex(_pendingPage);

        OpenBook();

        _hasPendingContextPage = false;
    }

    private void UpdateContextPageTimer()
    {
        if (!_hasPendingContextPage)
            return;

        _contextPageTimer -= Time.deltaTime;

        if (_contextPageTimer <= 0f)
        {
            _hasPendingContextPage = false;
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

        _bookRoot.SetActive(true);

        ShowPage(_currentPage);

        if (_currentPage == (int)BookPage.Notes)
        {
            _notesPageController.RefreshNotes();
            _notesPageController.FocusLastCollectedNote();
        }

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

    public void OpenBookAtPage(BookPage page)
    {
        _currentPage = GetPageIndex(page);

        OpenBook();
    }

    private int GetPageIndex(BookPage page)
    {
        switch (page)
        {
            case BookPage.Notes:
                return 0;

            case BookPage.Tutorials:
                return 1;

            case BookPage.Settings:
                return 2;
        }

        return 0;
    }

    public void QueueContextPage(BookPage page)
    {
        _hasPendingContextPage = true;

        _pendingPage = page;

        _contextPageTimer = 2f;
    }
}