using System;
using System.Collections;
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
    
    private bool _canClose;

    private void Awake()
    {
        Instance = this;
        _panel.SetActive(false);
    }

    private void Update()
    {
        if (GameStateManager.Instance.GetState() != GameStateManager.GameState.Tutorial)
            return;

        if (_canClose && _input != null && _input.InteractPressed)
        {
            CloseTutorial();
        }
    }

    public void ShowTutorial(string tutorialId, string message, Action onClose = null)
    {
        _currentTutorialId = tutorialId;
        _onCloseCallback = onClose;
        
        _text.text = message;
        _panel.SetActive(true);
        
        _canClose = false;
        
        GameStateManager.Instance.SetState(GameStateManager.GameState.Tutorial);
        
        UI_Interaction.Instance.Hide();
        InteractionUIManager.Instance.HideVisual();
        
        StartCoroutine(EnableCloseDelay());
    }

    private void CloseTutorial()
    {
        Debug.Log("CLOSING TUTORIAL");
        
        _panel.SetActive(false);
        
        TutorialManager.Instance.SetSeen(_currentTutorialId);
        
        GameStateManager.Instance.SetState(GameStateManager.GameState.Playing);
        
        _onCloseCallback?.Invoke();
    }
    
    private IEnumerator EnableCloseDelay()
    {
        yield return null;

        _canClose = true;
    }
}
