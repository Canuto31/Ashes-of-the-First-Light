using System.Collections;
using TMPro;
using UnityEngine;

public class UI_Interaction : MonoBehaviour
{
    public static UI_Interaction Instance;

    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI text;

    private Coroutine _currentRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Hide();
    }

    public void ShowText(string message)
    {
        if (!GameStateManager.Instance.IsPlaying())
            return;
        
        panel.SetActive(true);
        text.text = message;
    }

    public void ShowTextTimed(string message, float duration)
    {
        if (_currentRoutine != null)
            StopCoroutine(_currentRoutine);

        _currentRoutine = StartCoroutine(ShowRoutine(message, duration));
    }

    private IEnumerator ShowRoutine(string message, float duration)
    {
        ShowText(message);

        yield return new WaitForSeconds(duration);

        Hide();
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}