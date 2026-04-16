using TMPro;
using UnityEngine;

public class InteractionUIManager : MonoBehaviour
{
    public static InteractionUIManager Instance;

    [Header("References")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _text;

    [Header("Settings")]
    [SerializeField] private Vector3 _offset = new Vector3(0, 1f, 0);

    private Transform _currentTarget;

    private void Awake() {
        Instance = this;
        Hide();
    }

    private void LateUpdate() {
        if (_currentTarget == null) return;

        transform.position = _currentTarget.position + _offset;
    }

    public void Show(Transform target, string message) {
        _currentTarget = target;
        _text.text = message;
        _panel.SetActive(true);
    }

    public void Hide() {
        _currentTarget = null;
        _panel.SetActive(false);
    }
}
