using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public enum DoorType
    {
        Sliding,
        Hinged
    }

    [Header("Door Type")]
    [SerializeField] private DoorType _doorType;

    [Header("Sliding Settings")]
    [SerializeField] private float _openHeight = 3f;

    [Header("Hinged Settings")]
    [SerializeField] private float _openAngle = 90f;

    [Header("Common Settings")]
    [SerializeField] private float _speed = 2f;

    [Header("Timing")]
    [SerializeField] private float _openDelay = 1.5f; // 👈 NUEVO

    private bool _isOpen = false;

    private Vector3 _closedPosition;
    private Vector3 _openPosition;

    private Quaternion _closedRotation;
    private Quaternion _openRotation;

    private void Start()
    {
        _closedPosition = transform.position;
        _closedRotation = transform.rotation;

        if (_doorType == DoorType.Sliding)
        {
            _openPosition = _closedPosition + Vector3.up * _openHeight;
        }
        else if (_doorType == DoorType.Hinged)
        {
            _openRotation = _closedRotation * Quaternion.Euler(0f, 0f, _openAngle);
        }
    }

    public void Open()
    {
        if (_isOpen) return;

        _isOpen = true;
        StartCoroutine(OpenWithDelay());
    }

    private IEnumerator OpenWithDelay()
    {
        // ⏳ Espera antes de abrir
        yield return new WaitForSeconds(_openDelay);

        if (_doorType == DoorType.Sliding)
        {
            yield return StartCoroutine(OpenSliding());
        }
        else if (_doorType == DoorType.Hinged)
        {
            yield return StartCoroutine(OpenHinged());
        }
    }

    private IEnumerator OpenSliding()
    {
        float time = 0f;

        while (time < 1f)
        {
            time += Time.deltaTime * _speed;

            float t = Mathf.SmoothStep(0f, 1f, time);

            transform.position = Vector3.Lerp(
                _closedPosition,
                _openPosition,
                t
            );

            yield return null;
        }

        transform.position = _openPosition;
    }

    private IEnumerator OpenHinged()
    {
        float time = 0f;

        while (time < 1f)
        {
            time += Time.deltaTime * _speed;

            float t = Mathf.SmoothStep(0f, 1f, time);

            transform.rotation = Quaternion.Lerp(
                _closedRotation,
                _openRotation,
                t
            );

            yield return null;
        }

        transform.rotation = _openRotation;
    }
}