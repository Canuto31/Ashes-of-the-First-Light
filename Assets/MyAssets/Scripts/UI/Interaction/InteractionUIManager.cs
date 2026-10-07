using TMPro;
using UnityEngine;

/// <summary>
/// Coordinates interaction uimanager state and operations for the game.
/// </summary>
public class InteractionUIManager : MonoBehaviour
{

    #region Fields and Configuration

    public static InteractionUIManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _text;

    [Header("Settings")]
    [SerializeField] private Vector3 _offset = new Vector3(0, 1f, 0);

    private Transform _currentTarget;


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
        Clear();
    }

    /// <summary>
    /// Applies frame-dependent presentation updates after regular Update callbacks complete.
    /// </summary>
    private void LateUpdate()
    {
        UpdatePromptPosition();
    }


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Refreshes prompt position for this component.
    /// </summary>
    private void UpdatePromptPosition()
    {
        if (_currentTarget == null)
            return;

        if (GameStateManager.Instance == null || !GameStateManager.Instance.IsPlaying())
        {
            _panel.SetActive(false);
            return;
        }
        
        if (!_panel.activeSelf)
            _panel.SetActive(true);

        transform.position = _currentTarget.position + _offset;
    }

    /// <summary>
    /// Displays  for this component.
    /// </summary>
    public void Show(Transform target, string message)
    {
        if (target == null)
            return;

        _currentTarget = target;
        _text.text = message;
        _panel.SetActive(true);
    }

    /// <summary>
    /// Hides visual for this component.
    /// </summary>
    public void HideVisual()
    {
        _panel.SetActive(false);
    }

    /// <summary>
    /// Clears  for this component.
    /// </summary>
    public void Clear()
    {
        _currentTarget = null;
        _panel.SetActive(false);
    }

    #endregion
}
