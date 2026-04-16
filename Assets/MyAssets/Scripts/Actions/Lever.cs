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

    public string GetInteractionText()
    {
        return "Press E to interact";
    }

    public void Interact()
    {
        StartCoroutine(LeverSequence());
    }

    private IEnumerator LeverSequence()
    {
        CameraDirector.Instance.FocusOn(_targetCamera, _cameraDuration);

        yield return new WaitForSeconds(1f);

        if (_targetDoor != null)
        {
            _targetDoor.Open();
        }
    }
}
