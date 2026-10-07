using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Coordinates tutorial page behavior and its Unity lifecycle.
/// </summary>
public class TutorialPageController : MonoBehaviour
{

    #region Fields and Configuration

    [Header("List")]
    [SerializeField] private Transform _tutorialsContainer;
    [SerializeField] private GameObject _tutorialOptionPrefab;

    [Header("Content")]
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _contentText;

    private PlayerInputHandler _input;

    private List<TutorialData> _tutorials;

    private readonly List<UISelectableOption> _spawnedOptions = new();

    private int _currentTutorialIndex;


    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Initializes runtime state after all scene objects have completed their Awake phase.
    /// </summary>
    private void Start()
    {
        _input = FindFirstObjectByType<PlayerInputHandler>();

        RefreshTutorials();
    }

    /// <summary>
    /// Coordinates frame-based input and state updates for this component.
    /// </summary>
    private void Update()
    {
        if (_input == null || GameStateManager.Instance == null ||
            GameStateManager.Instance.GetState() != GameStateManager.GameState.BookMenu)
            return;

        HandleTutorialNavigation();
    }


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Rebuilds tutorials for this component.
    /// </summary>
    public void RefreshTutorials()
    {
        if (_tutorialsContainer == null || _tutorialOptionPrefab == null || TutorialManager.Instance == null)
            return;

        ClearSpawnedOptions();
        PopulateTutorials();
        SelectInitialTutorial();
    }

    /// <summary>
    /// Clears spawned options for this component.
    /// </summary>
    private void ClearSpawnedOptions()
    {
        foreach (Transform child in _tutorialsContainer)
            Destroy(child.gameObject);

        _spawnedOptions.Clear();
    }

    /// <summary>
    /// Executes the populate tutorials operation for this component.
    /// </summary>
    private void PopulateTutorials()
    {
        _tutorials = TutorialManager.Instance.GetUnlockedTutorials();

        foreach (TutorialData tutorial in _tutorials)
        {
            GameObject optionObj =
                Instantiate(_tutorialOptionPrefab, _tutorialsContainer);

            UISelectableOption option =
                optionObj.GetComponent<UISelectableOption>();

            TutorialOptionUI optionUI =
                optionObj.GetComponent<TutorialOptionUI>();

            optionUI?.SetTitle(tutorial.title);

            if (option != null)
                _spawnedOptions.Add(option);
        }

    }

    /// <summary>
    /// Selects initial tutorial for this component.
    /// </summary>
    private void SelectInitialTutorial()
    {
        if (_tutorials.Count > 0)
            SelectTutorial(0);
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
    }

    /// <summary>
    /// Selects tutorial for this component.
    /// </summary>
    private void SelectTutorial(int index)
    {
        if (_tutorials == null || index < 0 || index >= _tutorials.Count)
            return;

        _currentTutorialIndex = index;

        UpdateVisuals();

        UpdateContent();
    }

    /// <summary>
    /// Refreshes visuals for this component.
    /// </summary>
    private void UpdateVisuals()
    {
        for (int i = 0; i < _spawnedOptions.Count; i++)
        {
            bool selected = i == _currentTutorialIndex;

            _spawnedOptions[i].SetSelected(selected);
        }
    }

    /// <summary>
    /// Refreshes content for this component.
    /// </summary>
    private void UpdateContent()
    {
        if (_tutorials.Count == 0)
            return;

        TutorialData tutorial =
            _tutorials[_currentTutorialIndex];

        _titleText.text = tutorial.title;

        _contentText.text = tutorial.description;
    }

    /// <summary>
    /// Processes tutorial navigation for this component.
    /// </summary>
    private void HandleTutorialNavigation()
    {
        if (_tutorials == null || _tutorials.Count == 0)
            return;

        if (_input.NavigateUpPressed)
        {
            SelectTutorial((_currentTutorialIndex - 1 + _tutorials.Count) % _tutorials.Count);
        }
        else if (_input.NavigateDownPressed)
        {
            SelectTutorial((_currentTutorialIndex + 1) % _tutorials.Count);
        }
    }

    #endregion
}
