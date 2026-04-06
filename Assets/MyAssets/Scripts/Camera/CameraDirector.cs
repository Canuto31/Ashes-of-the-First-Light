using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraDirector : MonoBehaviour
{
    public static CameraDirector Instance;

    [Header("Main Player Camera")]
    [SerializeField] private CinemachineVirtualCamera _playerCamera;

    [Header("Settings ")]
    [SerializeField] private int _activePriority = 20;
    [SerializeField] private int _defaultPriority = 10;

    private bool _isPlayingCinematic = false;

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Focus on a target camera for a duration, then return to player camera
    /// </summary>
    public void FocusOn(CinemachineVirtualCamera targetCamera, float duration)
    {
        if (_isPlayingCinematic) return;

        StartCoroutine(FocusRoutine(targetCamera, duration));
    }

    private IEnumerator FocusRoutine(CinemachineVirtualCamera targetCamera, float duration)
    {
        _isPlayingCinematic = true;

        // Activate target camera
        targetCamera.Priority = _activePriority;
        _playerCamera.Priority = _defaultPriority;

        yield return new WaitForSeconds(duration);

        // Return to player camera
        targetCamera.Priority = _defaultPriority;
        _playerCamera.Priority = _activePriority;

        _isPlayingCinematic = false;
    }

    /// <summary>
    /// Force change without duration
    /// </summary>
    public void SetCamera(CinemachineVirtualCamera targetCamera)
    {
        if (_isPlayingCinematic) return;

        targetCamera.Priority = _activePriority;
        _playerCamera.Priority = _defaultPriority;
    }

    public bool IsPlayingCinematic()
    {
        return _isPlayingCinematic;
    }
}
