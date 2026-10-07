using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Presents and updates inventory item option user-interface data.
/// </summary>
public class InventoryItemOptionUI : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _title;

    public void Setup(InventoryItem item)
    {
        if (item == null)
            return;

        if (_icon != null)
            _icon.sprite = item.icon;

        if (_title != null)
            _title.text = item.itemName;
    }
}
