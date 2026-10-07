using UnityEngine;

/// <summary>
/// Provides the runtime behavior and data owned by the base hudmodule component.
/// </summary>
public abstract class BaseHUDModule : MonoBehaviour
{

    #region Fields and Configuration

    [SerializeField] protected CanvasGroup _canvasGroup;


    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Caches required dependencies and initializes this component before other Unity callbacks run.
    /// </summary>
    protected virtual void Awake()
    {
        if (_canvasGroup == null)
            _canvasGroup = GetComponent<CanvasGroup>();
    }


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Displays  for this component.
    /// </summary>
    public virtual void Show()
    {
        gameObject.SetActive(true);

        if (_canvasGroup == null)
            return;

        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// Hides  for this component.
    /// </summary>
    public virtual void Hide()
    {
        gameObject.SetActive(false);

        if (_canvasGroup == null)
            return;

        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// Updates opacity for this component.
    /// </summary>
    public virtual void SetOpacity(float opacity)
    {
        if (_canvasGroup != null)
            _canvasGroup.alpha = Mathf.Clamp01(opacity);
    }

    #endregion
}
