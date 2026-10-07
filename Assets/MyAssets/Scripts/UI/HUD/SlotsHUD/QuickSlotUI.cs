using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Presents and updates quick slot user-interface data.
/// </summary>
public class QuickSlotUI : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Image _itemIcon;
    [SerializeField] private Image _lockIcon;
    [SerializeField] private TextMeshProUGUI _keyLabel;

    public void Setkey(string key)
    {
        if (_keyLabel != null)
            _keyLabel.text = key;
    }

    public void SetState(QuickSlotState state)
    {
        bool isLocked = state == QuickSlotState.Locked;

        if (_lockIcon != null)
            _lockIcon.gameObject.SetActive(isLocked);

        if (_itemIcon != null)
            _itemIcon.gameObject.SetActive(!isLocked);
    }
}
