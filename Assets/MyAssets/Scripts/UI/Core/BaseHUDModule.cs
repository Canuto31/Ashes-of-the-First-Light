using System;
using UnityEngine;

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
        
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
        
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    public virtual void SetOpacity(float opacity)
    {
        _canvasGroup.alpha = opacity;
    }
}
