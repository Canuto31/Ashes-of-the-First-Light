using UnityEngine;

/// <summary>
/// Provides the runtime behavior and data owned by the interactable trigger component.
/// </summary>
public class InteractableTrigger : MonoBehaviour
{

    #region Fields and Configuration

    private IInteractable _interactable;
    private PlayerInputHandler _playerInput;

    private bool _playerInside;


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
        if (!other.CompareTag("Player") || _interactable == null)
            return;

        _playerInside = true;
        _playerInput = other.GetComponent<PlayerInputHandler>();

        InteractionUIManager.Instance?.Show(
            transform.parent,
            _interactable.GetInteractionText()
        );

        //UI_Interaction.Instance.ShowText(_interactable.GetInteractionText());
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

        InteractionUIManager.Instance?.Clear();

        //UI_Interaction.Instance.Hide();
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
        if (!IsInteractionAvailable())
            return;

        if (_playerInput.InteractPressed)
            _interactable.Interact();
    }

    /// <summary>
    /// Returns whether interaction available is currently true.
    /// </summary>
    private bool IsInteractionAvailable()
    {
        return GameStateManager.Instance != null &&
               GameStateManager.Instance.IsPlaying() &&
               _playerInside &&
               _playerInput != null &&
               _interactable != null;
    }

    #endregion
}
