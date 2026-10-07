using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Presents and updates quick slot user-interface data.
/// </summary>
public class QuickSlotUI : MonoBehaviour
{

    #region Fields and Configuration

    [Header("References")] 
    [SerializeField] private Image _itemIcon;
    [SerializeField] private Image _lockIcon;
    [SerializeField] private TextMeshProUGUI _keyLabel;


    #endregion

    #region Runtime Behavior

    /// <summary>
    /// Updates ey for this component.
    /// </summary>
    public void Setkey(string key)
    {
        if (_keyLabel != null)
            _keyLabel.text = key;
    }

    /// <summary>
    /// Updates state for this component.
    /// </summary>
    public void SetState(QuickSlotState state)
    {
        bool isLocked = state == QuickSlotState.Locked;

        if (_lockIcon != null)
            _lockIcon.gameObject.SetActive(isLocked);

        if (_itemIcon != null)
            _itemIcon.gameObject.SetActive(!isLocked);
    }

    #endregion
}
