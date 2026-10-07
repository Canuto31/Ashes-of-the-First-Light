using Cinemachine;
using UnityEngine;

/// <summary>
/// Provides the runtime behavior and data owned by the camera trigger component.
/// </summary>
public class CameraTrigger : MonoBehaviour
{

    #region Fields and Configuration

    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera _cameraTarget;
    
    [Header("Settings")]
    [SerializeField] private float _duration = 2f;
    [SerializeField] private bool _triggerOnce = true;

    [Header("Interaction")]
    [SerializeField] private bool _requireInput = false;

    private bool _hasTriggered;
    private bool _playerInside;
    private IInteractable _interactable;

    private PlayerInputHandler _playerInput;


    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Caches required dependencies and initializes this component before other Unity callbacks run.
    /// </summary>
    private void Awake()
    {
        _interactable = GetComponentInParent<IInteractable>();
    }

    /// <summary>
    /// Registers an eligible collider when it enters this component's trigger.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (!other.CompareTag("Player"))
            return;

        _playerInside = true;

        _playerInput = other.GetComponent<PlayerInputHandler>();

        if (_interactable != null)
            UI_Interaction.Instance?.ShowText(_interactable.GetInteractionText());

        /*if (!_requireInput)
        {
            TryActivate();
        }*/
    }

    /// <summary>
    /// Clears the registered collider when it leaves this component's trigger.
    /// </summary>
    private void OnTriggerExit2D(Collider2D other) 
    {
        if (!other.CompareTag("Player"))
            return;

        _playerInside = false;
        _playerInput = null;

        UI_Interaction.Instance?.Hide();
    }

    /// <summary>
    /// Coordinates frame-based input and state updates for this component.
    /// </summary>
    private void Update()
    {
        HandleInteractionInput();
    }


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Processes interaction input for this component.
    /// </summary>
    private void HandleInteractionInput()
    {
        if (!_requireInput || !_playerInside)
            return;

        if (_playerInput == null || _interactable == null)
            return;

        if (_playerInput.InteractPressed)
            _interactable.Interact();
    }

    /// <summary>
    /// Attempts to activate for this component.
    /// </summary>
    private void TryActivate()
    {
        if (_hasTriggered && _triggerOnce)
            return;

        if (_cameraTarget == null)
        {
            Debug.LogWarning("Camera target is not assigned in CameraTrigger on " + gameObject.name);
            return;
        }

        CameraDirector.Instance?.FocusOn(_cameraTarget, _duration);

        _hasTriggered = true;
    }

    #endregion
}
