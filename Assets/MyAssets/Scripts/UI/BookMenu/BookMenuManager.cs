using UnityEngine;

/// <summary>
/// Coordinates book menu state and operations for the game.
/// </summary>
public class BookMenuManager : MonoBehaviour
{

    #region Fields and Configuration

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
    }

    /// <summary>
    /// Initializes runtime state after all scene objects have completed their Awake phase.
    /// </summary>
    private void Start()
    {
        _input = FindFirstObjectByType<PlayerInputHandler>();

        _currentPage = 0;

        ShowPage(_currentPage);

        if (_bookRoot != null)
            _bookRoot.SetActive(false);
    }

    /// <summary>
    /// Coordinates frame-based input and state updates for this component.
    /// </summary>
    private void Update()
    {
        if (_input == null || GameStateManager.Instance == null)
            return;

        ProcessBookInput();
    }


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Executes the process book input operation for this component.
    /// </summary>
    private void ProcessBookInput()
    {
        UpdateContextPageTimer();
        HandleBookToggle();
        HandleContextPageOpen();

        if (GameStateManager.Instance.GetState() != GameStateManager.GameState.BookMenu)
            return;

        HandlePageNavigation();
    }

    /// <summary>
    /// Processes book toggle for this component.
    /// </summary>
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

    /// <summary>
    /// Processes context page open for this component.
    /// </summary>
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

    /// <summary>
    /// Refreshes context page timer for this component.
    /// </summary>
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

    /// <summary>
    /// Processes page navigation for this component.
    /// </summary>
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

    /// <summary>
    /// Opens book for this component.
    /// </summary>
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

    /// <summary>
    /// Closes book for this component.
    /// </summary>
    public void CloseBook()
    {
        GameStateManager.Instance?.SetState(GameStateManager.GameState.Playing);

        if (_bookRoot != null)
            _bookRoot.SetActive(false);
    }

    /// <summary>
    /// Executes the next page operation for this component.
    /// </summary>
    private void NextPage()
    {
        if (_pages.Length == 0)
            return;

        _currentPage = (_currentPage + 1) % _pages.Length;

        ShowPage(_currentPage);
    }

    /// <summary>
    /// Executes the previous page operation for this component.
    /// </summary>
    private void PreviousPage()
    {
        if (_pages.Length == 0)
            return;

        _currentPage = (_currentPage - 1 + _pages.Length) % _pages.Length;

        ShowPage(_currentPage);
    }

    /// <summary>
    /// Displays page for this component.
    /// </summary>
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

    /// <summary>
    /// Opens book at page for this component.
    /// </summary>
    public void OpenBookAtPage(BookPage page)
    {
        _currentPage = GetPageIndex(page);

        OpenBook();
    }

    /// <summary>
    /// Returns the current page index for this component.
    /// </summary>
    private int GetPageIndex(BookPage page) => (int)page;

    /// <summary>
    /// Queues context page for this component.
    /// </summary>
    public void QueueContextPage(BookPage page)
    {
        _hasPendingContextPage = true;

        _pendingPage = page;

        _contextPageTimer = 2f;
    }

    #endregion
}
