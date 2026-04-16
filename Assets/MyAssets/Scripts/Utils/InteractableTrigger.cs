using UnityEngine;

public class InteractableTrigger : MonoBehaviour
{
    private IInteractable _interactable;
    private PlayerInputHandler _playerInput;

    private bool _playerInside = false;

    private void Awake()
    {
        _interactable = GetComponentInParent<IInteractable>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _playerInside = true;
        _playerInput = other.GetComponent<PlayerInputHandler>();

        InteractionUIManager.Instance.Show(
            transform.parent,
            _interactable.GetInteractionText()
        );

        //UI_Interaction.Instance.ShowText(_interactable.GetInteractionText());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _playerInside = false;
        _playerInput = null;

        InteractionUIManager.Instance.Hide();

        //UI_Interaction.Instance.Hide();
    }

    private void Update()
    {
        if (!_playerInside || _playerInput == null) return;

        if (_playerInput.InteractPressed)
        {
            _interactable.Interact();
        }
    }
}
