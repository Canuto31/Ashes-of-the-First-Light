using Cinemachine;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera _cameraTarget;
    
    [Header("Settings")]
    [SerializeField] private float _duration = 2f;
    [SerializeField] private bool _triggerOnce = true;

    [Header("Interaction")]
    [SerializeField] private bool _requireInput = false;

    private bool _hasTriggered = false;
    private bool _playerInside = false;
    private IInteractable _interactable;

    private PlayerInputHandler _playerInput;

    private void Awake() {
        _interactable = GetComponentInParent<IInteractable>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (!other.CompareTag("Player")) return;

        _playerInside = true;

        _playerInput = other.GetComponent<PlayerInputHandler>();

        UI_Interaction.Instance.ShowText(_interactable.GetInteractionText());

        /*if (!_requireInput)
        {
            TryActivate();
        }*/
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (!other.CompareTag("Player")) return;

        _playerInside = false;
        _playerInput = null;

        UI_Interaction.Instance.Hide();
    }

    private void Update() {
        if (_requireInput && _playerInside)
        {
            if (_playerInput.InteractPressed)
            {
                _interactable.Interact();
            }
        }
    }

    private void TryActivate()
    {
        if (_hasTriggered && _triggerOnce) return;

        if (_cameraTarget == null)
        {
            Debug.LogWarning("Camera target is not assigned in CameraTrigger on " + gameObject.name);
            return;
        }

        CameraDirector.Instance.FocusOn(_cameraTarget, _duration);

        _hasTriggered = true;
    }
}
