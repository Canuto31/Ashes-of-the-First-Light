using UnityEngine;

/// <summary>
/// Provides the runtime behavior and data owned by the billboard component.
/// </summary>
public class Billboard : MonoBehaviour
{

    #region Fields and Configuration

    private Camera _cam;


    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Initializes runtime state after all scene objects have completed their Awake phase.
    /// </summary>
    private void Start()
    {
        _cam = Camera.main;
    }

    /// <summary>
    /// Applies frame-dependent presentation updates after regular Update callbacks complete.
    /// </summary>
    private void LateUpdate()
    {
        if (_cam == null)
            return;

        transform.forward = _cam.transform.forward;
    }

    #endregion
}
