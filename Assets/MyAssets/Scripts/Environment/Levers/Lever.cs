using System.Collections;
using Cinemachine;
using UnityEngine;

/// <summary>
/// Provides the runtime behavior and data owned by the lever component.
/// </summary>
public class Lever : MonoBehaviour, IInteractable
{
    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera _targetCamera;
    [SerializeField] private float _cameraDuration = 2f;

    [Header("Door")]
    [SerializeField] private DoorController _targetDoor;

    [Header("Requirements")]
    [SerializeField] private InventoryItem _requiredItem;
    [SerializeField] private string _missingMessage = "You need something";

    private bool _hasBeenUsed;

    private bool HasRequirement()
    {
        if (_requiredItem == null)
            return true;
        
        return PlayerInventory.Instance != null && PlayerInventory.Instance.HasItem(_requiredItem);
    }

    public string GetInteractionText()
    {
        if (_hasBeenUsed)
            return "";

        if (!HasRequirement())
            return "Locked";

        return "Press E to interact";
    }

    public void Interact()
    {
        if (_hasBeenUsed)
            return;

        if (!HasRequirement())
        {
            InteractionUIManager.Instance?.Show(transform.parent, _missingMessage);
            return;
        }

        StartCoroutine(LeverSequence());
    }

    private IEnumerator LeverSequence()
    {
        _hasBeenUsed = true;

        CameraDirector.Instance?.FocusOn(_targetCamera, _cameraDuration);

        yield return new WaitForSeconds(1f);

        if (_targetDoor != null)
        {
            _targetDoor.Open();
            InteractionUIManager.Instance?.Clear();
        }
    }
}
