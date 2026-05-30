using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        if (GameStateManager.Instance.GetState() != GameStateManager.GameState.BookMenu)
            return;

        HandleTutorialNavigation();
    }

    public void RefreshTutorials()
    {
        foreach (Transform child in _tutorialsContainer)
        {
            Destroy(child.gameObject);
        }

        _spawnedOptions.Clear();

        _tutorials = TutorialManager.Instance.GetUnlockedTutorials();

        foreach (TutorialData tutorial in _tutorials)
        {
            GameObject optionObj =
                Instantiate(_tutorialOptionPrefab, _tutorialsContainer);

            UISelectableOption option =
                optionObj.GetComponent<UISelectableOption>();

            TutorialOptionUI optionUI =
                optionObj.GetComponent<TutorialOptionUI>();

            optionUI.SetTitle(tutorial.title);

            _spawnedOptions.Add(option);
        }

        if (_tutorials.Count > 0)
        {
            SelectTutorial(0);
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
    }

    private void SelectTutorial(int index)
    {
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
            _currentTutorialIndex--;

            if (_currentTutorialIndex < 0)
                _currentTutorialIndex = _tutorials.Count - 1;

            SelectTutorial(_currentTutorialIndex);
        }
        else if (_input.NavigateDownPressed)
        {
            _currentTutorialIndex++;

            if (_currentTutorialIndex >= _tutorials.Count)
                _currentTutorialIndex = 0;

            SelectTutorial(_currentTutorialIndex);
        }
    }
}