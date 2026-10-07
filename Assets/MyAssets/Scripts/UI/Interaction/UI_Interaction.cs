using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Provides the runtime behavior and data owned by the ui interaction component.
/// </summary>
public class UI_Interaction : MonoBehaviour
{

    #region Fields and Configuration

    public static UI_Interaction Instance { get; private set; }

    [Header("References")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _text;

    [Header("Animation")]
    [SerializeField] private float _fadeDuration = 0.2f;

    private Coroutine _currentRoutine;


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

        HideImmediate();
    }


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Displays text for this component.
    /// </summary>
    public void ShowText(string message)
    {
        if (GameStateManager.Instance == null || !GameStateManager.Instance.IsPlaying())
            return;

        _text.text = message;

        if (_currentRoutine != null)
            StopCoroutine(_currentRoutine);

        _currentRoutine = StartCoroutine(FadeCanvas(1f));
    }

    /// <summary>
    /// Displays text timed for this component.
    /// </summary>
    public void ShowTextTimed(string message, float duration)
    {
        if (_currentRoutine != null)
            StopCoroutine(_currentRoutine);

        _currentRoutine = StartCoroutine(ShowRoutine(message, duration));
    }

    /// <summary>
    /// Displays routine for this component.
    /// </summary>
    private IEnumerator ShowRoutine(string message, float duration)
    {
        _text.text = message;

        yield return FadeCanvas(1f);

        yield return new WaitForSeconds(duration);

        yield return FadeCanvas(0f);
    }

    /// <summary>
    /// Executes the fade canvas operation for this component.
    /// </summary>
    private IEnumerator FadeCanvas(float targetAlpha)
    {
        if (_fadeDuration <= 0f)
        {
            _canvasGroup.alpha = targetAlpha;
            yield break;
        }

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

    /// <summary>
    /// Hides  for this component.
    /// </summary>
    public void Hide()
    {
        if (_currentRoutine != null)
            StopCoroutine(_currentRoutine);

        _currentRoutine = StartCoroutine(FadeCanvas(0f));
    }

    /// <summary>
    /// Hides immediate for this component.
    /// </summary>
    private void HideImmediate()
    {
        _canvasGroup.alpha = 0f;
        _currentRoutine = null;
    }

    #endregion
}
