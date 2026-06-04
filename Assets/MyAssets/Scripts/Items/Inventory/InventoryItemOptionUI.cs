using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemOptionUI : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _title;

    public void Setup(InventoryItem item)
    {
        _icon.sprite = item.icon;
        _title.text = item.itemName;
    }
}
