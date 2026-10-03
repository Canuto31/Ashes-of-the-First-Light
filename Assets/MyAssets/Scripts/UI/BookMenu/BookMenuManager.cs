using UnityEngine;

public class BookMenuManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _bookRoot;
    [SerializeField] private GameObject[] _pages;
    [SerializeField] private NotesPageController _notesPageController;
    [SerializeField] private TutorialPageController _tutorialPageController;
    [SerializeField] private InventoryPageController _inventoryPageController;

    private PlayerInputHandler _input;

    private int _currentPage;
    private bool _justOpenedBook;

    public static BookMenuManager Instance { get; private set; }

    // Context Page
    private bool _hasPendingContextPage;
    private BookPage _pendingPage;
    private float _contextPageTimer;

    public enum BookPage
    {
        Notes,
        Tutorials,
        Inventory,
        Settings
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        _input = FindFirstObjectByType<PlayerInputHandler>();

        _currentPage = 0;

        ShowPage(_currentPage);

        if (_bookRoot != null)
            _bookRoot.SetActive(false);
    }

    private void Update()
    {
        if (_input == null || GameStateManager.Instance == null)
            return;

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
        if (_bookRoot == null || GameStateManager.Instance == null)
            return;

        GameStateManager.Instance.SetState(GameStateManager.GameState.BookMenu);
        _bookRoot.SetActive(true);

        ShowPage(_currentPage);

        if (_currentPage == (int)BookPage.Notes)
            _notesPageController?.FocusLastCollectedNote();

        _justOpenedBook = true;

        UI_Interaction.Instance?.Hide();
        InteractionUIManager.Instance?.HideVisual();
    }

    public void CloseBook()
    {
        GameStateManager.Instance?.SetState(GameStateManager.GameState.Playing);

        if (_bookRoot != null)
            _bookRoot.SetActive(false);
    }

    private void NextPage()
    {
        if (_pages.Length == 0)
            return;

        _currentPage = (_currentPage + 1) % _pages.Length;

        ShowPage(_currentPage);
    }

    private void PreviousPage()
    {
        if (_pages.Length == 0)
            return;

        _currentPage = (_currentPage - 1 + _pages.Length) % _pages.Length;

        ShowPage(_currentPage);
    }

    private void ShowPage(int index)
    {
        if (index < 0 || index >= _pages.Length)
            return;

        for (int i = 0; i < _pages.Length; i++)
        {
            if (_pages[i] != null)
                _pages[i].SetActive(i == index);
        }

        switch ((BookPage)index)
        {
            case BookPage.Notes:
                _notesPageController?.RefreshNotes();
                break;
            
            case BookPage.Inventory:
                _inventoryPageController?.RefreshInventory();
                break;

            case BookPage.Tutorials:
                _tutorialPageController?.RefreshTutorials();
                break;
        }
    }

    public void OpenBookAtPage(BookPage page)
    {
        _currentPage = GetPageIndex(page);

        OpenBook();
    }

    private int GetPageIndex(BookPage page) => (int)page;

    public void QueueContextPage(BookPage page)
    {
        _hasPendingContextPage = true;

        _pendingPage = page;

        _contextPageTimer = 2f;
    }
}
