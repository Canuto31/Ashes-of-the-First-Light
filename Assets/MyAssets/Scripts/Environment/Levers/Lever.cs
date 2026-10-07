using System.Collections;
using Cinemachine;
using UnityEngine;

/// <summary>
/// Provides the runtime behavior and data owned by the lever component.
/// </summary>
public class Lever : MonoBehaviour, IInteractable
{

    #region Fields and Configuration

    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera _targetCamera;
    [SerializeField] private float _cameraDuration = 2f;

    [Header("Door")]
    [SerializeField] private DoorController _targetDoor;

    [Header("Requirements")]
    [SerializeField] private InventoryItem _requiredItem;
    [SerializeField] private string _missingMessage = "You need something";

    private bool _hasBeenUsed;


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Returns whether requirement is currently true.
    /// </summary>
    private bool HasRequirement()
    {
        if (_requiredItem == null)
            return true;
        
        return PlayerInventory.Instance != null && PlayerInventory.Instance.HasItem(_requiredItem);
    }

    /// <summary>
    /// Returns the prompt text presented for this interaction.
    /// </summary>
    public string GetInteractionText()
    {
        if (_hasBeenUsed)
            return "";

        if (!HasRequirement())
            return "Locked";

        return "Press E to interact";
    }

    /// <summary>
    /// Executes this object's response to a valid player interaction.
    /// </summary>
    public void Interact()
    {
        if (_hasBeenUsed)
            return;

        if (!HasRequirement())
        {
            ShowMissingRequirement();
            return;
        }

        StartCoroutine(LeverSequence());
    }

    /// <summary>
    /// Displays missing requirement for this component.
    /// </summary>
    private void ShowMissingRequirement()
    {
        InteractionUIManager.Instance?.Show(transform.parent, _missingMessage);
    }

    /// <summary>
    /// Executes the lever sequence operation for this component.
    /// </summary>
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

    #endregion
}
