using System.Collections;
using Cinemachine;
using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera _targetCamera;
    [SerializeField] private float _cameraDuration = 2f;

    [Header("Door")]
    [SerializeField] private DoorController _targetDoor;

    [Header("Requirements")]
    [SerializeField] private bool _requiresKey = true;
    [SerializeField] private string _missingRequirementMessage = "It's loocked...";

    [SerializeField] private bool _isUnlocked = false;
    private bool _hasBeenUsed = false;

    public string GetInteractionText()
    {
        if (_hasBeenUsed) return "";

        if (!_isUnlocked) return "Locked";

        return "Press E to interact";
    }

    public void Interact()
    {
        if (_hasBeenUsed) return;

        if (!_isUnlocked) {
            InteractionUIManager.Instance.Show(transform.parent, _missingRequirementMessage);
            return;
        }

        StartCoroutine(LeverSequence());
    }

    private IEnumerator LeverSequence()
    {
        _hasBeenUsed = true;

        //GetComponent<Collider2D>().enabled = false;

        CameraDirector.Instance.FocusOn(_targetCamera, _cameraDuration);

        yield return new WaitForSeconds(1f);

        if (_targetDoor != null)
        {
            _targetDoor.Open();
            InteractionUIManager.Instance.Hide();
        }
    }

    public void Unlock() {
        _isUnlocked = true;
    }
}
