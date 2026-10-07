using UnityEngine;

/// <summary>
/// Provides the runtime behavior and data owned by the billboard component.
/// </summary>
public class Billboard : MonoBehaviour
{
    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
    }

    private void LateUpdate()
    {
        if (_cam == null)
            return;

        transform.forward = _cam.transform.forward;
    }
}
