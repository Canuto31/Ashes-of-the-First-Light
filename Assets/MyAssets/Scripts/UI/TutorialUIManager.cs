using System;
using TMPro;
using UnityEngine;

public class TutorialUIManager : MonoBehaviour
{
    public static TutorialUIManager Instance;

    [SerializeField] private PlayerInputHandler _input;

    [Header("UI")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _text;

    private Action _onCloseCallback;
    private string _currentTutorialId;

    private void Awake()
    {
        Instance = this;
        _panel.SetActive(false);
    }

    private void Update()
    {
        if (GameStateManager.Instance.GetState() != GameStateManager.GameState.Tutorial)
            return;

        if (_input != null && _input.InteractPressed)
            CloseTutorial();
    }

    public void ShowTutorial(string tutorialId, string message, Action onClose)
    {
        _currentTutorialId = tutorialId;
        _onCloseCallback = onClose;
        
        _text.text = message;
        _panel.SetActive(true);
        
        GameStateManager.Instance.SetState(GameStateManager.GameState.Tutorial);
        
        UI_Interaction.Instance.Hide();
        InteractionUIManager.Instance.HideVisual();
    }

    private void CloseTutorial()
    {
        _panel.SetActive(false);
        
        TutorialManager.Instance.SetSeen(_currentTutorialId);
        
        GameStateManager.Instance.SetState(GameStateManager.GameState.Playing);
        
        _onCloseCallback?.Invoke();
    }
}
