using System.Collections;
using TMPro;
using UnityEngine;

public class UI_Interaction : MonoBehaviour
{
    public static UI_Interaction Instance;

    [Header("References")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _text;

    [Header("Animation")]
    [SerializeField] private float _fadeDuration = 0.2f;

    private Coroutine _currentRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        HideImmediate();
    }

    public void ShowText(string message)
    {
        if (!GameStateManager.Instance.IsPlaying())
            return;

        _text.text = message;

        if (_currentRoutine != null)
            StopCoroutine(_currentRoutine);

        _currentRoutine = StartCoroutine(FadeCanvas(1f));
    }

    public void ShowTextTimed(string message, float duration)
    {
        if (_currentRoutine != null)
            StopCoroutine(_currentRoutine);

        _currentRoutine = StartCoroutine(ShowRoutine(message, duration));
    }

    private IEnumerator ShowRoutine(string message, float duration)
    {
        _text.text = message;

        yield return FadeCanvas(1f);

        yield return new WaitForSeconds(duration);

        yield return FadeCanvas(0f);
    }

    private IEnumerator FadeCanvas(float targetAlpha)
    {
        float startAlpha = _canvasGroup.alpha;

        float elapsed = 0f;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;

            _canvasGroup.alpha = Mathf.Lerp(
                startAlpha,
                targetAlpha,
                elapsed / _fadeDuration
            );

            yield return null;
        }

        _canvasGroup.alpha = targetAlpha;
    }

    public void Hide()
    {
        if (_currentRoutine != null)
            StopCoroutine(_currentRoutine);

        _currentRoutine = StartCoroutine(FadeCanvas(0f));
    }

    private void HideImmediate()
    {
        _canvasGroup.alpha = 0f;
    }
}