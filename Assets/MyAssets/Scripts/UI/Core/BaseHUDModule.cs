using UnityEngine;

/// <summary>
/// Provides the runtime behavior and data owned by the base hudmodule component.
/// </summary>
public abstract class BaseHUDModule : MonoBehaviour
{
    [SerializeField] protected CanvasGroup _canvasGroup;

    protected virtual void Awake()
    {
        if (_canvasGroup == null)
            _canvasGroup = GetComponent<CanvasGroup>();
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);

        if (_canvasGroup == null)
            return;

        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);

        if (_canvasGroup == null)
            return;

        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    public virtual void SetOpacity(float opacity)
    {
        if (_canvasGroup != null)
            _canvasGroup.alpha = Mathf.Clamp01(opacity);
    }
}
