using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraDirector : MonoBehaviour
{
    public static CameraDirector Instance;

    [Header("Main Player Camera")]
    [SerializeField] private CinemachineVirtualCamera _playerCamera;

    [Header("Settings")]
    [SerializeField] private int _activePriority = 20;
    [SerializeField] private int _defaultPriority = 10;

    private bool _isPlayingCinematic = false;
    private Coroutine _currentRoutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void FocusOn(CinemachineVirtualCamera targetCamera, float duration)
    {
        if (_currentRoutine != null)
        {
            StopCoroutine(_currentRoutine);
        }

        _currentRoutine = StartCoroutine(FocusRoutine(targetCamera, duration));
    }

    private IEnumerator FocusRoutine(CinemachineVirtualCamera targetCamera, float duration)
    {
        _isPlayingCinematic = true;

        Debug.Log("Switching to target camera");

        // Activar cámara objetivo
        targetCamera.Priority = _activePriority;
        _playerCamera.Priority = _defaultPriority;

        yield return new WaitForSeconds(duration);

        Debug.Log("Returning to player camera");

        // Volver al jugador
        targetCamera.Priority = _defaultPriority;
        _playerCamera.Priority = _activePriority;

        _isPlayingCinematic = false;
        _currentRoutine = null;
    }

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