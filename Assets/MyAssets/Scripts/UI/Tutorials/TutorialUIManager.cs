using System;
using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Coordinates tutorial uimanager state and operations for the game.
/// </summary>
public class TutorialUIManager : MonoBehaviour
{

    #region Fields and Configuration

    public static TutorialUIManager Instance { get; private set; }

    [SerializeField] private PlayerInputHandler _input;

    [Header("UI")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _text;

    private Action _onCloseCallback;
    private string _currentTutorialId;

    private bool _canClose;


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
        _panel.SetActive(false);
    }

    /// <summary>
    /// Coordinates frame-based input and state updates for this component.
    /// </summary>
    private void Update()
    {
        HandleCloseInput();
    }


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Processes close input for this component.
    /// </summary>
    private void HandleCloseInput()
    {
        if (!CanProcessCloseInput())
            return;

        if (_input.InteractPressed)
            CloseTutorial();
    }

    /// <summary>
    /// Determines whether this component can process close input for this component.
    /// </summary>
    private bool CanProcessCloseInput()
    {
        return GameStateManager.Instance != null &&
               GameStateManager.Instance.GetState() == GameStateManager.GameState.Tutorial &&
               _canClose &&
               _input != null;
    }

    /// <summary>
    /// Displays tutorial for this component.
    /// </summary>
    public void ShowTutorial(string tutorialId, string message, Action onClose = null)
    {
        if (_panel == null || _text == null || GameStateManager.Instance == null)
            return;

        _currentTutorialId = tutorialId;
        _onCloseCallback = onClose;

        _text.text = message;
        _panel.SetActive(true);

        _canClose = false;

        GameStateManager.Instance.SetState(GameStateManager.GameState.Tutorial);

        UI_Interaction.Instance?.Hide();
        InteractionUIManager.Instance?.HideVisual();

        StartCoroutine(EnableCloseDelay());
    }

    /// <summary>
    /// Displays tutorial for this component.
    /// </summary>
    public void ShowTutorial(
        TutorialData tutorial,
        Action onClose = null)
    {
        if (tutorial == null)
            return;

        TutorialManager.Instance?.UnlockTutorial(tutorial);

        ShowTutorial(
            tutorial.tutorialId,
            tutorial.popupMessage,
            onClose
        );
    }

    /// <summary>
    /// Closes tutorial for this component.
    /// </summary>
    private void CloseTutorial()
    {
        _panel.SetActive(false);

        TutorialManager.Instance?.SetSeen(_currentTutorialId);

        GameStateManager.Instance.SetState(GameStateManager.GameState.Playing);

        _onCloseCallback?.Invoke();
    }

    /// <summary>
    /// Executes the enable close delay operation for this component.
    /// </summary>
    private IEnumerator EnableCloseDelay()
    {
        yield return null;

        _canClose = true;
    }

    #endregion
}
