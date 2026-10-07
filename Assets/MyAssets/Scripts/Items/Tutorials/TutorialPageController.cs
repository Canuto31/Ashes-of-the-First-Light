using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Coordinates tutorial page behavior and its Unity lifecycle.
/// </summary>
public class TutorialPageController : MonoBehaviour
{
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

    private void Start()
    {
        _input = FindFirstObjectByType<PlayerInputHandler>();

        RefreshTutorials();
    }

    private void Update()
    {
        if (_input == null || GameStateManager.Instance == null ||
            GameStateManager.Instance.GetState() != GameStateManager.GameState.BookMenu)
            return;

        HandleTutorialNavigation();
    }

    public void RefreshTutorials()
    {
        if (_tutorialsContainer == null || _tutorialOptionPrefab == null || TutorialManager.Instance == null)
            return;

        ClearSpawnedOptions();
        PopulateTutorials();
        SelectInitialTutorial();
    }

    private void ClearSpawnedOptions()
    {
        foreach (Transform child in _tutorialsContainer)
            Destroy(child.gameObject);

        _spawnedOptions.Clear();
    }

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

    private void SelectInitialTutorial()
    {
        if (_tutorials.Count > 0)
            SelectTutorial(0);
        else
            ClearContent();
    }

    private void ClearContent()
    {
        _titleText.text = "";
        _contentText.text = "";
    }

    private void SelectTutorial(int index)
    {
        if (_tutorials == null || index < 0 || index >= _tutorials.Count)
            return;

        _currentTutorialIndex = index;

        UpdateVisuals();

        UpdateContent();
    }

    private void UpdateVisuals()
    {
        for (int i = 0; i < _spawnedOptions.Count; i++)
        {
            bool selected = i == _currentTutorialIndex;

            _spawnedOptions[i].SetSelected(selected);
        }
    }

    private void UpdateContent()
    {
        if (_tutorials.Count == 0)
            return;

        TutorialData tutorial =
            _tutorials[_currentTutorialIndex];

        _titleText.text = tutorial.title;

        _contentText.text = tutorial.description;
    }

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
}
