using System.Collections;
using Cinemachine;
using UnityEngine;

/// <summary>
/// Provides the runtime behavior and data owned by the camera director component.
/// </summary>
public class CameraDirector : MonoBehaviour
{

    #region Fields and Configuration

    public static CameraDirector Instance { get; private set; }

    [Header("Main Player Camera")]
    [SerializeField] private CinemachineVirtualCamera _playerCamera;

    [Header("Settings")]
    [SerializeField] private int _activePriority = 20;
    [SerializeField] private int _defaultPriority = 10;

    private bool _isPlayingCinematic;
    private Coroutine _currentRoutine;
    private CinemachineVirtualCamera _currentTargetCamera;


    #endregion

    #region Unity Lifecycle

    /// <summary>
    /// Caches required dependencies and initializes this component before other Unity callbacks run.
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Focuses on for this component.
    /// </summary>
    public void FocusOn(CinemachineVirtualCamera targetCamera, float duration)
    {
        if (targetCamera == null || _playerCamera == null)
            return;

        if (_currentRoutine != null)
        {
            StopCoroutine(_currentRoutine);
            RestorePlayerCamera();
        }

        _currentTargetCamera = targetCamera;
        _currentRoutine = StartCoroutine(FocusRoutine(targetCamera, duration));
    }

    /// <summary>
    /// Focuses routine for this component.
    /// </summary>
    private IEnumerator FocusRoutine(CinemachineVirtualCamera targetCamera, float duration)
    {
        _isPlayingCinematic = true;

        targetCamera.Priority = _activePriority;
        _playerCamera.Priority = _defaultPriority;

        yield return new WaitForSeconds(duration);

        RestorePlayerCamera();
    }

    /// <summary>
    /// Updates camera for this component.
    /// </summary>
    public void SetCamera(CinemachineVirtualCamera targetCamera)
    {
        if (_isPlayingCinematic || targetCamera == null || _playerCamera == null)
            return;

        targetCamera.Priority = _activePriority;
        _playerCamera.Priority = _defaultPriority;
    }

    /// <summary>
    /// Returns whether playing cinematic is currently true.
    /// </summary>
    public bool IsPlayingCinematic()
    {
        return _isPlayingCinematic;
    }

    /// <summary>
    /// Executes the restore player camera operation for this component.
    /// </summary>
    private void RestorePlayerCamera()
    {
        if (_currentTargetCamera != null)
            _currentTargetCamera.Priority = _defaultPriority;

        if (_playerCamera != null)
            _playerCamera.Priority = _activePriority;

        _currentTargetCamera = null;
        _isPlayingCinematic = false;
        _currentRoutine = null;
    }

    #endregion
}
